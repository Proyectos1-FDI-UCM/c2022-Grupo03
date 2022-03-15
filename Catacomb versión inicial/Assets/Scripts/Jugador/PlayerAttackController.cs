using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _attackDuration;
    [SerializeField]
    private float _rayLength;
    [SerializeField]
    private float _rayCooldown;
    [SerializeField]
    private float _rayPreparationTime;
    [SerializeField]
    private float _spinCooldown;
    [SerializeField]
    private float _durationRay = 0.5f;
    [SerializeField]
    private Vector3[] _offsets = { Vector3.up, -Vector3.right, -Vector3.up, Vector3.right };
    #endregion

    #region properties
    // colores
    private string[] _enemyColors = { "Red", "Yellow", "Green", "Blue", "Pink" };
    private Color[] _colors = { Color.red, Color.yellow, Color.green, Color.blue, Color.magenta };
    // para que se puedan almacenar en el array tienen que ser estáticos
    private static Color _lightRed = new Color(1, 0.553459f, 0.553459f);
    private static Color _lightYellow = new Color(1, 0.9764464f, 0.7044024f);
    private static Color _lightGreen = new Color(0.7987421f, 1, 0.7987421f);
    private static Color _lightBlue = new Color(0.7610062f, 0.7610062f, 1);
    private static Color _lightMagenta = new Color(1, 0.8459119f, 1);
    private Color[] _lightCols = { _lightRed, _lightYellow, _lightGreen, _lightBlue, _lightBlue };

    private Quaternion[] _rotations = { Quaternion.identity, Quaternion.Euler(0, 0, 90) };
    GameObject _lastAttack;
    GameObject[] _attacks = new GameObject[4];
    private bool _attackRunning; // indicar si se ha efectuado el ataque o no
    private bool _rayMade;
    private bool _spinMade;
    private bool _rayWaiting;
    private float _elapsedTimeSpin;
    private float _elapsedTimeRay;
    private Vector3 _dir;
    #endregion

    #region references
    private Transform _myTransform;
    [SerializeField]
    private GameObject[] _damageZones;
    [SerializeField]
    private GameObject _dirArrow;
    private Transform _dirArrowTransform;
    private PlayerInputManager _myPlayerInputManager;
    private PlayerMovementController _myPlayerMovementController;
    private PlayerChangeColors _myPlayerChangeColors;
    private AttackAnimation _myAttackAnimation;
    private LayerMask _myLayerMask;
    private delegate void UpdateCooldown(float cd, float duration);
    // los delegados sirven para pasar métodos como argumentos de otro método
    private LineRenderer _myLineRenderer;
    #endregion

    #region methods
    public void MainAttack()
    {
        Vector3 angle = _dirArrowTransform.rotation.eulerAngles;

        if (!_attackRunning)
        {
            if (angle.y == 180)
            {
                angle.z = angle.y;
            }
            // ajustar los ángulos menores que 45º y mayores o iguales que 0º para que su índice sea 3
            else if (angle.z < 45 && angle.z >= 0)
            {
                angle.z = angle.z + 360;
            }

            int indice = (int)(angle.z - 45) / 90;

            Quaternion rotation = _rotations[indice % 2];
            Vector3 offset = _offsets[indice];

            Vector3 instPoint = _myTransform.position + offset;
            _lastAttack = Instantiate(_damageZones[0], instPoint, rotation);

            _attackRunning = true;
            _myPlayerInputManager.enabled = false;

            _myAttackAnimation.AttackAni(indice);
        }
    }

    public void SpintAttack()
    {
        if (!_attackRunning && !_spinMade)
        {
            for (int i = 0; i < _attacks.Length; i++)
            {
                _attacks[i] = Instantiate(_damageZones[i % 2], _myTransform.position + _offsets[i], Quaternion.identity);
            }
            _attackRunning = true;
            _spinMade = true;
            _myPlayerInputManager.enabled = false;
        }
    }

    public void Shoot()
    {
        if (!_rayMade)
        {
            _rayWaiting = true;
            _dir = _dirArrowTransform.right;
            _myPlayerInputManager.enabled = false;
        }
    }

    private void LightRay()
    {
        _myPlayerInputManager.enabled = true;

        // debug del raycast
        Debug.DrawLine(_myTransform.position, _myTransform.position + _dir.normalized * _rayLength, Color.red, 2f);
        // hacer que el raycast golpee a todos los enemigos que encuentra a su paso
        RaycastHit2D[] hitInfos;
        hitInfos = Physics2D.RaycastAll(_myTransform.position, _dir.normalized, _rayLength, _myLayerMask);
        int indice = _myPlayerChangeColors.GetCurrentColorIndex();

        // dibujo del raycast en el juego
        _myLineRenderer.positionCount = 2;
        _myLineRenderer.SetPosition(0, _myTransform.position);
        _myLineRenderer.SetPosition(1, _myTransform.position + _dir.normalized * _rayLength);
        _myLineRenderer.startColor = _lightCols[indice];
        _myLineRenderer.endColor = _colors[indice];

        foreach (RaycastHit2D hit in hitInfos)
        {
            if (hit.collider.GetComponent(_enemyColors[indice]) != null)
            {
                Debug.Log("dentro del if");
                hit.collider.GetComponent<EnemyLifeComponent>().Damage();
            }
        }

        Invoke(nameof(DisappearRay), _durationRay);

        _rayMade = true;
    }

    private void DisappearRay()
    {
        _myLineRenderer.positionCount = 0;
    }

    private void Cooldown(float duration, ref bool abilityMade, ref float elapsedTime, UpdateCooldown upCd)
    {
        if (abilityMade)
        {
            elapsedTime += Time.deltaTime;
            upCd(duration - elapsedTime, duration);
            if (elapsedTime > duration)
            {
                abilityMade = false;
                elapsedTime = 0;
            }
        }
    }

    private void DestroyDmgZone()
    {
        GameObject.Destroy(_lastAttack);
        for (int i = 0; i < _attacks.Length; i++)
        {
            GameObject.Destroy(_attacks[i]);
        }
        _myPlayerInputManager.enabled = true;
    }

    private void Awake()
    {

    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _dirArrowTransform = _dirArrow.transform;
        _attackRunning = false;
        _rayMade = false;
        _spinMade = false;
        _rayWaiting = false;
        _myPlayerInputManager = GetComponent<PlayerInputManager>();
        _myPlayerMovementController = GetComponent<PlayerMovementController>();
        _myPlayerChangeColors = GetComponent<PlayerChangeColors>();
        _myAttackAnimation = GetComponent<AttackAnimation>();
        _myLayerMask = LayerMask.GetMask("Enemy");
        _myLineRenderer = GetComponent<LineRenderer>();

        // se pueden eliminar estas dos líneas
        // GameManager.Instance.OnRayCooldown(0);
        // GameManager.Instance.OnSpinCooldown(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // duración del ataque principal y giratorio
        // sincronizar el tiempo que dura el ataque con el tiempo que el jugador no puede moverse
        if (_attackRunning)
        {
            // aunque se desactive el script de input, hay que hacer que deje de moverse
            // porque si se estaba moviendo antes de desactivarlo se seguirá moviendo después
            _myPlayerMovementController.SetMovementDirection(Vector3.zero);
            _attackRunning = false;
            Invoke(nameof(DestroyDmgZone), _attackDuration);
        }

        // tiempo de espera hasta que el rayo se lanza
        if (_rayWaiting)
        {
            _myPlayerMovementController.SetMovementDirection(Vector3.zero);
            _rayWaiting = false;
            Invoke(nameof(LightRay), _rayPreparationTime);
        }

        // tiempos de espera de las habilidades
        // Cooldown(_rayCooldown, ref _rayMade, ref _elapsedTimeRay, GameManager.Instance.OnRayCooldown);
        // el tiempo de espera del giro tiene que ser superior a la duración del ataque
        Cooldown(_spinCooldown, ref _spinMade, ref _elapsedTimeSpin, GameManager.Instance.OnSpinCooldown);
    }
}
