using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeComponent : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _maxLife = 5;
    public int MaxLife { get => _maxLife; }

    [SerializeField]
    private float _deathTime = 1;
    #endregion

    #region properties
    private int _currentLife;
    public int CurrentLife { get => _currentLife; set => _currentLife = value; }
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
            _deathAnimation.DamageAni(); // animación cuando recibe daño
            _healthBar.SetHealth(_currentLife);
            if (_currentLife <= 0)
            {
                Debug.Log("CurrentState: " + GameManager.Instance.CurrentState);
                GameManager.Instance.CurrentState = GameState.gameOver;
                StartCoroutine(GameManager.Instance.LevelInfo("¡Has perdido!", 0f, _deathTime));
                _deathAnimation.DeathAni();                   
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
        _currentLife = GameManager.Instance.SavedLife;
        _deathAnimation = GetComponent<DeathAnimation>();
        _healthBar.SetMaxHealth(_maxLife);
        _healthBar.SetHealth(_currentLife);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.gameOver)
        {
            _elapsedTime += Time.deltaTime;
            Debug.Log("time: " + _elapsedTime);
            if (_elapsedTime > _deathTime)
            {
                _elapsedTime = 0;
                GameManager.Instance.OnPlayerDefeat();
            }
        }
    }
}
