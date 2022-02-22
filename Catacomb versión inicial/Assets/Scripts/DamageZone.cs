using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties
    private float _elapsedTime;
    #endregion

    #region references

    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // duck typing
        EnemyLifeComponent _enemyLifeComponent = collider.GetComponent<EnemyLifeComponent>();
        if (_enemyLifeComponent != null)
        {
            _enemyLifeComponent.Damage();
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
