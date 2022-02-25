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
    private Vector3 WorldPointWithoutZ(Vector3 screenPoint)
    {
        Vector3 worldPoint;
        worldPoint = _mainCamera.ScreenToWorldPoint(screenPoint);
        worldPoint.z = 0;
        return worldPoint;
    }
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
        Vector3 targetPoint = WorldPointWithoutZ(Input.mousePosition);
        Vector3 dir = targetPoint - _myTransform.position;
        _myTransform.right = dir.normalized;
    }
}
