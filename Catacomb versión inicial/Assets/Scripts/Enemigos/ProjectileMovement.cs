using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _projectileSpeed = 5.0f;
    
    #endregion

    #region references
    private GameObject target;
    private Transform _myTransform;
    private Rigidbody2D hitbox;
    private EnemyShooter _myEnemyShooter;
    #endregion

    #region methods
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerLifeComponent _playerLifeComponent = collision.gameObject.GetComponent<PlayerLifeComponent>();
        if (_playerLifeComponent != null)
        {
            _playerLifeComponent.Damage(_myEnemyShooter.dañoAtaque());
        }
        Destroy(this.gameObject);
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        _myEnemyShooter = GetComponentInParent<EnemyShooter>();
        if (target != null)
        {
            _myTransform = transform;
            Vector3 temp = (target.transform.position - _myTransform.position).normalized;
            _myTransform.up = temp;
            hitbox = GetComponent<Rigidbody2D>();
            hitbox.velocity = (_myTransform.up * _projectileSpeed);
        }
        
    }
}
