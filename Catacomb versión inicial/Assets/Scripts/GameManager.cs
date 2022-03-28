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
    static private GameManager _instance;
    // accesor a la instancia del GameManager
    static public GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    // lista de enemigos
    List<EnemyLifeComponent> _listOfEnemies;

    // se utiliza para que se active la animaci�n de morir
    private bool _muriendo;
    // determina si el juego est� pausado o no
    private bool _gameIsPaused;

    // al poner los colors en el GameManager resulta sencillo modificar la paleta de colores de una sola vez
    [SerializeField]
    private Color[] _colors = {
        Color.red,  // rojo
        Color.yellow,   // amarillo
        Color.green,    // verde
        Color.blue,     // azul
        new Color(1,0,0.6452723f)   // rosa
    };
    public Color[] Colors { get => _colors; }
    [SerializeField]
    private Color[] _lightColors = {
        new Color(1, 0.553459f, 0.553459f), // rojo claro
        new Color(1, 0.9764464f, 0.7044024f),   // amarillo claro
        new Color(0.7987421f, 1, 0.7987421f),   // verde claro
        new Color(0.7610062f, 0.7610062f, 1),   // azul claro
        new Color(1, 0.5251572f, 0.8324085f)    // rosa claro
    };
    public Color[] LightColors { get => _lightColors; }
    [SerializeField]
    private Color[] _translucentColors = {
        new Color(1,0,0,1f),  // rojo transl�cido
        new Color(1,0.92f,0.16f,1f),  // amarillo transl�cido
        new Color(0,1,0,1f),  // verde transl�cido
        new Color(0,0,1,1f),  // azul transl�cido
        new Color(1,0,0.6452723f,1f)  // rosa trasnl�cido
    };
    public Color[] TranslucentColors { get => _translucentColors; }

    private int currentWave = 0;
    private float waveDuration = 15;
    private float timePassed = 0;
    private int nEnemies = 0;
    #endregion

    #region references
    private GameObject _player;
    private UI_Manager _myUIManager;
    private PlayerInputManager _playerInputManager;
    private DeathAnimation _playerDeathAnimation;
    private PlayerMovementController _playerMovementController;
    private DirectionArrow _directionArrow;
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

    public void DestroyEnemies()
    {
        foreach (EnemyLifeComponent enemy in _listOfEnemies)
        {
            GameObject.Destroy(enemy.gameObject);
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
    public void OnPlayerChangeColor(Color color)
    {
        _myUIManager.UpdateCurrentColor(color);
    }

    // actualiza el tiempo de espera del ataque giratorio
    public void OnSpinCooldown(float cd, float duration)
    {
        _myUIManager.UpdateSpinCooldown(cd, duration);
    }

    // actualiza el tiempo de espera del rayo de luz
    public void OnRayCooldown(float cd, float duration)
    {
        _myUIManager.UpdateRayCooldown(cd, duration);
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
        _myUIManager.SetPauseMenu(false);   // desaparece el men� de pausa
        Time.timeScale = 1f;    // el tiempo se reanuda
        _directionArrow.enabled = true; // se puede mover la flecha de dir
        _playerInputManager.enabled = true; // el jugador s� puede recibir input
        _gameIsPaused = false;
    }

    private void Pause()
    {
        _myUIManager.SetPauseMenu(true);    // aparece el men� de pausa
        Time.timeScale = 0f;    // el tiempo se para
        _directionArrow.enabled = false;    // no se puede mover la flecha de dir
        _playerInputManager.enabled = false;    // el jugador no puede recibir input
        _gameIsPaused = true;
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void GoToControllerMenu()
    {
        PlayerPrefs.SetString("Back", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public float GetTimePassed() { return timePassed; }

    public bool outOfTime()
    {
        timePassed += Time.deltaTime;
        if (timePassed > waveDuration)
        {
            currentWave++;
            return true;
        }
        return false;
    }

    public void EnemySpawned() { nEnemies++; }
    //public int GetCurrentWave() { return currentWave};

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
    }

    // Update is called once per frame
    void Update()
    {
        if (_muriendo)
        {
            _playerMovementController.SetMovementDirection(Vector3.zero);
            Invoke(nameof(OnPlayerDefeat), _tiempoMuerte);
        }

        // actualiza el HUD con los enemigos restantes
        _myUIManager.UpdateEnemiesLeft(_listOfEnemies.Count);
    }
}
