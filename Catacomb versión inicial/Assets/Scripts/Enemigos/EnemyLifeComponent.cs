using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifeComponent : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _maxLife;
    #endregion

    #region properties
    private int _currentLife;
    #endregion

    #region references
    #endregion

    #region methods
    public void Damage()
    {
        _currentLife--;
        Debug.Log(_currentLife);
        if (_currentLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject.Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<DamageZone>())
        {
            Damage();
        }
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
