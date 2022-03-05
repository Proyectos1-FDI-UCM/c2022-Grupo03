using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GeneralEnemyMovement : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private float _distance = 1.5f;
    #endregion

    #region properties
    protected Vector3 _movementDirection;
    #endregion

    #region references
    private Transform _myTransform;
    private bool _isReloading;
    private GameObject targetObject;
    private Transform targetTransform;
    private EnemyMelee _meleeEnemy;
    private Green _myGreenComponent;
    #endregion

    #region methods
    private void SetMovementDirection()
    {
        _movementDirection = (targetTransform.position - this.transform.position).normalized;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _meleeEnemy = GetComponent<EnemyMelee>();
        _myTransform = transform;
        targetObject = GameObject.Find("Player");
        targetTransform = targetObject.transform;
        SetMovementDirection();
        _myGreenComponent = GetComponent<Green>();
        if (_myGreenComponent != null)
        {
            _speed += _myGreenComponent.IncreasedSpeed();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //SetMovementDirection();
        Vector3 temp = (targetTransform.position - this.transform.position);
        if (!_meleeEnemy.atacando() && Math.Abs(temp.x) > _distance)
        {
            SetMovementDirection();
            _myTransform.Translate(_speed * _movementDirection * Time.deltaTime);
        }
        else
        {
            _myTransform.Translate(Vector3.zero);
        }
    }
}
