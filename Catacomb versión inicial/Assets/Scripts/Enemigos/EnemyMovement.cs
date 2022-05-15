using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    private float diferenciax;
    private float x_scale, y_scale, z_scale;
    private float _elapsedTime2 = 0;
    #endregion

    #region properties
    protected Vector3 _movementDirection;
    protected Vector3 _playerDirection;
    #endregion

    #region references
    private Transform _myTransform;
    private GameObject targetObject;
    private Transform targetTransform;
    private RaycastHit2D hit;
    private RaycastHit2D leftRay;
    private RaycastHit2D rightRay;
    private RaycastHit2D[] hitArray;
    private Collider2D[] tooClose;
    private int offset;
    private float distanceMultiplier;
    private Green _myGreenComponent;
    private Yellow _myYellowComponent;
    private Rigidbody2D rb;
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
    public void StopMovement() { rb.velocity = Vector3.zero; }
    public float GetRange() { return _range; }
    public float GetPlayerDistance() { return hit.distance; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        targetObject = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        targetTransform = targetObject.transform;
        SetPlayerDirection();
        offset = 90;
        distanceMultiplier = 1;
        layers = 1 << 8 | 1 << 6 | 1 << 3 ;
        _myGreenComponent = GetComponent<Green>();
        if (_myGreenComponent != null)
        {
            _speed += _myGreenComponent.IncreasedSpeed();
        }
        if(_myYellowComponent != null)
        {
            _range = _myYellowComponent.IncreasedRange();
        }

        x_scale = _myTransform.localScale.x;
        y_scale = _myTransform.localScale.y;
        z_scale = _myTransform.localScale.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _elapsedTime2 += Time.deltaTime;
        if(_elapsedTime2 > 0.2f)
        {
            _elapsedTime2 = 0;
            diferenciax = Math.Abs(transform.position.x) - Math.Abs(targetObject.transform.position.x);
            if (diferenciax < 0 && transform.position.x < 0 && targetObject.transform.position.x < 0)
            {
                _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
            }
            else if (diferenciax > 0 && transform.position.x < 0 && targetObject.transform.position.x < 0)
            {
                _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
            }
            else if (diferenciax < 0 && transform.position.x > 0 && targetObject.transform.position.x > 0)
            {
                _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
            }
            else if (diferenciax > 0 && transform.position.x > 0 && targetObject.transform.position.x > 0)
            {
                _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
            }
        }
        
        Vector3 temp = (targetTransform.position - transform.position);
        Debug.DrawRay(transform.position, _playerDirection *  100.0f, Color.red, 1.0f);
        hit = FirstTargetHit();
        SetPlayerDirection();
        if (hit.collider == targetObject.GetComponent<Collider2D>())
        {
            _movementDirection = temp.normalized;
            if (hit.distance > _range) SetPlayerDirection();
            else if (ranged && (hit.distance < _range)) { SetEscapeDirection(); }
        }
        else
        {
            Vector3 left = Quaternion.Euler(0, 0, offset) * _playerDirection;
            Vector3 right = Quaternion.Euler(0, 0, -offset) * _playerDirection;
            Debug.DrawRay(transform.position, left.normalized * 100.0f, Color.green, 1.0f);
            Debug.DrawRay(transform.position, right.normalized * 100.0f, Color.yellow, 1.0f);
            leftRay = Physics2D.Raycast(this.transform.position, left.normalized, 100.0f, layers);
            rightRay = Physics2D.Raycast(this.transform.position, right.normalized, 100.0f, layers);
            if (leftRay.distance == 0) _movementDirection = left.normalized;
            else if (rightRay.distance == 0) _movementDirection = right.normalized;
            else if (leftRay.distance > rightRay.distance) _movementDirection = left.normalized;
            else if (rightRay.distance > leftRay.distance) _movementDirection = right.normalized;

            _movementDirection += SocialDistancing();
        }

        if (!ranged && hit.distance < _range) _movementDirection = Vector3.zero;

        rb.velocity = (_speed * _movementDirection);
    }

    private RaycastHit2D FirstTargetHit()
    {
        Vector3 temp = (targetTransform.position - transform.position);
        Debug.DrawRay(transform.position, _playerDirection * 100.0f, Color.red, 1.0f);
        hitArray = Physics2D.RaycastAll(transform.position, temp.normalized, 100.0f, layers);
        return hitArray[1];
    }

    private Vector3 SocialDistancing()
    {
        Vector3 movement = new Vector3(0,0,0);
        tooClose = Physics2D.OverlapCircleAll(transform.position, 4, 1 << 6);
        if (tooClose.Length > 5)
        {
            for (int i = 0; i < tooClose.Length; i++)
            {
                movement += -(tooClose[i].transform.position - transform.position);
                if (tooClose[i].transform.position.magnitude - transform.position.magnitude < 2) distanceMultiplier = 0.8f;
                else if (tooClose[i].transform.position.magnitude - transform.position.magnitude < 3) distanceMultiplier = 0.5f;
                else if (tooClose[i].transform.position.magnitude - transform.position.magnitude < 4) distanceMultiplier = 0.3f;
                movement += -(tooClose[i].transform.position - transform.position) * distanceMultiplier;
            }
        }
        return movement.normalized;
    }
}
