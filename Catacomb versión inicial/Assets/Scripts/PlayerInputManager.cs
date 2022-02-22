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
    private PlayerAttackController _myPlayerAttackController;
    private Camera _mainCamera;
    [SerializeField]
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

    // la conversión de un punto de pantalla a un punto de mundo se realiza en el script relativo al input
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
        _myPlayerMovementController = GetComponent<PlayerMovementController>();
        _myPlayerAttackController = GetComponent<PlayerAttackController>();
        _mainCamera = Camera.main;
        _directionArrow = _dirArrow.GetComponent<DirectionArrow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _myPlayerAttackController.MainAttack();
        }

        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");

        _myPlayerMovementController.SetMovementDirection(new Vector3 (_horizontalInput, _verticalInput, 0));

        // se tiene que poner en el script de input porque se está convirtiendo la posición del cursor en un punto de mundo
        _directionArrow.PointingDirection(WorldPointWithoutZ(Input.mousePosition));
    }
}
