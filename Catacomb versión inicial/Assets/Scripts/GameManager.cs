using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { inGame, pause, gameOver}
public class GameManager : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties
    private int _savedLife;
    public int SavedLife { get => _savedLife; }
    private int _currentLevel;
    private GameState _currentState;
    public GameState CurrentState { get => _currentState; set => _currentState = value; }
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

    // paletas de colores
    private int _numActiveCols;
    public int NumActiveCols { get => _numActiveCols; }
    [SerializeField]
    private Color[] _colors = new Color[5];
    public Color[] Colors { get => _colors; }
    [SerializeField]
    private Color[] _lightColors = new Color[5];
    public Color[] LightColors { get => _lightColors; }
    [SerializeField]
    private Color[] _translucentColors = new Color[5];
    public Color[] TranslucentColors { get => _translucentColors; }

    // gestión de oleadas
    private int currentWave = 0;
    private float waveDuration = 15;
    private float timePassed = 0;
    private int nEnemies = 0;
    #endregion

    #region references
    private static UI_Manager _myUIManager;
    private static PlayerLifeComponent _myPlayerLifeComponent;
    #endregion

    #region methods
    // flujo de juego
    private void TransitionLevel(int numLevel)
    {
        _numActiveCols++;
        _currentState = GameState.inGame;
        _savedLife = _myPlayerLifeComponent.CurrentLife + 1;
        SceneManager.LoadScene(numLevel, LoadSceneMode.Single);
        if (numLevel == 0)
        {
            Destroy(gameObject);
        }
    }

    // generador de numeros aleatorios
    public int NumRandom(int minInclusive, int maxInclusive)
    {
        return Random.Range(minInclusive, maxInclusive + 1);
    }

    // gestión de los enemigos
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

    // se llama cuando el jugador pierde
    // se reinicia el nivel
    public void OnPlayerDefeat()
    {
        _currentState = GameState.inGame;
        _savedLife = _myPlayerLifeComponent.MaxLife;
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex, LoadSceneMode.Single);
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

    // menu de pausa
    public void PauseMenu()
    {
        if (_currentState == GameState.pause)
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
        _myUIManager.SetPauseMenu(false);   // desaparece el menu de pausa
        Time.timeScale = 1f;    // el tiempo se reanuda
        _currentState = GameState.inGame;
    }
    private void Pause()
    {
        _myUIManager.SetPauseMenu(true);    // aparece el menu de pausa
        Time.timeScale = 0f;    // el tiempo se para
        _currentState = GameState.pause;
    }
    public void BackToTitle()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    // gestión de oleadas
    public bool outOfTime()
    {
        timePassed += Time.deltaTime;
        if (timePassed > waveDuration)
        {
            currentWave++;
            timePassed = 0;
            return true;
        }
        return false;
    }
    public void EnemySpawned() { nEnemies++; }

    private void Awake()
    {
        _myUIManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _myPlayerLifeComponent = GameObject.Find("Player").GetComponent<PlayerLifeComponent>();
        // la primera vez esta instancia del script es null
        // las siguientes veces ya no es null
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _savedLife = _myPlayerLifeComponent.MaxLife;
        }
        // si ya hay una instancia los objetos de la misma clase se destruyen
        // entonces, se consigue que no haya dos GameManager
        else
        {
            Destroy(gameObject);
        }
        _listOfEnemies = new List<EnemyLifeComponent>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _numActiveCols = 3;
        Time.timeScale = 1f;
        _currentState = GameState.inGame;
        _currentLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // actualiza el HUD con los enemigos restantes
        _myUIManager.UpdateEnemiesLeft(_listOfEnemies.Count);

        // para debug
        if (Input.GetKeyDown(KeyCode.P))
        {
            _currentLevel++;
            if (_currentLevel > 3)
            {
                _currentLevel = 0;
            }
            TransitionLevel(_currentLevel);
        }
    }
}
