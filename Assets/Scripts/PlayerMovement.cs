using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float playerSpeed = 3;
    public float horizontalSpeed = 3;
    public float rightLimit = 4.5f;
    public float leftLimit = -4.5f;
    public float jumpForce = 5f;

    [Header("Slide Settings")]
    public float slideDuration = 1f;
    public float slideDownForce = 8f;
    [SerializeField] private CapsuleCollider standingCollider;
    [SerializeField] private CapsuleCollider slideCollider;

    private Rigidbody rb;
    private Animator animator;

    private bool isGrounded = true;
    private bool isGameOver = false;
    private bool isSliding = false;


    [Header("Difficulty Scaling")]
    public float speedIncreaseRate = 0.1f;
    public float maxSpeed = 15f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        if (rb == null || animator == null)
        {
            Debug.LogError("Player missing Rigidbody or Animator!");
            enabled = false;
            return;
        }

        if (standingCollider == null || slideCollider == null)
        {
            Debug.LogError("Standing or Slide Collider not assigned in Inspector!");
            enabled = false;
            return;
        }

        // Başlangıç durumu: ayakta collider aktif, slide kapalı
        standingCollider.enabled = true;
        slideCollider.enabled = false;

        animator.Play("Running", 0);

        // Animator parametrelerini kontrol et
        bool hasIsSliding = false, hasIsJumping = false;
        foreach (var p in animator.parameters)
        {
            if (p.name == "isSliding") hasIsSliding = true;
            if (p.name == "isJumping") hasIsJumping = true;
        }

        if (!hasIsSliding)
            Debug.LogWarning("Animator missing 'isSliding' parameter. Sliding animation won't trigger.");
        if (!hasIsJumping)
            Debug.LogWarning("Animator missing 'isJumping' parameter. Jump animation won't trigger.");
    }

    void Update()
    {
        if (isGameOver) return;

        playerSpeed = Mathf.Min(playerSpeed + speedIncreaseRate * Time.deltaTime, maxSpeed);

        // İleri hareket
        transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);

        // Yatay hareket
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(Vector3.left * Time.deltaTime * horizontalSpeed, Space.World);

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.Translate(Vector3.right * Time.deltaTime * horizontalSpeed, Space.World);

        // Yatay sınırlar
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        transform.position = pos;

        // Zıplama
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded && !isSliding)        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            AudioManager.instance.PlayJump();
            animator.SetBool("isJumping", true);
            isGrounded = false;
        }

        // Slide
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Slide input detected");
            if (isGrounded && !isSliding)
                StartSlide();
            else
                Debug.Log("Cannot slide: grounded=" + isGrounded + " isSliding=" + isSliding);
        }
    }

    void StartSlide()
    {
        if (isSliding) return;
        StartCoroutine(SlideCoroutine());
    }

    IEnumerator SlideCoroutine()
    {
        isSliding = true;
        Debug.Log("StartSlide coroutine started");

        // 1. Collider değişimi: ayaktaki kapan, slide aç
        standingCollider.enabled = false;
        slideCollider.enabled = true;

        // 2. Animasyon tetikle
        AudioManager.instance.PlaySlide();
        animator.SetBool("isSliding", true);

        // 3. Fiziksel olarak aşağı it (yere yapışsın)
        rb.AddForce(Vector3.down * slideDownForce, ForceMode.Impulse);

        // 4. Slide süresi kadar bekle
        yield return new WaitForSeconds(slideDuration);

        Debug.Log("Slide duration completed, ending slide");
        EndSlide();
    }

    void EndSlide()
    {
        isSliding = false;
        Debug.Log("EndSlide called");

        // Collider'ları eski haline döndür
        slideCollider.enabled = false;
        standingCollider.enabled = true;
        animator.SetBool("isSliding", false);

    }

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle") && !isGameOver)
            {
                StartCoroutine(GameOverSequence());
            }
            else if (other.CompareTag("Coin"))
            {
                AudioManager.instance.PlayCoinCollect();
                GameManager.instance.AddCoin();
                Destroy(other.gameObject);
            }
        }

    IEnumerator GameOverSequence()
    {
        isGameOver = true;
        GameManager.instance.StopScoring();
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayImpact();
        CameraShake.instance.Shake();
        
        
        animator.SetTrigger("GameOver");
        rb.linearVelocity = Vector3.zero;
        
        yield return new WaitForSeconds(2f);
        GameManager.instance.GameOver();
    }
}