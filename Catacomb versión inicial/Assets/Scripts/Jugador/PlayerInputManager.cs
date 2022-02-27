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
    private float _scrollInput;
    private float _rightHorizontal;
    private float _rightVertical;
    #endregion

    #region references
    private PlayerMovementController _myPlayerMovementController;
    private PlayerAttackController _myPlayerAttackController;
    private PlayerChangeColors _myPlayerChangeColors;
    private GameObject _dirArrow;
    private DirectionArrow _directionArrow;
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
        _myPlayerAttackController = GetComponent<PlayerAttackController>();
        _myPlayerChangeColors = GetComponent<PlayerChangeColors>();
        _dirArrow = GameObject.Find("DirectionArrow");
        _directionArrow = _dirArrow.GetComponent<DirectionArrow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _myPlayerAttackController.MainAttack();
        }

        // eje de input que funciona cuando se pulsa la barra espaciadora
        if (Input.GetButtonDown("Roll"))
        {
            _myPlayerMovementController.Rodar();
        }
        else
        {
            _verticalInput = Input.GetAxis("Vertical");
            _horizontalInput = Input.GetAxis("Horizontal");

            _myPlayerMovementController.SetMovementDirection(new Vector3(_horizontalInput, _verticalInput, 0));
        }

        _scrollInput = Input.GetAxis("Mouse ScrollWheel");
        _myPlayerChangeColors.ChangeColor(_scrollInput);
        _rightHorizontal = Input.GetAxis("RightHorizontal");
        _rightVertical = Input.GetAxis("RightVertical");
        Vector3 right = new Vector3(_rightHorizontal, _rightVertical, 0);
        if (right != Vector3.zero)
        {
            _directionArrow.SetDirection(right);
        }
    }
}
