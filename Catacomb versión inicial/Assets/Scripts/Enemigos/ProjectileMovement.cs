using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _projectileSpeed = 5.0f;
    #endregion

    #region properties
    private int _damage;
    private float _playerSpeed;
    #endregion

    #region references
    private GameObject _player;
    private Transform _myTransform;
    private Rigidbody2D _myRigidBody2D;
    private SpiderWebCollision _mySpiderWebCollision;
    #endregion

    #region methods
    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void SetSpeed(float newSpeed)
    {
        _playerSpeed = newSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // si la bala choca con el jugador este sufre daño
        PlayerLifeComponent _playerLifeComponent = collision.gameObject.GetComponent<PlayerLifeComponent>();
        if (_playerLifeComponent != null)
        {
            //Debug.Log("el jugador ha sufrido daño");
            // hay que descomentarlo
            // _playerLifeComponent.Damage(_damage);

            if (_mySpiderWebCollision != null)
            {
                collision.gameObject.GetComponent<PlayerMovementController>().SetSpeed(_playerSpeed);
            }
        }

        _myRigidBody2D.velocity = Vector3.zero;
        Destroy(this.gameObject);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _mySpiderWebCollision = GetComponent<SpiderWebCollision>();

        // jugador
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            Vector3 temp = (_player.transform.position - _myTransform.position).normalized;
            _myTransform.up = temp;
            _myRigidBody2D = GetComponent<Rigidbody2D>();
            _myRigidBody2D.velocity = (_myTransform.up * _projectileSpeed);
        }
    }

    private void Update()
    {

    }
}
