using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    #region parameters
    #endregion

    #region properties
    private string[] _joysticks;
    private bool _controllerConnected;
    private bool _check;
    private float _elapsedTime;
    [SerializeField]
    private float _durationUntilCheck;
    // cuanto tiempo tiene que pasar sin que haya input para que se comprueba si hay un mando conectado o no
    #endregion

    #region references
    GameObject _player;
    Transform _playerTransform;
    Camera _mainCamera;
    Transform _myTransform;
    #endregion

    #region methods
    private Vector3 WorldPointWithoutZ(Vector3 screenPoint)
    {
        Vector3 worldPoint;
        worldPoint = _mainCamera.ScreenToWorldPoint(screenPoint);
        worldPoint.z = 0;
        return worldPoint;
    }

    public void SetDirection(Vector3 newDirection)
    {
        if (_controllerConnected)
        {
            float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            _myTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _playerTransform = _player.transform;
        _mainCamera = Camera.main;
        _myTransform = transform;
        _controllerConnected = false;
        _check = false;
    }

    // Update is called once per frame
    void Update()
    {
        // hacer que la flecha de direcci�n est� siempre en la posici�n del jugador
        _myTransform.position = _playerTransform.position;

        if (!_controllerConnected)
        {
            // direcci�n de la flecha, que indica hacia donde se est� apuntando
            Vector3 targetPoint = WorldPointWithoutZ(Input.mousePosition);
            Vector3 dir = targetPoint - _playerTransform.position;
            _myTransform.right = dir.normalized;
        }

        // contador de tiempo que se inicia cuando no se recibe input (no ha cambiado el transform de la flecha)
        // si llega al tiempo establecido, si hay un mando conectado o no
        if (_myTransform.hasChanged)
        {
            _myTransform.hasChanged = false;
            _elapsedTime = 0;
        }
        else
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _durationUntilCheck)
            {
                _check = true;
                _elapsedTime = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_check)
        {
            _joysticks = Input.GetJoystickNames();
            // comprobar si se ha conectado o desconectado alg�n mando
            for (int i = 0; i < _joysticks.Length; i++)
            {
                if (string.IsNullOrEmpty(_joysticks[i]))
                {
                    _controllerConnected = false;
                    Cursor.visible = true;
                }
                else
                {
                    _controllerConnected = true;
                    Cursor.visible = false;
                }
            }
            _check=false;
        }
    }
}
