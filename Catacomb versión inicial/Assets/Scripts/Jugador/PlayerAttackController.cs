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
    [SerializeField]
    private GameObject _damageZone;
    Transform _myTransform;
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

            // ajustar los ángulos menores que 45º y mayores o iguales que 0º para que su índice sea 3
            if (angle.z < 45 && angle.z >= 0)
            {
                angle.z = angle.z + 360;
            }

            int indice = (int)(angle.z - 45) / 90;

            Quaternion rotation;
            Vector3 offset;
            if (indice % 2 == 0)
            {
                rotation = Quaternion.identity;
                offset = new Vector3(0, 1, 0);
                if (indice == 2)
                {
                    offset = -offset;
                }
            }
            else
            {
                rotation = Quaternion.Euler(0, 0, 90);
                offset = new Vector3(1, 0, 0);
                if (indice == 1)
                {
                    offset = -offset;
                }
            }
            Vector3 instPoint = transform.position + offset;
            
            _lastAttack = Instantiate(_damageZone, instPoint, rotation);

            _attackOn = true;
        }

    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
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
                GameObject.Destroy(_lastAttack);
                _elapsedTime = 0;
            }
        }
    }
}
