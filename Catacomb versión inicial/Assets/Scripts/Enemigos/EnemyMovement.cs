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
    #endregion

    #region properties
    protected Vector3 _movementDirection;
    #endregion

    #region references
    private Transform _myTransform;
    private bool _isReloading;
    private GameObject targetObject;
    private Transform targetTransform;
    #endregion

    #region methods
    private void SetMovementDirection()
    {
        _movementDirection = (targetTransform.position - this.transform.position).normalized;
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
        SetMovementDirection();
    }

    // Update is called once per frame
    void Update()
    {
        //SetMovementDirection();
        Vector3 temp = (targetTransform.position - this.transform.position);
        if(ranged) _isReloading = GetComponentInChildren<EnemyShooter>().reloading;
        if (temp.x > _range || temp.y > _range)
        {
            SetMovementDirection();
            _myTransform.Translate(_speed * _movementDirection * Time.deltaTime);
        }
        else if (_isReloading && (temp.x < _range / 2 || temp.y < _range / 2))
        {
            SetEscapeDirection();
            _myTransform.Translate(_speed * _movementDirection * Time.deltaTime);
        }
    }
}
