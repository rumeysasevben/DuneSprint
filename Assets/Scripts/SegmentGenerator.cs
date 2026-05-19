using System.Collections.Generic;
using UnityEngine;

public class SegmentGenerator : MonoBehaviour
{
    public GameObject[] segmentPrefabs;
    public GameObject slideSectionPrefab;
    public GameObject normalSectionPrefab;
    public Transform player;
    public float segmentLength = 50f;
    public int segmentsAhead = 3;
    [Range(0,100)]
    public int slideChance = 20;

    [Header("Coin Settings")]
    public GameObject coinPrefab;
    public int minCoins = 3;
    public int maxCoins = 7;
    public float coinY = 1.5f;
    public float coinScale = 0.1f;
    public float[] lanes = { -3f, 0f, 3f };

    private List<GameObject> activeSegments = new List<GameObject>();
    private float nextSpawnZ = 0f;

    void Start()
    {
        for (int i = 0; i < segmentsAhead; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        if (player.position.z + segmentLength * segmentsAhead > nextSpawnZ)
        {
            SpawnSegment();
            if (activeSegments.Count > segmentsAhead + 1)
            {
                Destroy(activeSegments[0]);
                activeSegments.RemoveAt(0);
            }
        }
    }

    void SpawnSegment()
    {
        int roll = Random.Range(0, 100);
        GameObject prefabToSpawn = roll < slideChance ? slideSectionPrefab : normalSectionPrefab;
        GameObject seg = Instantiate(prefabToSpawn, new Vector3(0, 0, nextSpawnZ), Quaternion.identity);
        SpawnCoins(seg);
        activeSegments.Add(seg);
        nextSpawnZ += segmentLength;
    }

    void SpawnCoins(GameObject segment)
    {
        if (coinPrefab == null) return;

        int coinCount = Random.Range(minCoins, maxCoins + 1);
        float segmentStartZ = segment.transform.position.z;

        for (int i = 0; i < coinCount; i++)
        {
            float x = lanes[Random.Range(0, lanes.Length)];
            float z = segmentStartZ + Random.Range(5f, segmentLength - 5f);
            Vector3 pos = new Vector3(x, coinY, z);

            GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
            coin.transform.localScale = Vector3.one * coinScale;
            coin.tag = "Coin";
            coin.transform.parent = segment.transform;
        }
    }
}