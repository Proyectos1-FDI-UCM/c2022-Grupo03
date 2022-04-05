using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    // 1º pata de arriba a la izquierda
    // 2º pata de abajo a la izquierda
    // 3º pata de arriba a la derecha
    // 4º pata de abajo a la derecha
    #region parameters
    [SerializeField]
    private float _timeToStart;
    [SerializeField]
    private float _durationFirstState;
    [SerializeField]
    private float _durationSecondState;
    // distancia de las patas respecto del cuerpo
    [SerializeField]
    private Vector3[] _legOffsets;
    [SerializeField]
    private Vector3 _legsSize;
    [SerializeField]
    private float _rotationFactor;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _movementDuration;
    #endregion

    #region properties
    // estado del jefe ya que es un máquina de estados
    private int _state;
    public int State { get => _state; }
    // contador del número de patas que debería tener la araña en este momento
    private int _numLegs;
    // booleano que sirve para hacer que los Invoke se ejecuten una sola vez
    private bool _transitionMade;

    private float _elapsedTime;
    #endregion

    #region references
    [SerializeField]
    private GameObject[] _legs;
    private GameObject[] _spiderLegs;
    [SerializeField]
    private GameObject _spiderBody;
    private Transform _spiderBodyTransform;
    // la cabeza del jefe siempre es del mismo color
    [SerializeField]
    private GameObject _spiderHead;
    private SpriteRenderer _spiderHeadSprite;
    private NestSpawner _myNestSpawner;
    private SpiderWebAttack _mySpiderWebAttack;
    [SerializeField]
    private GameObject _bossLifeBar;
    private Collider2D[] _legsColliders;
    #endregion

    #region methods
    // cuando el jefe pasa del segundo estado al estado cero le aparecen nuevas patas
    private void NewSpiderLegs()
    {
        for (int i = 0; i < _spiderLegs.Length; i++)
        {
            _spiderLegs[i] = Instantiate(_legs[i], _spiderBodyTransform);
            // situar las patas y darlas el tamaño adecuado
            Transform spiderLegTransform = _spiderLegs[i].transform;
            spiderLegTransform.localPosition = _legOffsets[i];
            _spiderLegs[i].transform.localScale = _legsSize;

            // obtener los colliders de las patas
            _legsColliders[i] = _spiderLegs[i].GetComponent<Collider2D>();
        }
    }

    private void InitiateSpider(int state)
    {
        _state = state;
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

    // pasar del 1 a moverse hacia abajo
    private void FirstToGoDown()
    {
        _state = -1;
        _transitionMade = false;
        SetSpider(true);   // cuando ha pasado cierto tiempo vuelve a aparecer
    }

    // activar/desactivar la araña
    private void SetSpider(bool activate)
    {
        _spiderBody.SetActive(activate);
    }

    private void SecondToThird()
    {
        _state = 3;
        _transitionMade = false;
        // el jefe tiene que rotar en sentido contrario para volver a la posición inicial
        _rotationFactor = -_rotationFactor;
        _spiderHeadSprite.color = Color.white;
    }

    private void ZeroToSecond()
    {
        _state = 2;
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

    // cuando se destruye la cabeza, el jefe muere
    private void BossEliminated()
    {
        GameObject.Destroy(_spiderBody);
        _myNestSpawner.DestroyNests();
        GameManager.Instance.DestroyEnemies();
        // cancelar el invoke de que el jefe pase del
        // estado 2 al 4 en el caso de que haya sido derrotado
        CancelInvoke(nameof(SecondToThird));
        _bossLifeBar.SetActive(false);
        gameObject.SetActive(false);
    }

    private void StartBoss()
    {
        _spiderBody.SetActive(true);
        _bossLifeBar.SetActive(true);
        // el jefe comienza bajando hacia abajo
        InitiateSpider(-1);
        SetLegColliders(false);
    }

    // mover a la araña hacia abajo o hacia arriba
    private bool SpiderMovement(int dir)
    {
        bool stop = false;
        _spiderBodyTransform.Translate(dir * _spiderBodyTransform.up * _speed * Time.deltaTime);
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _movementDuration)
        {
            stop = true;
            _elapsedTime = 0;
        }
        return stop;
    }

    // cuando la araña se mueve no se le puede golpear a las piernas,
    // por lo tanto, sus colliders se desactivan
    private void SetLegColliders(bool activate)
    {
        for (int i = 0; i < _spiderLegs.Length; i++)
        {
            if (_spiderLegs[i] != null)
            {
                _legsColliders[i].enabled = activate;
            }
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // referencias a otros componentes
        _spiderBodyTransform = _spiderBody.GetComponent<Transform>();
        _spiderHeadSprite = _spiderHead.GetComponent<SpriteRenderer>();
        _spiderLegs = new GameObject[4];
        _transitionMade = false;
        _myNestSpawner = GetComponent<NestSpawner>();
        _mySpiderWebAttack = GetComponent<SpiderWebAttack>();
        _legsColliders = new Collider2D[4];

        _state = -2;
        Invoke(nameof(StartBoss), _timeToStart);
    }

    // Update is called once per frame
    void Update()
    {
        // el jefe tiene 3 estados principales más 3 de transición
        switch (_state)
        {
            case -1:
                // primero aparece y luego, se mueve
                _spiderHeadSprite.color = Color.white;
                Debug.Log("estado -1");
                if (SpiderMovement(-1))
                {
                    _state = 0;
                    SetLegColliders(true);
                }
                break;

            case 0:
                Debug.Log("estado 0");
                _mySpiderWebAttack.SpiderWeb();
                int contLegs = CountCurrentLegs();
                if (contLegs == 0)
                {
                    ZeroToSecond();
                }
                else if (contLegs < _numLegs)
                {
                    _mySpiderWebAttack.ElapsedTime = 0;
                    // se pueden eliminar varias patas a la vez
                    _numLegs = contLegs;

                    // pasa de estar en el estado 0 a moverse hacia arriba
                    _state = 4;
                    SetLegColliders(false); // se des
                    _myNestSpawner.SpawnNests();
                }
                break;

            case 4:
                // primero se mueve y luego, desaparece
                Debug.Log("estado 4");
                // la araña se mueve hacia arriba
                if (SpiderMovement(1))
                {
                    // cuando ha avanzado lo suficiente se desactiva
                    SetSpider(false);
                    _state = 1;
                }
                break;

            case 1:
                Debug.Log("estado 1");
                if (!_transitionMade)
                {
                    _transitionMade = true;
                    Invoke(nameof(FirstToGoDown), _durationFirstState);
                }
                break;

            case 2:
                Debug.Log("estado 2");
                // eliminar al jefe
                if (_spiderHead == null)
                {
                    BossEliminated();
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
                    InitiateSpider(0);
                }
                break;
        }
    }
}
