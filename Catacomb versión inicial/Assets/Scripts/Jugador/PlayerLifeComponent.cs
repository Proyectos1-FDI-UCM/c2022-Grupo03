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
    [SerializeField]
    private HpBarScript hpbar;
    #endregion

    #region methods
    public void Damage(int damage)
    {
        if (_currentLife > 0)
        {
            _currentLife -= damage;
        }
        _deathAnimation.DamageAni(); //animación cuando recibe daño
        GameManager.Instance.OnPlayerDamage(_currentLife);
        hpbar.SetHealth(_currentLife);
    }

    public bool Heal()
    {
        Debug.Log("Curado!");
        if (_currentLife < _maxLife)
        {
            _currentLife++;
            GameManager.Instance.OnPlayerDamage(_currentLife);
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
        GameManager.Instance.OnPlayerDamage(_currentLife);
        _deathAnimation = GetComponent<DeathAnimation>();
        hpbar.SetMaxHealth(_maxLife);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
