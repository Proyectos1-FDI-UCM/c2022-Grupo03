using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeComponent : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _maxLife = 5;
    [SerializeField]
    private float _deathTime;
    #endregion

    #region properties
    private int _currentLife;
    private float _elapsedTime;
    #endregion

    #region references
    private DeathAnimation _deathAnimation;
    [SerializeField]
    private HpBarScript _healthBar;

    #endregion

    #region methods
    public void Damage(int damage)
    {
        if (_currentLife > 0)
        {
            _currentLife -= damage;
            Debug.Log(_currentLife);
            _deathAnimation.DamageAni(); // animaci�n cuando recibe da�o
            _healthBar.SetHealth(_currentLife);
            if (_currentLife <= 0)
            {
                _deathAnimation.DeathAni();
                GameManager.Instance._currentState = GameState.gameOver;
            }
        }
    }

    public bool Heal()
    {
        if (_currentLife < _maxLife)
        {
            _currentLife++;
            _healthBar.SetHealth(_currentLife);
            return true;
        }
        return false;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _currentLife = _maxLife;
        _deathAnimation = GetComponent<DeathAnimation>();
        _healthBar.SetMaxHealth(_maxLife);
        _healthBar.SetHealth(_currentLife);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance._currentState == GameState.gameOver)
        {
            _elapsedTime += Time.deltaTime;
            Debug.Log(_elapsedTime);
            if (_elapsedTime > _deathTime)
            {
                GameManager.Instance.OnPlayerDefeat();
            }
        }
    }
}
