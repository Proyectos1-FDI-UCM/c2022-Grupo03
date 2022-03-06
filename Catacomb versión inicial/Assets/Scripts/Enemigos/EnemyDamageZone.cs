using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageZone : MonoBehaviour
{
    #region parameters
    #endregion

    #region references
    private EnemyMelee _enemyMeleeComponent;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // duck typing
        
        PlayerLifeComponent _playerLifeComponent = collider.GetComponent<PlayerLifeComponent>();
        if (_playerLifeComponent != null)
        {
            _playerLifeComponent.Damage(_enemyMeleeComponent.dañoAtaque());
        }

    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _enemyMeleeComponent = GetComponentInParent<EnemyMelee>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
