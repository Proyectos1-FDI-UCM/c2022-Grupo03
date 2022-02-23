using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _speed = 1.0f;
    #endregion

    #region properties
    private Vector3 _movementDirection;
    #endregion

    #region references
    private Transform _myTransform;
    #endregion

    #region methods
    public void SetMovementDirection(Vector3 newMovementDirection)
    {
        _movementDirection = newMovementDirection;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _myTransform.Translate(_speed * _movementDirection * Time.deltaTime);
    }
}
