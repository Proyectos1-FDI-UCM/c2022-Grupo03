using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties
    bool _spinTextAppears;
    bool _rayTextAppears;
    #endregion

    #region references
    [SerializeField]
    private GameObject _currentColorObject;
    private Image _currentColorImage;
    [SerializeField]
    private GameObject _spinCooldownObject;
    private Text _spinCooldownText;
    private Image _imageSpinCd;
    [SerializeField]
    private GameObject _rayCooldownObject;
    private Text _rayCooldownText;
    private Image _imageRayCd;
    [SerializeField]
    private GameObject _enemiesLeftObject;
    private Text _enemiesLeftText;
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _level;
    private Text _levelText;
    [SerializeField]
    private GameObject _playerLifeBar;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private GameObject _timer;    
    [SerializeField]
    private GameObject _bossBar;
    #endregion

    #region methods
    // HUD del juego
    public void UpdateCurrentColor(Color newColor)
    {
        _currentColorImage.color = newColor;
    }

    // HUD relativa al tiempo de espera de las habilidades
    public void UpdateSpinCooldown(float cd, float duration)
    {
        UpdateCooldown(cd, duration, _spinCooldownText.gameObject, _imageSpinCd, _spinCooldownText, ref _spinTextAppears);
    }
    public void UpdateRayCooldown(float cd, float duration)
    {
        UpdateCooldown(cd, duration, _rayCooldownText.gameObject, _imageRayCd, _rayCooldownText, ref _rayTextAppears);
    }
    private void UpdateCooldown(float cd, float duration, GameObject cdObject, Image imageCd, Text cdText, ref bool textAppears)
    {
        if (cd < 0f)
        {
            cdObject.SetActive(false);
            imageCd.fillAmount = 0f;
            textAppears = false;
        }
        else
        {
            // este condicional sirve para hacer que la instrucción
            // que hace que el texto aparezca en pantalla solo se ejecute una vez
            if (!textAppears)
            {
                cdObject.SetActive(true);
                textAppears = true;
            }
            cdText.text = ((int)cd).ToString();
            imageCd.fillAmount = cd / duration;
        }
    }

    public void UpdateEnemiesLeft(int numEnemies)
    {
        if (numEnemies < 0)
            numEnemies = 0;
        
        _enemiesLeftText.text = "Enemigos: " + numEnemies;
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
    public void Resume()
    {
        GameManager.Instance.Resume();
    }
    public void BackToTitle()
    {
        GameManager.Instance.BackToTitle();
    }

    public void LevelMessage(string newText)
    {
        _levelText.text = newText;
    }

    public void SetLvMessage(bool enabled)
    {
        _level.SetActive(enabled);
    }

    public void SetRayCooldown(bool enabled)
    {
        _rayCooldownObject.SetActive(enabled);
    }

    public void SetSpinCooldown(bool enabled)
    {
        _spinCooldownObject.SetActive(enabled);
    }

    public void SetCurrentColor(bool enabled)
    {
        _currentColorObject.SetActive(enabled);
    }

    public void SetEnemiesLeft(bool enabled)
    {
        _enemiesLeftObject.SetActive(enabled);
    }

    public void SetPlayerLifeBar(bool enabled)
    {
        _playerLifeBar.SetActive(enabled);
    }

    public void SetTimer(bool enabled)
    {
        _timer.SetActive(enabled);
    }    
    public void SetBossBar(bool enabled)
    {
        if(_bossBar != null) _bossBar.SetActive(enabled);
    }


    private void Awake()
    {
        _imageSpinCd = _spinCooldownObject.GetComponentsInChildren<Image>()[1];
        _imageRayCd = _rayCooldownObject.GetComponentsInChildren<Image>()[1];
        _enemiesLeftText = _enemiesLeftObject.GetComponent<Text>();
        _currentColorImage = _currentColorObject.GetComponent<Image>();
        _spinCooldownText = _spinCooldownObject.GetComponentInChildren<Text>();
        _rayCooldownText = _rayCooldownObject.GetComponentInChildren<Text>();
        _levelText = _level.GetComponent<Text>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _spinCooldownText.gameObject.SetActive(false);
        _imageSpinCd.fillAmount = 0f;
        _rayCooldownText.gameObject.SetActive(false);
        _imageRayCd.fillAmount = 0f;
        _spinTextAppears = _rayTextAppears = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Options"))
        {
            PauseMenu();
        }
        timerText.text = GameManager.Instance.GetWaveTime().ToString();
    }
}
