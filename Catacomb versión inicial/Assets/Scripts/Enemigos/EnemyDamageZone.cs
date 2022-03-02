using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageZone : MonoBehaviour
{
    #region methods
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // duck typing
        PlayerLifeComponent _playerLifeComponent = collider.GetComponent<PlayerLifeComponent>();
        EnemyDamageZone _enemyDamageZone = collider.GetComponent<EnemyDamageZone>();
        if (_playerLifeComponent != null && _enemyDamageZone == null)
        {
            _playerLifeComponent.Damage();
        }
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
