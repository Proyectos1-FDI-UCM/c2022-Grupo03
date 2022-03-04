using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeComponent : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _maxLife = 5;
    [SerializeField]
    private int _hitDamage = 1; //vida perdida por golpe
    #endregion

    #region properties
    private int _currentLife;
    #endregion

    #region references
    private DeathAnimation _deathAnimation;
    #endregion

    #region methods
    public void Damage(int damage)
    {
        _currentLife -= damage;
        if (_currentLife <= 0)
        {
            _deathAnimation.DeathAni();
        }
        Debug.Log(_currentLife);
    }
    /*
    public void DamageRed(int damage)
    {
        _currentLife -= damage;
        if (_currentLife <= 0)
        {
            _deathAnimation.DeathAni();
        }
        Debug.Log(_currentLife);
    }
    */
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _currentLife = _maxLife;
        _deathAnimation = GetComponent<DeathAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
