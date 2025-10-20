using UnityEngine;

public class BackgroundVertical : MonoBehaviour
{
    public Transform player;
    public float switchHeight = 8f;
    public GameObject upperLayer;
    public GameObject lowerLayer;

    void Update()
    {
        if (player.position.y > switchHeight)
        {
            upperLayer.SetActive(true);
            lowerLayer.SetActive(false);
        }
        else
        {
            upperLayer.SetActive(false);
            lowerLayer.SetActive(true);
        }
    }
}
