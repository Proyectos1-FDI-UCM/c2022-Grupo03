using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetectionZone : MonoBehaviour
{
    #region parameters
    float diferenciax;
    float diferenciay;
    #endregion

    #region references
    Transform _myTransform;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // duck typing
        PlayerLifeComponent _playerLifeComponent = collider.GetComponent<PlayerLifeComponent>();
        if (_playerLifeComponent != null)
        {
            diferenciax = Math.Abs(transform.position.x) - Math.Abs(collider.transform.position.x);
            diferenciay = Math.Abs(transform.position.y) - Math.Abs(collider.transform.position.y);

            if (diferenciax < 0)
            {

            }
            else if(diferenciax > 0)
            {

            }
            else if (diferenciay < 0)
            {

            }
            else if(diferenciay > 0)
            {

            }
        }
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
