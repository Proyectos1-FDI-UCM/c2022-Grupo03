using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _attackDuration;
    [SerializeField]
    private Vector3[] _offsets = { Vector3.up, -Vector3.right, -Vector3.up, Vector3.right };
    #endregion

    #region properties
    private float _elapsedTime;
    private bool _attackOn; // indicar si se ha efectuado el ataque o no
    private Quaternion[] _rotations = { Quaternion.identity, Quaternion.Euler(0, 0, 90) };
    #endregion

    #region references
    [SerializeField]
    private GameObject _damageZone;
    Transform _myTransform;
    [SerializeField]
    GameObject _dirArrow;
    Transform _dirArrowTransform;
    GameObject _lastAttack;
    PlayerInputManager _myPlayerInputManager;
    PlayerMovementController _myPlayerMovementController;
    #endregion

    #region methods
    public void MainAttack()
    {
        Vector3 angle = _dirArrowTransform.rotation.eulerAngles;

        if (!_attackOn)
        {
            // ajustar los ángulos menores que 45º y mayores o iguales que 0º para que su índice sea 3
            if (angle.z < 45 && angle.z >= 0)
            {
                angle.z = angle.z + 360;
            }

            int indice = (int)(angle.z - 45) / 90;

            Quaternion rotation = _rotations[indice % 2];
            Vector3 offset = _offsets[indice];

            Vector3 instPoint = transform.position + offset;
            _lastAttack = Instantiate(_damageZone, instPoint, rotation);

            _attackOn = true;
            _myPlayerInputManager.enabled = false;
        }

    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _dirArrowTransform = _dirArrow.transform;
        _attackOn = false;
        _myPlayerInputManager = GetComponent<PlayerInputManager>();
        _myPlayerMovementController = GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackOn)
        {
            // sincronizar el tiempo que dura el ataque con el tiempo que el jugador no puede moverse
            _elapsedTime += Time.deltaTime;
            // aunque se desactive el script de input, hay que hacer que deje de moverse
            // porque si se estaba moviendo antes de desactivarlo se seguirá moviendo después
            _myPlayerMovementController.SetMovementDirection(Vector3.zero);
            if (_elapsedTime > _attackDuration)
            {
                _attackOn = false;
                GameObject.Destroy(_lastAttack);
                _myPlayerInputManager.enabled = true;
                _elapsedTime = 0;
            }
        }
    }
}
