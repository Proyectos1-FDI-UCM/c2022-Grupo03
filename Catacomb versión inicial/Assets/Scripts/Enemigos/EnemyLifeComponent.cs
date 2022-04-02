using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeComponent : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _maxLife;
    public int MaxLife { set => _maxLife = value; }
    [SerializeField]
    private float _desiredTimeToRecovery = 5f;
    // determina cada cuanto tiempo recuperan vida los enemigos azules
    private Vector3 _offset; //distancia desde el transform hasta el cartel que se instancia a los enemigos azules que esquiven un ataque
    #endregion

    #region properties
    private int _currentLife;
    public int CurrentLife { get => _currentLife; }

    private bool _isBlue = false; // determina si un enemigo es azul
    private float _elapsedTime = 0;
    #endregion

    #region references
    private Pink _myPinkComponent;
    private Blue _myBlueComponent;
    private Transform _myTransform;
    private Message _myMessage;
    private Shield _myShield;
    [SerializeField]
    private HpBarScript healthBar = null;

    #endregion

    #region methods
    public void Damage(int hitDamage)
    {
        // los enemigos azules tienen un 20% de probabilidades de ser inmunes a los ataques
        int rndNum = GameManager.Instance.NumRandom(0, 5);
        if (!_isBlue || rndNum != 0)
        {
            _currentLife -= hitDamage;
            if (_myShield == null)
            {
               _myMessage.SetMessage((-1).ToString());
            }
            if (_currentLife <= 0)
            {
                Die();
            }
        }
        else if (_isBlue && rndNum == 0) //es azul y lo ha esquivado
        {
           _myMessage.SetMessage("MISS!!");
        }
        if (_myShield == null)
        {
           _myMessage.ActivateMessage();
        }
        if (healthBar != null) healthBar.SetHealth(_currentLife);
    }

    private void Die()
    {
        GameManager.Instance.OnEnemyDies(this);
        GameObject.Destroy(gameObject);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // registrar al enemigo en la lista de enemigos
        GameManager.Instance.RegisterEnemy(this);

        // inicializar referencias a otros componentes
        _myTransform = transform;
        _myMessage = GetComponentInChildren<Message>();
        _myShield = GetComponent<Shield>();

        // componente rosa
        _myPinkComponent = GetComponent<Pink>();
        if (_myPinkComponent != null)
        {
            _maxLife += _myPinkComponent.IncreasedLife();
        }

        // componente azul
        _myBlueComponent = GetComponent<Blue>();
        _isBlue = _myBlueComponent != null;

        // vida actual del enemigo
        _currentLife = _maxLife;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(_maxLife);
            healthBar.SetHealth(_currentLife);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // los enemigos azules recuperan vida cada cierto tiempo
        _elapsedTime += Time.deltaTime;
        // se ha puesto de forma que no pueda recuperar vida si ya la tiene al máximo
        if (_isBlue && _elapsedTime > _desiredTimeToRecovery && _currentLife < _maxLife)
        {
            _currentLife += _myBlueComponent.RecoveryPoints();
            _elapsedTime = 0;
        }
    }
}
