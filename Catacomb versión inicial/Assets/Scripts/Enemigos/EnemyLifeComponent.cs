using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeComponent : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _maxLife;
    [SerializeField]
    private float _desiredTimeToRecovery=5;  //determina cada cuanto tiempo recuperan vida los enemigo azules
    #endregion

    #region properties
    private int _currentLife;
    private bool _isBlue = false; //si un enemigo es azul se pone a true
    private float _elapsedTime = 0; //contador 
    #endregion

    #region references
    private Pink _myPinkComponent;
    private Blue _myBlueComponent;
    #endregion

    #region methods
    public void Damage()
    {
        _currentLife--;
        //Debug.Log(_currentLife);
        if (_currentLife <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameObject.Destroy(gameObject);
    }

    public void Kamikaze()
    {
        _currentLife = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<DamageZone>())
        {
            int rnd = Random.Range(1, 6); //TIENE UN 20% DE POSIBILIDADES DE SER INMUNE AL ATAQUE
            if(rnd!=1) Damage();

        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
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
        if(_isBlue && _elapsedTime>=_desiredTimeToRecovery && _currentLife<_maxLife) //se ha puesto de forma que no pueda recuperar vida si ya la tiene al maximo
        {
            _currentLife += _myBlueComponent.RecoveryPoints();
            _elapsedTime = 0;
        }
        
    }
}
