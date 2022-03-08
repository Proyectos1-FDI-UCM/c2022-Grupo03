using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int tiempoMuerte = 1000;
    #endregion

    #region properties
    private bool muriendo;
    private float _elapsedTime;
    #endregion

    #region references
    private GameObject _player;
    private DeathAnimation _deathAnimation;
    private UI_Manager _myUIManager;
    private PlayerInputManager _playerInputManager;
    private PlayerMovementController _playerMovement;
    #endregion

    #region properties
    static private GameManager _instance;
    static public GameManager Instance // Accesor a la instancia del game manager 
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    #region methods
    public void OnPlayerDamage(int lifePoints)
    {
        _myUIManager.UpdatePlayerLife(lifePoints);
        if (lifePoints <= 0)
        {
            muriendo = _deathAnimation.DeathAni(); // animación de la muerte
            _playerInputManager.enabled = false;
            _playerMovement.SetMovementDirection(new Vector3 (0, 0, 0));
        }
    }

    private void OnPlayerDefeat()   // se llama cuando el jugador pierde
    {
        SceneManager.LoadScene(0);
        // _player.SetActive(false);
    }

    public void OnPlayerChangeColor(string color)
    {
        _myUIManager.UpdateCurrentColor(color);
    }

    public void OnSpinCooldown(float time)
    {
        _myUIManager.UpdateSpinCooldown((int)time);
    }

    public void OnRayCooldown(float time)
    {
        _myUIManager.UpdateRayCooldown((int)time);
    }

    private void Awake()
    {
        _instance = this;
        _myUIManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _deathAnimation = _player.GetComponent<DeathAnimation>();
        _playerInputManager = _player.GetComponent<PlayerInputManager>();
        _playerMovement = _player.GetComponent<PlayerMovementController>();
        muriendo = false;
        _elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (muriendo)
        {
            _elapsedTime += Time.deltaTime;
        }
        if (muriendo && _elapsedTime > tiempoMuerte)
        {
            OnPlayerDefeat();
        }
    }
}
