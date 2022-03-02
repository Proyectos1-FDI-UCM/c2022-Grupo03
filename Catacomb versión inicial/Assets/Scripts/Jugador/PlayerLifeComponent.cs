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

    #endregion

    #region methods
    public void Damage()
    {
        _currentLife -= _hitDamage;
        if (_currentLife <= 0)
        {
            GameManager.Instance.OnPlayerDies();
        }
        Debug.Log(_currentLife);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _currentLife = _maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
