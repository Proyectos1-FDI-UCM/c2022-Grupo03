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
    bool _gameIsPaused;
    #endregion

    #region references
    private GameObject _player;
    private UI_Manager _myUIManager;
    private PlayerInputManager _playerInputManager;
    private DeathAnimation _playerDeathAnimation;
    private PlayerMovementController _playerMovementController;
    private DirectionArrow _directionArrow;
    #endregion

    #region properties
    static private GameManager _instance;
    static public GameManager Instance // accesor a la instancia del game manager 
    {
        get
        {
            return _instance;
        }
    }
    List<EnemyLifeComponent> _listOfEnemies;
    #endregion

    #region methods
    // generador de n�meros aleatorios
    public int NumRandom(int minInclusive, int maxInclusive)
    {
        return Random.Range(minInclusive, maxInclusive + 1);
    }

    // actualiza la lista de enemigos cuando muere un enemigo
    public void OnEnemyDies(EnemyLifeComponent enemy)
    {
        if (_listOfEnemies.Contains(enemy))
        {
            _listOfEnemies.Remove(enemy);
        }
    }

    // actualiza la lista de enemigos cuando aparece un nuevo enemigo
    public void RegisterEnemy(EnemyLifeComponent enemy)
    {
        if (!_listOfEnemies.Contains(enemy))
        {
            _listOfEnemies.Add(enemy);
        }
    }

    // se llama cuando el personaje sufre da�o
    public void OnPlayerDamage(int lifePoints)
    {
        if (lifePoints <= 0)
        {
            _muriendo = _playerDeathAnimation.DeathAni(); // animaci�n de la muerte
            _playerInputManager.enabled = false;
        }
    }

    // se llama cuando el jugador pierde
    // se reinicia el nivel
    private void OnPlayerDefeat()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);
    }

    // actualiza en el HUD el color de la espada
    public void OnPlayerChangeColor(string color)
    {
        _myUIManager.UpdateCurrentColor(color);
    }

    // actualiza el tiempo de espera del ataque giratorio
    public void OnSpinCooldown(float time)
    {
        _myUIManager.UpdateSpinCooldown((int)time);
    }

    // actualiza el tiempo de espera del rayo de luz
    public void OnRayCooldown(float time)
    {
        _myUIManager.UpdateRayCooldown((int)time);
    }

    // men� de pausa
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
        _directionArrow.enabled = true; // se puede mover la flecha de dir
        _myUIManager.SetPauseMenu(false);   // desaparece el men� de pausa
        Cursor.visible = false; // desaparece el cursor
        Time.timeScale = 1f;    // el tiempo se reanuda
        _gameIsPaused = false;
    }

    private void Pause()
    {
        _directionArrow.enabled = false;    // no se puede mover la flecha de dir
        _myUIManager.SetPauseMenu(true);    // aparece el men� de pausa
        Cursor.visible = true;  // aparece el cursor
        Time.timeScale = 0f;    // el tiempo se para
        _gameIsPaused = true;
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
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
        Time.timeScale = 1f;
        _gameIsPaused = false;
        Cursor.visible = false;
        _player = GameObject.Find("Player");
        _playerInputManager = _player.GetComponent<PlayerInputManager>();
        _playerMovementController = _player.GetComponent<PlayerMovementController>();
        _playerDeathAnimation = _player.GetComponent<DeathAnimation>();
        _directionArrow = GameObject.Find("DirectionArrow").GetComponent<DirectionArrow>();
        _muriendo = false;
        _elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_muriendo)
        {
            _elapsedTime += Time.deltaTime;
            _playerMovementController.SetMovementDirection(Vector3.zero);
            if (_elapsedTime > _tiempoMuerte)
            {
                OnPlayerDefeat();
            }
        }

        // actualiza el HUD con los enemigos restantes
        _myUIManager.UpdateEnemiesLeft(_listOfEnemies.Count);
    }
}
