using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI_Manager : MonoBehaviour
{
    public GameObject Setting;
    public GameObject MenuPanel;
    public GameObject RateUs;


    public void LoadPLayScene()
    {
        SceneManager.LoadSceneAsync("GamePlay_Menu");
    }
}