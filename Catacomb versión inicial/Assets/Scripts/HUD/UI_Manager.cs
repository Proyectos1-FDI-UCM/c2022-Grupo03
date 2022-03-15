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
    private GameObject _currentColorObject;
    private Text _currentColorText;
    [SerializeField]
    private GameObject _spinCooldownObject;
    Text _spinCooldownText;
    private Image _imageSpinCd;
    [SerializeField]
    private GameObject _rayCooldownObject;
    private Text _rayCooldownText;
    [SerializeField]
    private GameObject _enemiesLeftObject;
    private Text _enemiesLeftText;
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private GameObject _optionsMenu;
    #endregion

    #region methods
    // HUD del juego
    public void UpdateCurrentColor(string newColor)
    {
        _currentColorText.text = newColor;
    }
    public void UpdateSpinCooldown(float cd, float duration)
    {
        if (cd < 0f)
        {
            Debug.Log("desaparece el cd");
            _spinCooldownText.gameObject.SetActive(false);
            _imageSpinCd.fillAmount = 0f;
        }
        else
        {
            Debug.Log("aparece el cd");
            _spinCooldownText.gameObject.SetActive(true);
            _spinCooldownText.text = Mathf.RoundToInt(cd).ToString();
            _imageSpinCd.fillAmount = cd / duration;
        }
    }
    public void UpdateRayCooldown(int newTime)
    {
        _rayCooldownText.text = newTime.ToString();
    }
    public void UpdateEnemiesLeft(int numEnemies)
    {
        _enemiesLeftText.text = "Enemies: " + numEnemies;
    }

    // menú de pausa
    private void PauseMenu()
    {
        GameManager.Instance.PauseMenu();
    }
    public void SetPauseMenu(bool enabled)
    {
        _pauseMenu.SetActive(enabled);
    }
    public void SetMenu(bool enabled)
    {
        _menu.SetActive(false);
    }
    public void SetOptionsMenu(bool enabled)
    {
        _optionsMenu.SetActive(enabled);
    }
    public void Resume()
    {
        GameManager.Instance.Resume();
    }
    public void BackToTitle()
    {
        GameManager.Instance.BackToTitle();
    }
    public void GoToControllerMenu()
    {
        GameManager.Instance.GoToControllerMenu();
    }

    private void Awake()
    {
        _currentColorText = _currentColorObject.GetComponent<Text>();
        _spinCooldownText = _spinCooldownObject.GetComponentInChildren<Text>();
        _imageSpinCd = _spinCooldownObject.GetComponent<Image>();
        _rayCooldownText = _rayCooldownObject.GetComponentInChildren<Text>();
        _enemiesLeftText = _enemiesLeftObject.GetComponent<Text>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _imageSpinCd.fillAmount = 0f;
        _spinCooldownText.gameObject.SetActive(false);
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
