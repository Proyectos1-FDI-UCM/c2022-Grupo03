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
    public GameObject target;
    [SerializeField]
    private bool ranged = false;
    #endregion

    #region properties
    protected Vector3 _movementDirection;
    #endregion

    #region references
    private Transform _myTransform;
    private bool _isReloading;
    #endregion

    #region methods
    private void SetMovementDirection()
    {
        _movementDirection = (target.transform.position - this.transform.position).normalized;
    }

    private void SetEscapeDirection()
    {
        _movementDirection = (this.transform.position - target.transform.position).normalized;
    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        SetMovementDirection();
        _myTransform = transform;
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //SetMovementDirection();
        Vector3 temp = (target.transform.position - this.transform.position);
        if(ranged) _isReloading = GetComponentInChildren<EnemyShooter>().reloading;
        if (temp.x > _range || temp.y > _range)
        {
            SetMovementDirection();
            _myTransform.Translate(_speed * _movementDirection * Time.deltaTime);
        }
        else if (ranged && _isReloading && (temp.x < _range / 2 || temp.y < _range / 2)) 
        {
            SetEscapeDirection();
            _myTransform.Translate(_speed * _movementDirection * Time.deltaTime);        }
    }
}
