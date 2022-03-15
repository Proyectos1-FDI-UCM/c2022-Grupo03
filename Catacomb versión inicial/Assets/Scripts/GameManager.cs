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
    // generador de números aleatorios
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

    // se llama cuando el personaje sufre daño
    public void OnPlayerDamage(int lifePoints)
    {
        if (lifePoints <= 0)
        {
            _muriendo = _playerDeathAnimation.DeathAni(); // animación de la muerte
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
    public void OnSpinCooldown(float cd, float duration)
    {
        _myUIManager.UpdateSpinCooldown(cd, duration);
    }

    // actualiza el tiempo de espera del rayo de luz
    public void OnRayCooldown(float time)
    {
        _myUIManager.UpdateRayCooldown((int)time);
    }

    // menú de pausa
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
        _myUIManager.SetPauseMenu(false);   // desaparece el menú de pausa
        Time.timeScale = 1f;    // el tiempo se reanuda
        _directionArrow.enabled = true; // se puede mover la flecha de dir
        _playerInputManager.enabled = true; // el jugador sí puede recibir input
        _gameIsPaused = false;
    }

    private void Pause()
    {
        _myUIManager.SetPauseMenu(true);    // aparece el menú de pausa
        Time.timeScale = 0f;    // el tiempo se para
        _directionArrow.enabled = false;    // no se puede mover la flecha de dir
        _playerInputManager.enabled = false;    // el jugador no puede recibir input
        _gameIsPaused = true;
    }

    public void BackToTitle()
    {
        PlayerPrefs.DeleteKey("Back");
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void GoToControllerMenu()
    {
        PlayerPrefs.SetString("Back", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
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
        _player = GameObject.Find("Player");
        _playerInputManager = _player.GetComponent<PlayerInputManager>();
        _playerMovementController = _player.GetComponent<PlayerMovementController>();
        _playerDeathAnimation = _player.GetComponent<DeathAnimation>();
        _directionArrow = GameObject.Find("DirectionArrow").GetComponent<DirectionArrow>();
        _muriendo = false;
        _elapsedTime = 0;

        if (PlayerPrefs.GetString("Back") == "Escena Pedro")
        {
            PauseMenu();
            _myUIManager.SetMenu(false);
            _myUIManager.SetOptionsMenu(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_muriendo)
        {
            _playerMovementController.SetMovementDirection(Vector3.zero);
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _tiempoMuerte)
            {
                OnPlayerDefeat();
            }
        }

        // actualiza el HUD con los enemigos restantes
        _myUIManager.UpdateEnemiesLeft(_listOfEnemies.Count);
    }
}
