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

    #endregion

    #region methods
    public void StartMatch()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Level1()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        PlayerPrefs.SetString("Back", "SelecNivel");
    }

    public void Level2()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        PlayerPrefs.SetString("Back", "SelecNivel");
    }

    public void Level3()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
        PlayerPrefs.SetString("Back", "SelecNivel");
    }

    public void PruebaMelee()
    {
        SceneManager.LoadScene(4, LoadSceneMode.Single);
        PlayerPrefs.SetString("Back", "Prueba");
    }

    public void PruebaDistancia()
    {
        SceneManager.LoadScene(5, LoadSceneMode.Single);
        PlayerPrefs.SetString("Back", "Prueba");
    }

    public void PruebaKamikaze()
    {
        SceneManager.LoadScene(6, LoadSceneMode.Single);
        PlayerPrefs.SetString("Back", "Prueba");
    }

    public void PruebaTanque()
    {
        SceneManager.LoadScene(7, LoadSceneMode.Single);
        PlayerPrefs.SetString("Back", "Prueba");
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
