using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMenu : MonoBehaviour
{
    #region parameters
    
    #endregion

    #region properties

    #endregion

    #region references

    #endregion

    #region methods
    public void Back()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("Back"), LoadSceneMode.Single);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
