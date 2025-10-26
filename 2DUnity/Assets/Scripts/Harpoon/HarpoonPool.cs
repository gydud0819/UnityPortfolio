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

        // ���������� �߰� ���� (�ʿ� ������ �ּ�ó�� ����)
        GameObject extra = Instantiate(harpoonTipPrefab, transform);
        return extra;
    }

    public void ReturnHarpoon(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
