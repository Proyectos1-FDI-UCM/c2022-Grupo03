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
    Transform _myTransform;
    Camera _mainCamera;
    #endregion

    #region methods

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = 0;
        Vector3 dir = worldPoint - _myTransform.position;
        _myTransform.up = dir;
    }
}
