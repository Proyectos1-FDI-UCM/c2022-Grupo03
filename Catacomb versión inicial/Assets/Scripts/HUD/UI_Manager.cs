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
    Text _spinCooldownText;
    [SerializeField]
    private Image _imageSpinCd;
    [SerializeField]
    private GameObject _rayCooldownObject;
    private Text _rayCooldownText;
    [SerializeField]
    private Image _imageRayCd;
    [SerializeField]
    private GameObject _enemiesLeftObject;
    private Text _enemiesLeftText;
    [SerializeField]
    private GameObject _pauseMenu;
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
        UpdateCooldown(cd, duration, _spinCooldownObject, _imageSpinCd, _spinCooldownText, ref _spinTextAppears);
    }
    public void UpdateRayCooldown(float cd, float duration)
    {
        UpdateCooldown(cd, duration, _rayCooldownObject, _imageRayCd, _rayCooldownText, ref _rayTextAppears);
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

    private void Awake()
    {
        _currentColorImage = _currentColorObject.GetComponent<Image>();
        _spinCooldownText = _spinCooldownObject.GetComponent<Text>();
        _rayCooldownText = _rayCooldownObject.GetComponent<Text>();
        _enemiesLeftText = _enemiesLeftObject.GetComponent<Text>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _spinCooldownObject.SetActive(false);
        _imageSpinCd.fillAmount = 0f;
        _rayCooldownObject.SetActive(false);
        _imageRayCd.fillAmount = 0f;
        _spinTextAppears = _rayTextAppears = false;
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
