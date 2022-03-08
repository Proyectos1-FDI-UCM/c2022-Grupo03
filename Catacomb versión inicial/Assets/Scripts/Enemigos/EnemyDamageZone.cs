using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageZone : MonoBehaviour
{
    #region parameters
    private bool _kamikaze = false;
    #endregion

    #region references
    private EnemyMelee _enemyMeleeComponent;
    private EnemyKamikaze _enemyKamikaze;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // duck typing
        
        PlayerLifeComponent _playerLifeComponent = collider.GetComponent<PlayerLifeComponent>();
        if (_playerLifeComponent != null)
        {
            if(_kamikaze)
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
        
    }
}
