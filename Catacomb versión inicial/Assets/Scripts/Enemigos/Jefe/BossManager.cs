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
    [SerializeField]
    private Vector3[] _nestPositions;
    [SerializeField]
    private int _numNestsSpawn;
    [SerializeField]
    private float _oftenAttack;
    [SerializeField]
    private int _spiderWebDamage;
    #endregion

    #region properties
    // estado del jefe ya que es un máquina de estados
    private int _state;
    public int State { get => _state; }
    // contador del número de patas que debería tener la araña en este momento
    private int _numLegs;
    // booleano que sirve para hacer que los Invoke se ejecuten una sola vez
    private bool _transitionMade;
    // indica el nido de araña que aparece en la escena
    private int _nestIndex;
    // vector de booleanos que sirve para indicar si un nido está desactivado o no
    // true --> está activado
    // false --> está desactivado
    private bool[] _nestsActivated;
    private int _numNests;
    private float _elapsedTime;
    #endregion

    #region references
    [SerializeField]
    private GameObject _rndLeg;
    private GameObject[] _spiderLegs;
    [SerializeField]
    private GameObject _spiderBody;
    private Transform _spiderBodyTransform;
    // la cabeza del jefe siempre es del mismo color
    private GameObject _spiderHead;
    private Transform _spiderHeadTransform;
    private SpriteRenderer _spiderHeadSprite;
    private GameObject[] _nests;
    [SerializeField]
    private GameObject _nestPrefab;
    [SerializeField]
    private GameObject _spiderWebBullet;
    #endregion

    #region methods
    private void SpiderWebAttack()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _oftenAttack)
        {
            GameObject spiderWeb = Instantiate(_spiderWebBullet, _spiderHeadTransform.position, Quaternion.identity);
            spiderWeb.GetComponent<ProjectileMovement>().SetDamage(_spiderWebDamage);
            _elapsedTime = 0;
        }
    }

    // lógica del spawn de los nidos de araña
    private void SpawnNests()
    {
        CreateNests();
        bool todosActivados = false;
        int i = 0;
        while (i < _numNestsSpawn && !todosActivados)
        {
            todosActivados = CheckAllNests();
            ActivateNests(todosActivados, ref i);
        }
    }

    private void CreateNests()
    {
        // los nidos se instancian en escena y se desactivan
        // posteriormete se irán desactivando de dos en dos
        for (int i = 0; i < _numNests; i++)
        {
            if (_nests[i] == null)
            {
                _nests[i] = Instantiate(_nestPrefab, _nestPositions[i], Quaternion.identity);
                _nests[i].SetActive(false);
                _nestsActivated[i] = false;
            }
        }
    }

    private void DestroyNests()
    {
        for (int i = 0; i < _numNests; i++)
        {
            if (_nests[i] != null)
            {
                GameObject.Destroy(_nests[i]);
            }
        }
    }

    private bool CheckAllNests()
    {
        // comprueba si todos los nidos están activados
        // si lo están devuelve true
        // en caso contrario, devuelve false
        bool check = true;
        int j = 0;
        while (j < _numNests && check)
        {
            if (!_nestsActivated[j])
            {
                check = false;
            }
            j++;
        }
        return check;
    }

    private void ActivateNests(bool allActivated, ref int index)
    {
        // si no están todos los nidos activados procede a activar varios
        if (!allActivated)
        {
            // si el nido no está activado
            // lo activa, lo marco como activado y
            // aumenta index en 1 para saber que se ha activado un nido
            if (!_nestsActivated[_nestIndex])
            {
                _nests[_nestIndex].SetActive(true);
                _nestsActivated[_nestIndex] = true;
                _nestIndex++;
                index++;
            }
            // en caso de que el nido esté activado pasa a buscar otro que no lo esté
            else
            {
                _nestIndex++;
            }
            // si llega al último nido pasa al primero
            if (_nestIndex == _numNests)
            {
                _nestIndex = 0;
            }
        }
    }

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
        _elapsedTime = 0;
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

    private void BossEliminated()
    {
        // si se destruye la cabeza, el jefe muere
        _state = -1;
        GameObject.Destroy(_spiderBody);
        DestroyNests();
        GameManager.Instance.DestroyEnemies();
        // cancelar el invoke de que el jefe pase del
        // estado 2 al 4 en el caso de que haya sido derrotado
        CancelInvoke(nameof(SecondToThird));
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // inicialiar las referencias a la araña y a sus estados
        _spiderBodyTransform = _spiderBody.GetComponent<Transform>();
        _spiderHead = _spiderBodyTransform.GetChild(0).gameObject;
        _spiderHeadTransform = _spiderHead.transform;
        _spiderHeadSprite = _spiderHead.GetComponent<SpriteRenderer>();
        _spiderHeadSprite.color = Color.white;
        _spiderLegs = new GameObject[4];
        _transitionMade = false;
        // inicializar las referencias los nidos de araña
        _numNests = _nestPositions.Length;
        _nests = new GameObject[_numNests];
        _nestsActivated = new bool[_numNests];
        _nestIndex = 0;

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
                SpiderWebAttack();
                int contLegs = CountCurrentLegs();
                if (contLegs == 0)
                {
                    ZeroToSecond();
                }
                else if (contLegs < _numLegs)
                {
                    _elapsedTime = 0;
                    // se pueden eliminar varias patas a la vez
                    _numLegs = contLegs;
                    StartCoroutine(Transition(1, false, 0f));
                    SpawnNests();
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
                    InitiateStateZero();
                }
                break;
        }
    }
}
