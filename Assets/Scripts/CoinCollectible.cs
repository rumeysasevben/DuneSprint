using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    public float rotateSpeed = 100f;
    public int coinValue = 1;

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }
}
