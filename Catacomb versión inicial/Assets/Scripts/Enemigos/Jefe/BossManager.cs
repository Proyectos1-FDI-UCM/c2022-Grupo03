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
    [SerializeField]
    private Vector3[] _offsets = { new Vector3(-3, -1, 0), new Vector3(-3, 1, 0), new Vector3(3, -1, 0), new Vector3(3, 1, 0) };
    [SerializeField]
    private float _rotationFactor;
    #endregion

    #region properties
    private int _state;
    private int _numLegs;
    // private int _contLegs;
    private float _elapsedTime;
    private bool _headWorking;
    private bool _transitionMade;
    private float rotZ;
    private Color _white = new Color(1, 1, 1);
    #endregion

    #region references
    [SerializeField]
    private GameObject _rndLeg;
    private GameObject[] _spiderLegs;
    [SerializeField]
    private GameObject _spiderBody;
    private Transform _spiderBodyTransform;
    [SerializeField]
    private GameObject _spiderHead;
    private BoxCollider2D _spiderHeadCollider;
    private SpriteRenderer _spiderHeadSprite;
    #endregion

    #region methods
    // una de las cosas que sucede cuando el jefe pasa del segundo estado
    // al estado cero es que le aparecen nuevas patas
    private void NewSpiderLegs()
    {
        for (int i = 0; i < _spiderLegs.Length; i++)
        {
            _spiderLegs[i] = Instantiate(_rndLeg, _spiderBodyTransform.position + _offsets[i], Quaternion.identity);
        }
    }
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

    private IEnumerator Transition(int state, bool activate, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _state = state;
        _transitionMade = false;
        _spiderBody.SetActive(activate);
        _spiderHead.SetActive(activate);
        for (int i = 0; i < _spiderLegs.Length; i++)
        {
            if (_spiderLegs[i] != null)
            {
                _spiderLegs[i].SetActive(activate);
            }
        }
    }

    private void SecondToZero()
    {
        _state = 0;
        _spiderBodyTransform.rotation = Quaternion.identity;
        _transitionMade = false;
        _spiderHeadCollider.enabled = false;
        _headWorking = false;
        _spiderHeadSprite.color = _white;
        NewSpiderLegs();
        _numLegs = 4;
    }

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

    Quaternion myQuat;
    Quaternion targetQuat;
    float elapsedTime;
    IEnumerator prueba(float tiempo)
    {
        Debug.Log("ha entrado prueba");
        Debug.Log(myQuat);
        Debug.Log(targetQuat);
        if (myQuat != Quaternion.identity)
        {
            _spiderBodyTransform.rotation = Quaternion.RotateTowards(myQuat, Quaternion.identity, _rotationFactor);
            myQuat = Quaternion.Euler(_spiderBodyTransform.eulerAngles);
            yield return new WaitForSeconds(tiempo);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _state = 0;
        _spiderBodyTransform = _spiderBody.GetComponent<Transform>();
        _spiderLegs = new GameObject[4];
        _numLegs = _spiderLegs.Length;
        NewSpiderLegs();
        _spiderHeadCollider = _spiderHead.GetComponent<BoxCollider2D>();
        _spiderHeadCollider.enabled = false;
        _spiderHeadSprite = _spiderHead.GetComponent<SpriteRenderer>();
        _headWorking = false;
        _transitionMade = false;

        targetQuat = Quaternion.Euler(0, 360, 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case 0:
                Debug.Log("estado 0");
                int contLegs = CountCurrentLegs();
                if (contLegs == 0)
                {
                    _state = 2;
                }
                else if (contLegs < _numLegs)
                {
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
                if (!_headWorking)
                {
                    _spiderHeadCollider.enabled = true;
                    _headWorking = true;
                    // la cabeza de la araña siempre es roja
                    _spiderHeadSprite.color = Color.red;
                }

                // eliminar al jefe
                if (_spiderHead == null)
                {
                    _state = -1;
                    GameObject.Destroy(_spiderBody);
                }

                SpiderRotation();

                _elapsedTime += Time.deltaTime;
                if (_elapsedTime > _durationSecondState)
                {
                    _state = 4;
                    _elapsedTime = 0;
                    myQuat = Quaternion.Euler(_spiderBodyTransform.eulerAngles);
                }
                /*
                if (!_transitionMade)
                {
                    _transitionMade = true;
                    Invoke(nameof(SecondToZero), _durationSecondState);
                }
                */
                break;

            case 4:
                Debug.Log("estado 4");
                if (myQuat != Quaternion.identity)
                {
                    _spiderBodyTransform.rotation = Quaternion.RotateTowards(myQuat, Quaternion.identity, _rotationFactor);
                    myQuat = Quaternion.Euler(_spiderBodyTransform.eulerAngles);
                }
                    break;
        }
    }
}
