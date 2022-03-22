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
    private Rigidbody2D rb;
    #endregion

    #region methods
    public void SetMovementDirection(Vector3 newMovementDirection)
    {
        _movementDirection = newMovementDirection;
    }
    public void Rodar()
    {
        _movementDirection = new Vector3(10,0,0);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = (_speed * _movementDirection);
        //rb.MovePosition(_speed * _movementDirection * Time.deltaTime);
    }
}
