using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public string sceneName;

    private void Start()
    {
        // Show the menu panel when the game starts
        ShowMenu();
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
