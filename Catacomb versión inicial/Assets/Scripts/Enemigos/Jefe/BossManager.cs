using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _durationFirstState;
    [SerializeField]
    private float _durationSecondState;
    // distancia de las patas respecto del cuerpo
    [SerializeField]
    private Vector3[] _legOffsets = { new Vector3(-3, -1, 0), new Vector3(-3, 1, 0), new Vector3(3, -1, 0), new Vector3(3, 1, 0) };
    [SerializeField]
    private float _rotationFactor;
    #endregion

    #region properties
    // estado del jefe ya que es un máquina de estados
    private int _state;
    // contador del número de patas que debería tener la araña en este momento
    private int _numLegs;
    // booleano que sirve para hacer que los Invoke se ejecuten una sola vez
    private bool _transitionMade;
    private Color _white = new Color(1, 1, 1);
    #endregion

    #region references
    [SerializeField]
    private GameObject _rndLeg;
    private GameObject[] _spiderLegs;
    [SerializeField]
    private GameObject _spiderBody;
    private Transform _spiderBodyTransform;
    // la cabeza del jefe siempre es del mismo color
    [SerializeField]
    private GameObject _spiderHead;
    private BoxCollider2D _spiderHeadCollider;
    private SpriteRenderer _spiderHeadSprite;
    #endregion

    #region methods
    // cuando el jefe pasa del segundo estado al estado cero le aparecen nuevas patas
    private void NewSpiderLegs()
    {
        for (int i = 0; i < _spiderLegs.Length; i++)
        {
            _spiderLegs[i] = Instantiate(_rndLeg, _spiderBodyTransform.position + _legOffsets[i], Quaternion.identity);
        }
    }

    private void InitiateStateZero()
    {
        _state = 0;
        NewSpiderLegs();
        _numLegs = 4;
    }

    // devuelve el número de patas actuales de la araña
    private int CountCurrentLegs()
    {
        int contLegs = 0;
        for (int i = 0; i < _spiderLegs.Length; i++)
        {
            if (_spiderLegs[i] != null)
            {
                contLegs++;
            }
        }
        return contLegs;
    }

    // pasar del estado 0 al 1 y viceversa
    private IEnumerator Transition(int state, bool activate, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _state = state;
        _transitionMade = false;
        // al activar/desactivar el cuerpo de la araña le sucede lo mismo a la cabeza
        _spiderBody.SetActive(activate);
        for (int i = 0; i < _spiderLegs.Length; i++)
        {
            if (_spiderLegs[i] != null)
            {
                _spiderLegs[i].SetActive(activate);
            }
        }
    }

    private void SecondToThird()
    {
        _state = 3;
        _transitionMade = false;
        // el jefe tiene que rotar en sentido contrario para volver a la posición inicial
        _rotationFactor = -_rotationFactor;
        _spiderHeadCollider.enabled = false;
        _spiderHeadSprite.color = _white;
    }

    private void ZeroToSecond()
    {
        _state = 2;
        // se puede golpear a la cabeza y, por lo tanto, el jefe puede sufrir daño
        _spiderHeadCollider.enabled = true;
        _spiderHeadSprite.color = GameManager.Instance.Colors[0];
    }

    // en la fase 2 el jefe rota para que sea más difícil golpearle
    private void SpiderRotation()
    {
        _spiderBodyTransform.Rotate(_spiderBodyTransform.forward, _rotationFactor * Time.deltaTime);
        float rotZ = _spiderBodyTransform.rotation.eulerAngles.z;
        if (rotZ > 90f && rotZ < 270f)
        {
            _rotationFactor = -_rotationFactor;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _spiderBodyTransform = _spiderBody.GetComponent<Transform>();
        _spiderLegs = new GameObject[4];
        _spiderHeadCollider = _spiderHead.GetComponent<BoxCollider2D>();
        _spiderHeadSprite = _spiderHead.GetComponent<SpriteRenderer>();
        _spiderHeadCollider.enabled = false;
        _spiderHeadSprite.color = _white;
        _transitionMade = false;

        // el jefe comienza en el estado 0
        InitiateStateZero();
    }

    // Update is called once per frame
    void Update()
    {
        // el jefe tiene 4 estados
        switch (_state)
        {
            case 0:
                Debug.Log("estado 0");
                int contLegs = CountCurrentLegs();
                if (contLegs == 0)
                {
                    ZeroToSecond();
                }
                else if (contLegs < _numLegs)
                {
                    // se pueden eliminar varias patas a la vez
                    _numLegs = contLegs;
                    StartCoroutine(Transition(1, false, 0f));
                }
                break;

            case 1:
                Debug.Log("estado 1");
                if (!_transitionMade)
                {
                    _transitionMade = true;
                    StartCoroutine(Transition(0, true, _durationFirstState));
                }
                break;

            case 2:
                Debug.Log("estado 2");
                // eliminar al jefe
                if (_spiderHead == null)
                {
                    // si se destruye la cabeza, el jefe muere
                    _state = -1;
                    GameObject.Destroy(_spiderBody);
                    // cancelar el invoke de que el jefe pase del
                    // estado 2 al 4 en el caso de que haya sido derrotado
                    CancelInvoke(nameof(SecondToThird));
                }

                SpiderRotation();

                if (!_transitionMade)
                {
                    _transitionMade = true;
                    Invoke(nameof(SecondToThird), _durationSecondState);
                }
                break;

            case 3:
                // el jefe rota para regresar a la posición inicial
                Debug.Log("estado 3");
                float rotZ = _spiderBodyTransform.rotation.eulerAngles.z;
                // valores cercanos a 360 y 0 porque son float
                if (rotZ < 359f && rotZ > 1f)
                {
                    _spiderBodyTransform.Rotate(_spiderBodyTransform.forward, _rotationFactor * Time.deltaTime);
                }
                else
                {
                    // terminar de ajustar la posición del jefe
                    _spiderBodyTransform.rotation = Quaternion.identity;
                    InitiateStateZero();
                }
                break;
        }
    }
}
