using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpwaner : MonoBehaviour
{
    [SerializeField] private List<GameObject> fishPrefabs;
    [SerializeField] private float fishInterval = 2f;
    [SerializeField] private float fishSpawnX = 5f;
    [SerializeField] private float fishSpawnY = 1f;

    private void Start()
    {
        StartCoroutine(FishSpwan());
    }

    private IEnumerator FishSpwan()
    {
        while (true)
        {
            yield return new WaitForSeconds(fishInterval);

            GameObject randomFish = fishPrefabs[Random.Range(0, fishPrefabs.Count)];

            Vector2 randomPos = new Vector2(transform.position.x + Random.Range(-fishSpawnX, fishSpawnX), transform.position.y + Random.Range(-fishSpawnY, fishSpawnY));

            Instantiate(randomFish, randomPos, Quaternion.identity);
        }
    }
}
