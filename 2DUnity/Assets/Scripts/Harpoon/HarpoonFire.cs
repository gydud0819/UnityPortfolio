using UnityEngine;

public class HarpoonFire : MonoBehaviour
{
    public GameObject harpoon;
    public Transform firePoint; // �ѱ� ��ġ

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            harpoon.SetActive(true);
        }
    }
}
