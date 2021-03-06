using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { inGame, pause, gameOver}
public class GameManager : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _timeChangeLevel;
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

    // flujo del juego
    private GameState _currentState;
    public GameState CurrentState { get => _currentState; set => _currentState = value; }
    private int _currentLevel;
    private int _savedLife;
    public int SavedLife { get => _savedLife; }
    private float _timeAppearLvMessage = 1f;
    private float _timeDisappearLvMessage = 3f;

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
    private int nEnemies = -1;
    private bool nivelTerminado;
    //variable para que no se llame all método varias veces en el update
    private bool _delay;
    private int numW = 0;
    private bool state;
    [SerializeField]
    private bool debug = false;
    public bool State { get => state; set => state = value; }
    #endregion

    #region references
    private static UI_Manager _myUIManager;
    private static PlayerLifeComponent _myPlayerLifeComponent;
    private List<GameObject> spawners;
    private AudioSource _myAudioSource;
    #endregion

    #region methods
    // generador de numeros aleatorios
    public int NumRandom(int minInclusive, int maxInclusive)
    {
        return Random.Range(minInclusive, maxInclusive + 1);
    }

    #region enemigos
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
            if (enemy != null)
            {
                GameObject.Destroy(enemy.gameObject);
            }
        }
    }
    #endregion

    #region flujo
    private IEnumerator TransitionLevel(int numLevel, string lvText, float time)
    {
        StartCoroutine(LevelInfo(lvText, 0f, _timeChangeLevel));

        yield return new WaitForSeconds(time);
        _currentState = GameState.inGame;
        // aumenta la vida del jugador
        _numActiveCols++;
        if (_savedLife < _myPlayerLifeComponent.MaxLife)
        {
            _savedLife = _myPlayerLifeComponent.CurrentLife + 1;
        }

        // se carga el siguiente nivel
        SceneManager.LoadScene(numLevel, LoadSceneMode.Single);
        if (numLevel == 0)
        {
            Destroy(gameObject);
        }
        // se muestra en pantalla el número del siguiente nivel
        StartCoroutine(LevelInfo("Nivel " + _currentLevel, _timeAppearLvMessage, _timeDisappearLvMessage));
    }

    public void NextLevel()
    {
        // mensaje que se muestra en pantalla al terminar un nivel
        string lvText;

        // después de terminar un menú seleccionado se vuelve a la pantalla de título
        if (PlayerPrefs.GetString("Back") == "SelecNivel")
        {
            lvText = "Nivel terminado";
            _currentLevel = 0;
        }
        else if (PlayerPrefs.GetString("Back") == "Prueba")
        {
            lvText = "Prueba terminado";
            _currentLevel = 0;
        }
        // aumenta el número de nivel
        else
        {
            if (_currentLevel == 3)
            {
                lvText = "¡¡VICTORIA!!";
                _currentLevel = 0;
            }
            else
            {
                lvText = "Nivel terminado\nvida restaurada";
                _currentLevel++;
            }
        }

        StartCoroutine(TransitionLevel(_currentLevel, lvText, _timeChangeLevel));
    }

    // se llama cuando el jugador pierde
    // se reinicia el nivel
    public void OnPlayerDefeat()
    {    
        _savedLife = _myPlayerLifeComponent.MaxLife;
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex, LoadSceneMode.Single);
        StartCoroutine(LevelInfo(LvIndexToName(_currentLevel), _timeAppearLvMessage, _timeDisappearLvMessage));
        numW = 0;
        spawners.Clear();
        currentWave = -1;
        nEnemies = -1;
        _delay = false;
        timePassed = waveDuration * 0.995f;
        _currentState = GameState.inGame;
    }

    private string LvIndexToName(int lvNumber)
    {
        string lvName = "";
        switch (lvNumber)
        {
            case 1:
            case 2:
            case 3:
                lvName = "Nivel " + lvNumber;
                break;
            case 4:
                lvName = "Prueba enemigos cuerpo a cuerpo";
                break;
            case 5:
                lvName = "Prueba enemigos a distancia";
                break;
            case 6:
                lvName = "Prueba enemigos kamikazes";
                break;
            case 7:
                lvName = "Prueba enemigos tanques";
                break;
        }
        return lvName;
    }
    #endregion

    #region HUD
    public IEnumerator LevelInfo(string lvText, float timeMessageAppear, float timeMessageDisappear)
    {
        yield return new WaitForSeconds(timeMessageAppear);
        _myUIManager.LevelMessage(lvText);    // cambia el mensaje
        _myUIManager.SetLvMessage(true);    // aparece el mensaje
        Invoke(nameof(DeactiveLvMessage), timeMessageDisappear);  // se desactiva el mensaje
    }
    public int GetWaveTime() { return (int)(waveDuration + 1 - timePassed); }
    private void DeactiveLvMessage()
    {
        _myUIManager.SetLvMessage(false);
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
    #endregion

    #region menuPausa
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
        _myAudioSource.mute = false;
        _myUIManager.SetPauseMenu(false);   // desaparece el menu de pausa
        _myUIManager.SetRayCooldown(true);
        _myUIManager.SetSpinCooldown(true);
        _myUIManager.SetCurrentColor(true);
        _myUIManager.SetEnemiesLeft(true);
        _myUIManager.SetPlayerLifeBar(true);
        _myUIManager.SetTimer(true);
        _myUIManager.SetBossBar(true);
        Time.timeScale = 1f;    // el tiempo se reanuda
        _currentState = GameState.inGame;
    }
    private void Pause()
    {
        _myAudioSource.mute = true;
        _myUIManager.SetPauseMenu(true);    // aparece el menu de pausa
        _myUIManager.SetLvMessage(false);
        _myUIManager.SetRayCooldown(false);
        _myUIManager.SetSpinCooldown(false);
        _myUIManager.SetCurrentColor(false);
        _myUIManager.SetEnemiesLeft(false);
        _myUIManager.SetPlayerLifeBar(false);
        _myUIManager.SetTimer(false);
        _myUIManager.SetBossBar(false);
        Time.timeScale = 0f;    // el tiempo se para
        _currentState = GameState.pause;
    }
    public void BackToTitle()
    {
        GameObject.Destroy(gameObject);
        SceneManager.LoadScene(0, LoadSceneMode.Single);

        Time.timeScale = 1f;
        _currentState = GameState.inGame;
        numW = 0;
        spawners.Clear();
        currentWave = -1;
        nEnemies = -1;
        timePassed = 10;
        _currentLevel = 0;
    }
    #endregion

    #region oleadas
    // controla el tiempo de cada oleada
    public bool WaveTimer()
    {
        timePassed += Time.deltaTime;
        if (timePassed > waveDuration)
        {
            timePassed = 0;
            return true;
        }
        return false;
    }

    public void AddSpawner(GameObject spawner)
    {
        if(spawners.Count <= 4)
            spawners.Add(spawner);
    }

    // controla el número de enemigos que hay en el nivel, se llama cada vez que se instancia un enemigo
    public void EnemySpawned() { nEnemies++; _delay = false; }
    public void EnemyDestroyed() { nEnemies--; }
    public int GetCurrentWave() { return currentWave; }
    public int GetNumEnemies() { return nEnemies; }
    public void InitEnemyNumber() { nEnemies = 0; }
    public bool GetWaveState() { return state; }

    private void ActivateSpawners()
    {
        foreach (GameObject g in spawners)
        {           
            if (!g.GetComponent<WaveManager>().Spawn())
            {
                numW++;
            }
        }
    }
    #endregion

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
        spawners = new List<GameObject>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myAudioSource = GetComponent<AudioSource>();

        Time.timeScale = 1f;
        _currentState = GameState.inGame;
        _currentLevel = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LevelInfo(LvIndexToName(_currentLevel), 0f, _timeDisappearLvMessage));
        _numActiveCols = 3 + _currentLevel - 1;
        if (_numActiveCols > 5)
        {
            _numActiveCols = 5;
        }

        _delay = false;
        nivelTerminado = false;
        state = false;
        ActivateSpawners();
    }

    // Update is called once per frame
    void Update()
    {
        if(debug)
        {
            waveDuration = 999;
        }
        // actualiza el HUD con los enemigos restantes
        //_myUIManager.UpdateEnemiesLeft(_listOfEnemies.Count);
        _myUIManager.UpdateEnemiesLeft(nEnemies);

        if ((!_delay) && (nEnemies == 0 || WaveTimer()))
        {
            _delay = true;
            currentWave++;
            _myUIManager.UpdateCurrentWave(currentWave + 1);
            if (debug) currentWave = 0;
            ActivateSpawners();
            timePassed = 0;
        }
        else if (numW >= spawners.Count)
        {
            _myUIManager.EnabledTimer(false);
        }

        if ((numW >= spawners.Count) && nEnemies == 0) nivelTerminado = true;

        if (_currentLevel != 3 && nivelTerminado)
        {
            currentWave = -1;
            numW = 0;
            nEnemies = -1;

            spawners.Clear();
            
            NextLevel();
            
            currentWave = -1;
            numW = 0;
            nEnemies = -1;

            spawners.Clear();

            _myUIManager.EnabledTimer(true);

            _delay = false;

            nivelTerminado = false;
        }
    }
}
