using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _tiempoMuerte = 1000;
    #endregion

    #region properties
    private bool _muriendo;
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
    List<EnemyLifeComponent> _listOfEnemies;
    #endregion

    #region methods
    public int NumRandom(int minInclusive, int maxInclusive)
    {
        return Random.Range(minInclusive, maxInclusive + 1);
    }

    public void OnEnemyDies(EnemyLifeComponent enemy)
    {
        if (_listOfEnemies.Contains(enemy))
        {
            _listOfEnemies.Remove(enemy);
            _myUIManager.UpdateEnemiesLeft(_listOfEnemies.Count);
        }
    }

    public void RegisterEnemy(EnemyLifeComponent enemy)
    {
        if (!_listOfEnemies.Contains(enemy))
        {
            _listOfEnemies.Add(enemy);
            _myUIManager.UpdateEnemiesLeft(_listOfEnemies.Count);
        }
    }

    public void OnPlayerDamage(int lifePoints)
    {
        if (lifePoints <= 0)
        {
            _muriendo = _deathAnimation.DeathAni(); // animación de la muerte
            _playerInputManager.enabled = false;
        }
    }

    private void OnPlayerDefeat()   // se llama cuando el jugador pierde
    {
        SceneManager.LoadScene(0);
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
        _listOfEnemies = new List<EnemyLifeComponent>();
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
        _muriendo = false;
        _elapsedTime = 0;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_muriendo)
        {
            _elapsedTime += Time.deltaTime;
            _playerMovement.SetMovementDirection(Vector3.zero);
            if (_elapsedTime > _tiempoMuerte)
            {
                OnPlayerDefeat();
            }
        }
    }
}
