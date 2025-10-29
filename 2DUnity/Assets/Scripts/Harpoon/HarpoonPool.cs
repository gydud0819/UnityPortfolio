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

            HarpoonTip tip = obj.GetComponent<HarpoonTip>();
            if (tip != null)
                tip.SetPool(this);

            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetHarpoon()
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(harpoonTipPrefab, transform);

            // ���� ������ �ۻ쿡�� HarpoonPool ������ �����Ƿ� �־���� ��
            HarpoonTip tip = obj.GetComponent<HarpoonTip>();
            if (tip != null)
                tip.SetPool(this);
        }

        obj.SetActive(true);
        return obj;
    }

    public void ReturnHarpoon(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
