using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageZone : MonoBehaviour
{
    #region parameters
    private bool _kamikaze = false;
    private float _elapsedTime = 0;
    #endregion

    #region references
    private EnemyMelee _enemyMeleeComponent;
    private EnemyKamikaze _enemyKamikaze;
    #endregion

    #region methods
    private void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log(_enemyKamikaze.DañoAtaque());
        // duck typing       
        PlayerLifeComponent _playerLifeComponent = collider.gameObject.GetComponent<PlayerLifeComponent>();
        if (_playerLifeComponent != null)
        {
            if (_kamikaze)
                _playerLifeComponent.Damage(_enemyKamikaze.DañoAtaque());
            else
                _playerLifeComponent.Damage(_enemyMeleeComponent.DañoAtaque());
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(_enemyKamikaze.DañoAtaque());
        // duck typing       
        PlayerLifeComponent _playerLifeComponent = collider.gameObject.GetComponent<PlayerLifeComponent>();
        if (_playerLifeComponent != null)
        {
            if (_kamikaze)
                _playerLifeComponent.Damage(_enemyKamikaze.DañoAtaque());
            else
                _playerLifeComponent.Damage(_enemyMeleeComponent.DañoAtaque());
        }
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _enemyMeleeComponent = GetComponentInParent<EnemyMelee>();
        _enemyKamikaze = GetComponentInParent<EnemyKamikaze>();
        if (_enemyKamikaze != null)
            _kamikaze = true;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime > 0.3f)
        {
            Destroy(this.gameObject);
            _elapsedTime = 0;
        }
    }
}
