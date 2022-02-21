using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties
    private float _horizontalInput;
    private float _verticalInput;
    #endregion

    #region references
    private PlayerMovementController _myPlayerMovementController;
    #endregion

    #region methods
    public float HInput()
    {
        return _horizontalInput;
    }
    public float VInput()
    {
        return _verticalInput;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myPlayerMovementController = GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");

        _myPlayerMovementController.SetMovementDirection(new Vector3 (_horizontalInput, _verticalInput, 0));
    }
}
