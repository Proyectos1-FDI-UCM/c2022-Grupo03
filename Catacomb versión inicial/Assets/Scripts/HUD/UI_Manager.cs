using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties

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
    // HUD del juego
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

    // men� de pausa
    private void PauseMenu()
    {
        GameManager.Instance.PauseMenu();
    }
    public void SetPauseMenu(bool enabled)
    {
        _pauseMenu.SetActive(enabled);
    }
    public void Resume()
    {
        GameManager.Instance.Resume();
    }
    public void BackToTitle()
    {
        GameManager.Instance.BackToTitle();
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

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }
}
