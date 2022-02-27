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
    public float _rightStickHorInput;
    public float _rightStickVerInput;
    #endregion

    #region references
    private PlayerMovementController _myPlayerMovementController;
    private PlayerAttackController _myPlayerAttackController;
    private PlayerChangeColors _myPlayerChangeColors;
    GameObject _dirArrow;
    DirectionArrow _directionArrow;
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
        // ataque principal
        if (Input.GetButtonDown("Fire1"))
        {
            _myPlayerAttackController.MainAttack();
        }

        // rodar
        // eje de input que funciona cuando se pulsa la tecla "E"
        if (Input.GetButtonDown("Roll"))
        {
            _myPlayerMovementController.Rodar();
        }
        // movimiento
        else
        {
            _verticalInput = Input.GetAxis("Vertical");
            _horizontalInput = Input.GetAxis("Horizontal");

            _myPlayerMovementController.SetMovementDirection(new Vector3(_horizontalInput, _verticalInput, 0));
        }

        // cambiar de color
        _scrollInput = Input.GetAxis("Mouse ScrollWheel");
        _myPlayerChangeColors.ChangeColor(_scrollInput);

        // apuntar con la flecha de dirección
        /*
        _rightStickHorInput = Input.GetAxis("RightStick Horizontal");
        _rightStickVerInput = Input.GetAxis("RightStick Vertical");
        Vector3 rightStick = new Vector3(_rightStickHorInput, _rightStickVerInput, 0);
        _directionArrow.SetDirection(rightStick);
        */
    }
}
