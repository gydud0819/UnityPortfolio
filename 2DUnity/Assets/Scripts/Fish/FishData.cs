using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "ScriptableObjects/FishData")]
public class FishData : ScriptableObject
{
    public string fishName;
    public Sprite fishSprite;
    [TextArea] public string description;
}
