using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    #region parameters
    [SerializeField]
    float smoothSpeed = 0.2f;
    #endregion

    #region references
    private GameObject _player;
    private Transform _playerTransform;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _playerTransform = _player.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPos = new Vector3(_playerTransform.position.x, _playerTransform.position.y, transform.position.z);
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothedPos;
    }
}
