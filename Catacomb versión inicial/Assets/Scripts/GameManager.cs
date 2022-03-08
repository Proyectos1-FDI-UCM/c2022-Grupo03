using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }

    private void OnPlayerDefeat()   // se llama cuando el jugador pierde
    {
        _player.SetActive(false);
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
        
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _deathAnimation = GetComponent<DeathAnimation>();
        muriendo = false;
        _myUIManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (!muriendo)
            _elapsedTime = 0;
        if (muriendo && _elapsedTime > tiempoMuerte)
            OnPlayerDefeat();
    }
}
