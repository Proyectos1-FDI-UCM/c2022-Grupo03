using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties
    bool _gameIsPaused;
    #endregion

    #region references
    [SerializeField]
    GameObject _currentColorObject;
    Text _currentColorText;
    [SerializeField]
    GameObject _spinCooldownObject;
    Text _spinCooldownText;
    [SerializeField]
    GameObject _rayCooldownObject;
    Text _rayCooldownText;
    [SerializeField]
    GameObject _enemiesLeftObject;
    Text _enemiesLeftText;
    [SerializeField]
    GameObject _pauseMenu;
    #endregion

    #region methods
    public void UpdateCurrentColor(string newColor)
    {
        _currentColorText.text = newColor;
    }
    public void UpdateSpinCooldown(int newTime)
    {
        _spinCooldownText.text = "Spin: " + newTime;
    }
    public void UpdateRayCooldown(int newTime)
    {
        _rayCooldownText.text = "Ray: " + newTime;
    }
    public void UpdateEnemiesLeft(int numEnemies)
    {
        _enemiesLeftText.text = "Enemies: " + numEnemies;
    }
    public void PauseMenu()
    {
        if (_gameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1f;
        _gameIsPaused = false;
    }

    private void Pause()
    {
        _pauseMenu.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0f;
        _gameIsPaused = true;
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void Awake()
    {
        _currentColorText = _currentColorObject.GetComponent<Text>();
        _spinCooldownText = _spinCooldownObject.GetComponent<Text>();
        _rayCooldownText = _rayCooldownObject.GetComponent<Text>();
        _enemiesLeftText = _enemiesLeftObject.GetComponent<Text>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _gameIsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
