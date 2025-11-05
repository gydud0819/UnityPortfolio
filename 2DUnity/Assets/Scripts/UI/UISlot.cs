using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI countText;

    public bool IsEmpty { get; private set; } = true;
    public Sprite CurrentSprite { get; private set; }
    public FishType FishType { get; private set; }
    private int itemCount = 0;

    public void SetItem(Sprite newIcon, FishType fish)
    {
        if (newIcon== null) return;

        FishType = fish;
        CurrentSprite = newIcon;

        itemIcon.sprite = newIcon;
        itemIcon.enabled = true;
        IsEmpty = false;

        itemCount = 1;
        UpdateCountText();

        //// 새 아이템일 때만 초기화
        //if (IsEmpty)
        //{
        //    CurrentSprite = newIcon;
        //    itemIcon.sprite = newIcon;
        //    itemIcon.enabled = true;
        //    IsEmpty = false;
        //    itemCount = 1;
        //}
        //else if (CurrentSprite != null && CurrentSprite.name == newIcon.name)
        //{
        //    // 같은 아이템이면 수량만 증가
        //    itemCount++;
        //}

        //UpdateCountText();
    }

    public void AddCount()
    {
        itemCount++;
        UpdateCountText();
    }

    public void ClearItem()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        CurrentSprite = null;
        IsEmpty = true;
        itemCount = 0;
        UpdateCountText();
    }

    private void UpdateCountText()
    {
        if (countText == null) return;
       
        if(itemCount <= 1)
        {
            countText.text = "";
        }
        else
        {
            countText.text = itemCount.ToString();
        }
    }
}
