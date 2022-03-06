using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeComponent : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _maxLife = 5;
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
        _deathAnimation.DamageAni(); //animación cuando recibe daño
        if (_currentLife <= 0)
        {
            _deathAnimation.DeathAni(); //animación de la muerte
        }
        Debug.Log(_currentLife);
    }
    public bool Heal()
    {
        Debug.Log("Curado!");
        if (_currentLife < _maxLife)
        {
            _currentLife++;
            return true;
        }
        return false;
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
