using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    #region parameters
    // daño que causa el ataque principal y cada una de las habilidades
    [SerializeField]
    private int _mainAttackDmg = 1;
    [SerializeField]
    private int _spinDamage = 1;
    [SerializeField]
    private int _rayDamage = 1;
    [SerializeField]
    private float _dmgZoneDuration;
    // tiempo que duran las zonas de daño en escena
    [SerializeField]
    private float _afterDmgZone;
    // tiempo después del cual se destruyen las zonas de daño
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
    private Quaternion[] _rotations = { Quaternion.Euler(180, 0, 0), Quaternion.Euler(0, 0, 270), Quaternion.identity, Quaternion.Euler(0, 0, 90) };
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
    private delegate void UpdateCooldown(float cd, float duration);
    // los delegados sirven para pasar métodos como argumentos de otro método
    private LayerMask _rayLayerMask;
    // el raycast tiene que ignorar al player y a las damage zones
    private LineRenderer _myLineRenderer;
    private GameObject _bossManagerObject;
    private BossManager _bossManager;
    #endregion

    #region methods
    // ataque principal
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

            Quaternion rotation = _rotations[indice];
            Vector3 offset = _offsets[indice];

            Vector3 instPoint = _myTransform.position + offset;
            StartCoroutine(DmgZonesMainAttack(instPoint, rotation));

            _attackRunning = true;
            _myPlayerInputManager.enabled = false;

            _myAttackAnimation.AttackAni(indice);
        }
    }
    private IEnumerator DmgZonesMainAttack(Vector3 position, Quaternion rotation)
    {
        yield return new WaitForSeconds(_afterDmgZone);

        _lastAttack = Instantiate(_damageZones[0], position, rotation);
        _lastAttack.GetComponent<DamageZone>().SetDamage(_mainAttackDmg);
    }

    // ataque giratorio
    public void SpintAttack()
    {
        if (!_attackRunning && !_spinMade)
        {
            Invoke(nameof(DmgZonesSpinAttack), _afterDmgZone);
            _attackRunning = true;
            _spinMade = true;
            _myPlayerInputManager.enabled = false;
        }
    }
    private void DmgZonesSpinAttack()
    {
        for (int i = 0; i < _attacks.Length; i++)
        {

            _attacks[i] = Instantiate(_damageZones[i % 2], _myTransform.position + _offsets[i], _rotations[i]);
            _attacks[i].GetComponent<DamageZone>().SetDamage(_spinDamage);
        }
    }

    // destruir las zonas de daño, tanto las del ataque principal como las del ataque giratorio
    private void DestroyDmgZone()
    {
        GameObject.Destroy(_lastAttack);
        for (int i = 0; i < _attacks.Length; i++)
        {
            GameObject.Destroy(_attacks[i]);
        }
        _myPlayerInputManager.enabled = true;
    }

    // rayo de luz
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
        RaycastHit2D[] hitInfos = Physics2D.RaycastAll(_myTransform.position, _dir.normalized, _rayLength, _rayLayerMask);
        int indice = _myPlayerChangeColors.GetCurrentColorIndex();

        // dibujo del raycast en el juego
        _myLineRenderer.positionCount = 2;
        _myLineRenderer.SetPosition(0, _myTransform.position);
        _myLineRenderer.SetPosition(1, _myTransform.position + _dir.normalized * _rayLength);
        _myLineRenderer.startColor = GameManager.Instance.LightColors[indice];
        _myLineRenderer.endColor = GameManager.Instance.Colors[indice];

        // el rayo de luz de luz se frena cuando choca con un obstáculo
        bool enemigoChocado = true;
        int i = 0;
        while (i < hitInfos.Length && enemigoChocado)
        {
            // si choca con un enemigo, el rayo continúa
            EnemyLifeComponent enemyLifeComponent = hitInfos[i].collider.GetComponent<EnemyLifeComponent>();
            if (enemyLifeComponent != null)
            {
                // si el color del enemigo es igual que el del rayo, el enemigo sufre daño
                if (hitInfos[i].collider.GetComponent(_enemyColors[indice]) != null)
                {
                    // solo se puede dañar al jefe si se encuentra en el segundo estado
                    if (hitInfos[i].collider.name == "SpiderHead" && _bossManager.State == 2)
                    {
                        hitInfos[i].collider.GetComponent<EnemyLifeComponent>().Damage(_rayDamage);
                    }
                    // dañar al resto de cosas
                    else
                    {
                        hitInfos[i].collider.GetComponent<EnemyLifeComponent>().Damage(_rayDamage);
                    }
                }
            }
            // si choca con un obstáculo, el rayo se frena
            else
            {
                enemigoChocado = false;
                _myLineRenderer.SetPosition(1, hitInfos[i].point);
            }
            i++;
        }

        // tras cierto tiempo, el rayo desaparece
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
        _rayLayerMask = ~(LayerMask.GetMask("Player") | LayerMask.GetMask("DmgZones"));
        _myLineRenderer = GetComponent<LineRenderer>();
        _bossManagerObject = GameObject.Find("BossManager");
        if (_bossManagerObject != null)
        {
            _bossManager = _bossManagerObject.GetComponent<BossManager>();
        }
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
            Invoke(nameof(DestroyDmgZone), _dmgZoneDuration + _afterDmgZone);
            // las zonas de daño se destruyen después de que se hayan creado
            // y haya pasado un tiempo determinado
        }

        // tiempo de espera hasta que el rayo se lanza
        if (_rayWaiting)
        {
            _myPlayerMovementController.SetMovementDirection(Vector3.zero);
            _rayWaiting = false;
            Invoke(nameof(LightRay), _rayPreparationTime);
        }

        // tiempos de espera de las habilidades
        Cooldown(_rayCooldown, ref _rayMade, ref _elapsedTimeRay, GameManager.Instance.OnRayCooldown);
        // el tiempo de espera del giro tiene que ser superior a la duración del ataque
        Cooldown(_spinCooldown, ref _spinMade, ref _elapsedTimeSpin, GameManager.Instance.OnSpinCooldown);
    }
}
