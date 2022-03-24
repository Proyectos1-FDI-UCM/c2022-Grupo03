using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties

    #endregion

    #region references
    [SerializeField]
    GameObject _menu;
    [SerializeField]
    GameObject _optionsMenu;
    #endregion

    #region methods
    public void StartMatch()
    {
        PlayerPrefs.DeleteKey("Back");
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }
    public void GoToControllerMenu()
    {
        PlayerPrefs.DeleteKey("Back");
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("Back") == "TitleScreen")
        {
            _menu.SetActive(false);
            _optionsMenu.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
