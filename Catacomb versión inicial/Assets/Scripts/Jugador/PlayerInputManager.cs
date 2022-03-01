using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _inputDeadZone;
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
    private void DeadZone(ref Vector3 input)
    {
        if (input.magnitude < _inputDeadZone)
        {
            input= Vector3.zero;
        }
        else
        {
            input = input.normalized * ((input.magnitude - _inputDeadZone) / (1 - _inputDeadZone));
        }
    }

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
        // ataque principal
        if (Input.GetButtonDown("Fire1"))
        {
            _myPlayerAttackController.MainAttack();
        }

        // rodar
        if (Input.GetButtonDown("Roll"))
        {
            _myPlayerMovementController.Rodar();
        }

        else
        {
            _verticalInput = Input.GetAxis("Vertical");
            _horizontalInput = Input.GetAxis("Horizontal");
            Vector3 movementInput = new Vector3(_horizontalInput, _verticalInput, 0);
            _myPlayerMovementController.SetMovementDirection(movementInput);
        }

        // cambiar de color
        _scrollInput = Input.GetAxisRaw("Mouse ScrollWheel");
        if (_scrollInput != 0)
        {
            _myPlayerChangeColors.ChangeColor(_scrollInput);
        }

        // girar la flecha de dirección con el mando
        _rightHorizontal = Input.GetAxis("RightHorizontal");
        _rightVertical = Input.GetAxis("RightVertical");
        Vector3 right = new Vector3(_rightHorizontal, _rightVertical, 0);
        DeadZone(ref right);
        if (right.magnitude != 0)
        {
            _directionArrow.SetDirection(right);
        }
    }
}
