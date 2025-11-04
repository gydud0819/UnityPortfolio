using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI countText;
    public bool IsEmpty { get; private set; } = true;
    public Sprite CurrentSprite { get; private set; }
    private int itemCount = 0;


    public void SetItem(Sprite newIcon)
    {
        // 기존 아이템과 동일한 경우 → 수량만 증가
        if (!IsEmpty && CurrentSprite == newIcon)
        {
            itemCount++;
            UpdateCountText();
            return;
        }

        // 새 아이템이 들어올 경우
        CurrentSprite = newIcon;
        itemIcon.sprite = newIcon;
        itemIcon.enabled = true;
        IsEmpty = false;

        itemCount = 1;
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

        if (itemCount <= 1)
            countText.text = "";   // 1마리면 숫자 안 보이게
        else
            countText.text = itemCount.ToString();
    }
}
