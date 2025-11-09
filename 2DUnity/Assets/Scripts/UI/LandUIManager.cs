using UnityEngine.SceneManagement;
using UnityEngine;

public class LandUIManager : MonoBehaviour
{
    public GameObject confirmationPanel;
    public GameObject mainButtonsGroup;

    public void OnExploreButton()
    {
        confirmationPanel.SetActive(true);
        mainButtonsGroup.SetActive(false);
        GameManager.Instance.GoToOcean();
    }

    public void ConfirmExplore()
    {
        SceneManager.LoadScene("Ocean");
    }

    public void CancelExplore()
    {
        confirmationPanel.SetActive(true);
        mainButtonsGroup.SetActive(true);
    }
}
