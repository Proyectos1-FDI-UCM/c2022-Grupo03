using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    #region parameters
    // daño que causa el ataque principal y cada una de las habilidades
    [SerializeField]
    private int _mainAttackDmg = 1;
    // tiempo que duran las zonas de daño en escena del ataque principal
    [SerializeField]
    private float _afterDmgZoneMain;
    [SerializeField]
    private int _spinDamage = 1;
    [SerializeField]
    private float _afterDmgZonesSpin;
    [SerializeField]
    private float _spinCooldown;
    [SerializeField]
    private float _dmgZoneDuration;
    // tiempo después del cual se destruyen las zonas de daño
    [SerializeField]
    private int _rayDamage = 1;
    [SerializeField]
    private float _rayLength;
    [SerializeField]
    private float _rayCooldown;
    [SerializeField]
    private float _rayPreparationTime;
    [SerializeField]
    private float _durationRay = 0.5f;
    [SerializeField]
    private Vector3[] _offsets = { Vector3.up, -Vector3.right, -Vector3.up, Vector3.right };
    // animación del ataque giratorio
    private float x_scale, y_scale, z_scale;
    private float time = 0;
    #endregion

    #region properties
    // colores
    private string[] _enemyColors = { "Red", "Yellow", "Green", "Blue", "Pink" };
    private Quaternion[] _rotations = { Quaternion.Euler(180, 0, 0), Quaternion.Euler(0, 0, 270), Quaternion.identity, Quaternion.Euler(0, 0, 90) };
    GameObject _lastAttack;
    GameObject _spinAttack;
    private bool _attackRunning; // indicar si se está atacando
    private bool _rayMade;  // indicar si se ha realizado la habilidad del rayo
    private bool _spinMade; // indicar si se ha realizado el ataque giratorio
    private bool _spinCdOn; // indicar si el rayo se encuentra en cooldown
    private bool _rayWaiting;
    private float _elapsedTimeSpin;
    private float _elapsedTimeRay;
    private Vector3 _dir;
    // animación del rayo
    private bool atacarayo = false;
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
    // los delegados sirven para pasar métodos como argumentos de otro método
    private delegate void UpdateCooldown(float cd, float duration);
    private LayerMask _rayLayerMask;
    // el raycast tiene que ignorar al player y a las damage zones
    private LineRenderer _myLineRenderer;
    private GameObject _bossManagerObject;
    private BossManager _bossManager;
    private Rigidbody2D _myRigidBody2D;
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
        yield return new WaitForSeconds(_afterDmgZoneMain);

        _lastAttack = Instantiate(_damageZones[0], position, rotation);
        _lastAttack.GetComponent<DamageZone>().SetDamage(_mainAttackDmg);
    }

    // ataque giratorio
    public void SpintAttack()
    {
        if (!_attackRunning && !_spinMade && !_spinCdOn)
        {
            Invoke(nameof(SpinZone), _afterDmgZonesSpin);
            _attackRunning = true;
            _spinMade = true;
            // animación del rayo
            _myAttackAnimation.Rotate(_spinMade);
            _myPlayerInputManager.enabled = false;
        }
    }
    private void SpinZone()
    {
        _spinAttack = Instantiate(_damageZones[1], _myTransform.position, Quaternion.identity);
        DamageZone[] spinZones = GetComponentsInChildren<DamageZone>();
        for (int i = 0; i < spinZones.Length; i++)
        {
            spinZones[i].SetDamage(_spinDamage);
        }
        _spinCdOn = true;
        // el cd del ataque giratorio comienza cuando se han instanciado las zonas de daño
    }

    // destruir las zonas de daño
    private void DestroyDmgZones()
    {
        if (_spinMade)
        {
            Invoke(nameof(DestroySpinZone), _dmgZoneDuration + _afterDmgZonesSpin);
        }
        else
        {
            Invoke(nameof(DestroyDmgZoneMainAttack), _dmgZoneDuration + _afterDmgZoneMain);
        }
    }
    // destruir la zona de daño del ataque principal
    private void DestroyDmgZoneMainAttack()
    {
        GameObject.Destroy(_lastAttack);
        _myPlayerInputManager.enabled = true;
    }
    // destruir las zonas de daño del ataque giratorio
    private void DestroySpinZone()
    {
        GameObject.Destroy(_spinAttack);
        _myPlayerInputManager.enabled = true;
        _spinMade = false;  // el ataque ya se ha efectuado
    }

    // rayo de luz
    // dispara el rayo de luz en el caso de que no se encuentre en cooldown
    public void Shoot()
    {
        if (!_rayMade)
        {
            _rayWaiting = true;
            _dir = _dirArrowTransform.right;
            _myPlayerInputManager.enabled = false;
        }
    }
    // crea el rayo de luz y lo dibuja en escena
    private void LightRay()
    {
        _myPlayerInputManager.enabled = true;

        // debug del raycast
        Debug.DrawLine(_myTransform.position, _myTransform.position + _dir.normalized * _rayLength, Color.red, 2f);
        // hacer que el raycast golpee a todos los enemigos que encuentra a su paso
        RaycastHit2D[] hitInfos = Physics2D.RaycastAll(_myTransform.position, _dir.normalized, _rayLength, _rayLayerMask);
        int indice = _myPlayerChangeColors.GetCurrentColorIndex();

        //que el jugador mire hacia donde tiene que mirar cuando se usa el rayo
        Vector3 angle = _dirArrowTransform.rotation.eulerAngles;
        if (angle.y == 180)
        {
            angle.z = angle.y;
        }
        // ajustar los ángulos menores que 45º y mayores o iguales que 0º para que su índice sea 3
        else if (angle.z < 45 && angle.z >= 0)
        {
            angle.z = angle.z + 360;
        }

        int ind = (int)(angle.z - 45) / 180;

        if (ind == 0)
        {
            _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
            _myRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if (ind == 1)
        {
            _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
            _myRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // animación del rayo
        atacarayo = true;
        time = 0;

        // dibujo del raycast en el juego
        _myLineRenderer.positionCount = 2;
        _myLineRenderer.SetPosition(0, _myTransform.position + _dir * 1f);
        _myLineRenderer.SetPosition(1, _myTransform.position + _dir.normalized * _rayLength);
        _myLineRenderer.startColor = GameManager.Instance.LightColors[indice];
        _myLineRenderer.endColor = GameManager.Instance.Colors[indice];

        // el rayo de luz de luz se frena cuando choca con un obstáculo
        bool enemigoChocado = true;
        int i = 0;
        Debug.Log(hitInfos.Length);
        while (i < hitInfos.Length && enemigoChocado)
        {
            Shield enemyTankShield = hitInfos[i].collider.GetComponent<Shield>();
            EnemyLifeComponent enemyLifeComponent = hitInfos[i].collider.GetComponent<EnemyLifeComponent>();
            if (enemyTankShield != null)
            {
                if (enemyTankShield.gameObject.GetComponent(_enemyColors[indice]) != null)
                {
                    enemyTankShield.ChangeShieldCol();
                }
            }
            // si choca con un enemigo, el rayo continúa
            else if (enemyLifeComponent != null)
            {
                if (hitInfos[i].collider.GetComponentInChildren<Shield>() == null)
                {
                    // si el color del enemigo es igual que el del rayo, el enemigo sufre daño
                    // se pone en un if separado para que el rayo pueda atravesar al escudo
                    if (hitInfos[i].collider.GetComponent(_enemyColors[indice]) != null)
                    {
                        // solo se puede dañar al jefe si se encuentra en el segundo estado
                        if (hitInfos[i].collider.name != "SpiderHead" || _bossManager.State == 2)
                        {
                            enemyLifeComponent.Damage(_rayDamage);
                        }
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

    // cooldowns
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

    // animación rayo
    public bool AtacaRayo()
    {
        return atacarayo;
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
        _spinCdOn = false;
        _rayWaiting = false;
        _myPlayerInputManager = GetComponent<PlayerInputManager>();
        _myPlayerMovementController = GetComponent<PlayerMovementController>();
        _myPlayerChangeColors = GetComponent<PlayerChangeColors>();
        _myAttackAnimation = GetComponent<AttackAnimation>();
        _rayLayerMask = ~(LayerMask.GetMask("Player") |
            LayerMask.GetMask("Item") |
            LayerMask.GetMask("DmgZones") |
            LayerMask.GetMask("Obstacle(LetBulletPass)"));
        _myLineRenderer = GetComponent<LineRenderer>();
        _bossManagerObject = GameObject.Find("BossManager");
        if (_bossManagerObject != null)
        {
            _bossManager = _bossManagerObject.GetComponent<BossManager>();
        }
        x_scale = _myTransform.localScale.x;
        y_scale = _myTransform.localScale.y;
        z_scale = _myTransform.localScale.z;
        _myRigidBody2D = GetComponent<Rigidbody2D>();
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
            DestroyDmgZones();
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
        Cooldown(_spinCooldown, ref _spinCdOn, ref _elapsedTimeSpin, GameManager.Instance.OnSpinCooldown);

        // animación del rayo
        time += Time.deltaTime;
        if (time > 0.4)
            atacarayo = false;
    }
}
