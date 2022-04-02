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
    private bool _speedChanged;
    private float _elapsedTime;
    private float _originalSpeed;
    private float _duration;
    #endregion

    #region references
    private Transform _myTransform;
    private Rigidbody2D rb;
    #endregion

    #region methods
    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
        _elapsedTime = 0;
        _speedChanged = true;
    }

    public void SetDuration(float newDuration)
    {
        _duration = newDuration;
    }

    public void SetMovementDirection(Vector3 newMovementDirection)
    {
        _movementDirection = newMovementDirection;
    }

    public void Rodar()
    {
        _movementDirection = new Vector3(10, 0, 0);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        _originalSpeed = _speed;
        _speedChanged = false;
    }

    private void Update()
    {
        if (_speedChanged)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _duration)
            {
                _speed = _originalSpeed;
                _elapsedTime = 0;
                _speedChanged = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance._currentState == GameState.inGame)
        {
            rb.velocity = (_speed * _movementDirection);

        }
    }
}
