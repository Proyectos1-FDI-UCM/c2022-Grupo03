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
    private Vector3[] _offsets = { Vector3.up, -Vector3.right, -Vector3.up, Vector3.right };
    #endregion

    #region properties
    private string[] _enemyColors = { "Red", "Yellow", "Green", "Blue", "Pink" };
    private Quaternion[] _rotations = { Quaternion.identity, Quaternion.Euler(0, 0, 90) };
    GameObject _lastAttack;
    GameObject[] _attacks = new GameObject[4];
    private bool _attackRunning; // indicar si se ha efectuado el ataque o no
    private bool _rayMade;
    private bool _spinMade;
    private bool _rayWaiting;
    private float _elapsedTime;  // mientras se está realizando/preparando el ataque
    private float _elapsedTimeBis;   // para los cooldowns
    private Vector3 _dir;
    #endregion

    #region references
    Transform _myTransform;
    [SerializeField]
    private GameObject[] _damageZones;
    [SerializeField]
    GameObject _dirArrow;
    Transform _dirArrowTransform;
    PlayerInputManager _myPlayerInputManager;
    PlayerMovementController _myPlayerMovementController;
    PlayerChangeColors _myPlayerChangeColors;
    private AttackAnimation _myAttackAnimation;
    #endregion

    #region methods
    public void MainAttack()
    {
        _myAttackAnimation.AttackAni(); 

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
        Debug.DrawRay(_myTransform.position, _dir.normalized * _rayLength, Color.red, 2f);  // debug del raycast
        RaycastHit2D hitInfo;
        hitInfo = Physics2D.Raycast(_myTransform.position, _dir.normalized, _rayLength);
        if (hitInfo)
        {
            int indice = _myPlayerChangeColors.GetCurrentColorIndex();
            if (hitInfo.collider.GetComponent(_enemyColors[indice]) != null)
            {
                hitInfo.collider.GetComponent<EnemyLifeComponent>().Damage();
            }
        }
        _rayMade = true;
    }

    private void Cooldown(float duration, ref bool abilityMade)
    {
        if (abilityMade)
        {
            _elapsedTimeBis += Time.deltaTime;
            if (_elapsedTimeBis > duration)
            {
                abilityMade = false;
                _elapsedTimeBis = 0;
            }
        }
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
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackRunning)
        {
            // sincronizar el tiempo que dura el ataque con el tiempo que el jugador no puede moverse
            _elapsedTime += Time.deltaTime;
            // aunque se desactive el script de input, hay que hacer que deje de moverse
            // porque si se estaba moviendo antes de desactivarlo se seguirá moviendo después
            _myPlayerMovementController.SetMovementDirection(Vector3.zero);
            if (_elapsedTime > _attackDuration)
            {
                GameObject.Destroy(_lastAttack);
                for (int i = 0; i < _attacks.Length; i++)
                {
                    GameObject.Destroy(_attacks[i]);
                }
                _attackRunning = false;
                _myPlayerInputManager.enabled = true;
                _elapsedTime = 0;
            }
        }

        Cooldown(_rayCooldown, ref _rayMade);
        Cooldown(_spinCooldown, ref _spinMade);

        if (_rayWaiting)
        {
            _myPlayerMovementController.SetMovementDirection(Vector3.zero);
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _rayPreparationTime)
            {
                _rayWaiting = false;
                LightRay();
                _myPlayerInputManager.enabled = true;
                _elapsedTime = 0;
            }
        }
        // el tiempo de espera del giro tiene que ser superior a la duración del ataque
    }
}
