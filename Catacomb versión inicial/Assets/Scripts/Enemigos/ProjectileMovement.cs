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
    private bool _lineExists;
    private float _playerSpeed;
    #endregion

    #region references
    private GameObject _target;
    private Transform _myTransform;
    private Rigidbody2D _myRigidBody2D;
    private LineRenderer _myLineRenderer;
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
        // las balas se destruyen cuando chocan contra cualquier cosa
        // excepto los enemigos u otras balas
        // se tiene que colocar debajo
        if (_lineExists)
        {
            _myLineRenderer.positionCount = 0;
        }
        _myRigidBody2D.velocity = Vector3.zero;
        Destroy(this.gameObject);
        PlayerLifeComponent _playerLifeComponent = collision.gameObject.GetComponent<PlayerLifeComponent>();
        if (_playerLifeComponent != null)
        {
            // _playerLifeComponent.Damage(_damage);
            if (_lineExists)
            {
                collision.gameObject.GetComponent<PlayerMovementController>().SetSpeed(_playerSpeed);
            }
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _target = GameObject.Find("Player");
        if (_target != null)
        {
            Vector3 temp = (_target.transform.position - _myTransform.position).normalized;
            _myTransform.up = temp;
            _myRigidBody2D = GetComponent<Rigidbody2D>();
            _myRigidBody2D.velocity = (_myTransform.up * _projectileSpeed);
        }

        // ataque de la araña
        _myLineRenderer = GetComponent<LineRenderer>();
        _lineExists = _myLineRenderer != null;
        if (_lineExists)
        {
            _myLineRenderer.positionCount = 2;
            _myLineRenderer.SetPosition(0, _myTransform.position);
            _myLineRenderer.SetPosition(1, _myTransform.position);
            _myLineRenderer.endColor = Color.white;
            _myLineRenderer.startColor = Color.white;
        }
    }

    private void Update()
    {
        if (_lineExists)
        {
            _myLineRenderer.SetPosition(1, _myTransform.position);
        }
    }
}
