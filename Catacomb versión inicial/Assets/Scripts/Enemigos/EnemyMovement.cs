using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _speed = 1.0f;  
    [SerializeField]
    private float _range = 5.0f;
    [SerializeField]
    private bool ranged = false;
    private int layers;
    #endregion

    #region properties
    protected Vector3 _movementDirection;
    protected Vector3 _playerDirection;
    #endregion

    #region references
    private Transform _myTransform;
    private bool _isReloading;
    private GameObject targetObject;
    private Transform targetTransform;
    private RaycastHit2D hit;
    private RaycastHit2D leftRay;
    private RaycastHit2D rightRay;
    private int offset;
    private Green _myGreenComponent;
    private Yellow _myYellowComponent;
    #endregion

    #region methods
    private void SetPlayerDirection()
    {
        _playerDirection = (targetTransform.position - this.transform.position).normalized;
    }

    private void SetEscapeDirection()
    {
        _movementDirection = (this.transform.position - targetTransform.position).normalized;
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        targetObject = GameObject.Find("Player");
        targetTransform = targetObject.transform;
        SetPlayerDirection();
        offset = 45;
        RaycastHit2D hitinfo  = Physics2D.Raycast(this.transform.position, _movementDirection);
        layers = 1 << 8 | 1 << 3;
        _myGreenComponent = GetComponent<Green>();
        if (_myGreenComponent != null)
        {
            _speed += _myGreenComponent.IncreasedSpeed();
        }
        if(_myYellowComponent != null)
        {
            _range = _myYellowComponent.IncreasedRange();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //SetMovementDirection();
        Vector3 temp = (targetTransform.position - transform.position);
        Debug.DrawRay(transform.position, _playerDirection *  100.0f, Color.red, 1.0f);
        hit = Physics2D.Raycast(transform.position, temp.normalized, 100.0f, layers);
        SetPlayerDirection();
        if (hit.collider == targetObject.GetComponent<Collider2D>())
        {
            _movementDirection = temp.normalized;
            if (ranged) _isReloading = GetComponentInChildren<EnemyShooter>().reloading;
            if (hit.distance > _range) SetPlayerDirection();
            else if (_isReloading && (hit.distance < _range)) SetEscapeDirection();
        }
        else
        {
            Vector3 left = Quaternion.Euler(0, 0, offset) * _playerDirection;
            Vector3 right = Quaternion.Euler(0, 0, -offset) * _playerDirection;
            Debug.DrawRay(transform.position, left.normalized * 100.0f, Color.green, 1.0f);
            Debug.DrawRay(transform.position, right.normalized * 100.0f, Color.yellow, 1.0f);
            leftRay = Physics2D.Raycast(this.transform.position, left.normalized, 100.0f);
            rightRay = Physics2D.Raycast(this.transform.position, right.normalized, 100.0f);
            if (leftRay.distance > rightRay.distance) _movementDirection = left.normalized;
            else if (rightRay.distance > leftRay.distance) _movementDirection = right.normalized;
            else if (leftRay.distance == 0) _movementDirection = left.normalized;
            else if (rightRay.distance == 0) _movementDirection = right.normalized;
        }
        _myTransform.Translate(_speed * _movementDirection * Time.deltaTime);
    }



    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Vector3 direc = new Vector3(0, 0, -90);
    //    _movementDirection += direc;
    //    Debug.Log("moving");
    //}
}
