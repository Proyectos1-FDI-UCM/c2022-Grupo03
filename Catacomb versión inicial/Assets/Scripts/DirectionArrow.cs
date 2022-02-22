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
    #endregion

    #region methods
    // se llama desde el PlayerInputManager
    public void PointingDirection(Vector3 targetPoint)
    {
        Vector3 dir = targetPoint - _myTransform.position;
        _myTransform.right = dir;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
