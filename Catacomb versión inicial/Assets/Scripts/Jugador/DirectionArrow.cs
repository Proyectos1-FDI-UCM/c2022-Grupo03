using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties

    #endregion

    #region references
    [SerializeField]
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
        _myTransform.right = newDirection;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = _player.transform;
        _mainCamera = Camera.main;
        _myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // hacer que la flecha de dirección esté siempre en la posición del jugador
        _myTransform.position = _playerTransform.position;

        if (Input.GetJoystickNames().Length == 0)
        {
            // dirección de la flecha, que indica hacia donde se está apuntando
            Vector3 targetPoint = WorldPointWithoutZ(Input.mousePosition);
            Vector3 dir = targetPoint - _playerTransform.position;
            _myTransform.right = dir.normalized;
        }
    }
}
