using System.Collections.Generic;
using UnityEngine;

public class HarpoonPool : MonoBehaviour
{
    [SerializeField] private int poolSize = 5;
    [SerializeField] private GameObject harpoonTipPrefab;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(harpoonTipPrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetHarpoon()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // 여유없으면 추가 생성 (필요 없으면 주석처리 가능)
        GameObject extra = Instantiate(harpoonTipPrefab, transform);
        return extra;
    }

    public void ReturnHarpoon(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
