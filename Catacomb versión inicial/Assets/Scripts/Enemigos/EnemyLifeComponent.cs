using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeComponent : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _maxLife;
    [SerializeField]
    private float _desiredTimeToRecovery = 5;  
    // determina cada cuanto tiempo recuperan vida los enemigos azules
    #endregion

    #region properties
    private int _currentLife;
    private bool _isBlue = false; // determina si un enemigo es azul
    private float _elapsedTime = 0; 
    #endregion

    #region references
    private Pink _myPinkComponent;
    private Blue _myBlueComponent;
    #endregion

    #region methods
    public void Damage()
    {
        _currentLife--;
        if (_currentLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.OnEnemyDies(this);
        GameObject.Destroy(gameObject);
    }

    // no funciona!!!
    // la colisión esta en el script de dmg zone
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<DamageZone>())
        {
            Debug.Log("ha entrado");
            int rnd = Random.Range(1, 6);
            // TIENE UN 20% DE POSIBILIDADES DE SER INMUNE AL ATAQUE SI ES AZUL
            if (_isBlue && rnd != 1) Damage();
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.RegisterEnemy(this);

        _myPinkComponent = GetComponent<Pink>();
        _myBlueComponent = GetComponent<Blue>();
        if (_myPinkComponent != null)
        {
            _maxLife += _myPinkComponent.IncreasedLife();
        }
        if(_myBlueComponent!= null)
        {
            _isBlue = true;
        }

        _currentLife = _maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        // se ha puesto de forma que no pueda recuperar vida si ya la tiene al máximo
        if (_isBlue && _elapsedTime > _desiredTimeToRecovery && _currentLife < _maxLife)
        {
            _currentLife += _myBlueComponent.RecoveryPoints();
            _elapsedTime = 0;
        }
    }
}
