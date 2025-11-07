using UnityEngine;

public class StorageButton : MonoBehaviour
{
    [SerializeField] private GameObject storageUI;

    public void ToggleStorage()
    {
        if (storageUI == null) return;

        bool isActive = storageUI.activeSelf;
        storageUI.SetActive(!isActive);
    }
}
