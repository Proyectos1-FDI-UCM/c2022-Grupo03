using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _attackDuration;
    #endregion

    #region properties
    private float _elapsedTime;
    private bool _attackOn;
    #endregion

    #region references
    Transform _myTransform;
    [SerializeField]
    GameObject _dmgLeft;
    [SerializeField]
    GameObject _dmgRight;
    [SerializeField]
    GameObject _dmgUp;
    [SerializeField]
    GameObject _dmgDown;
    Camera _mainCamera;
    [SerializeField]
    GameObject _dirArrow;
    Transform _dirArrowTransform;
    GameObject _lastAttack;
    PlayerMovementController _myPlayerMovementController;
    #endregion

    #region methods
    public void MainAttack()
    {
        Vector3 angle = _dirArrowTransform.rotation.eulerAngles;

        if (!_attackOn)
        {
            // sirve para reiniciar la colisión ya que, si el personaje vuelve a realizar el ataque principal,
            // pero no se ha movido previamente, no se vuelve a detectar esta colisión
            _myTransform.position = _myTransform.position + new Vector3(0.000001f, 0, 0);

            if (angle.z >= 45 && angle.z < 135)
            {
                _dmgUp.SetActive(true);
                _lastAttack = _dmgUp;
            }
            else if (angle.z >= 135 && angle.z < 225)
            {
                _dmgLeft.SetActive(true);
                _lastAttack = _dmgLeft;
            }
            else if (angle.z >= 225 && angle.z < 315)
            {
                _dmgDown.SetActive(true);
                _lastAttack = _dmgDown;
            }
            else
            {
                _dmgRight.SetActive(true);
                _lastAttack = _dmgRight;
            }

            _attackOn = true;
        }

    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _mainCamera = Camera.main;
        _dirArrowTransform = _dirArrow.transform;
        _attackOn = false;
        _myPlayerMovementController = GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackOn)
        {
            // sincronizar el tiempo que dura el ataque con el tiempo que el jugador no puede moverse
            _elapsedTime += Time.deltaTime;
            _myPlayerMovementController.SetMovementDirection(Vector3.zero);
            if (_elapsedTime > _attackDuration)
            {
                _attackOn = false;
                _lastAttack.SetActive(false);
                _elapsedTime = 0;
            }
        }
    }
}
