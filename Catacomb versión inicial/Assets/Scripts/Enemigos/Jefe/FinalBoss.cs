using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _durationSecondState;
    [SerializeField]
    private float _durationThirdState;
    #endregion

    #region properties
    private int _state;
    private int _numLegs;
    private int _contLegs;
    public Vector3[] _offsets = { new Vector3(-3, -1, 0), new Vector3(-3, 1, 0), new Vector3(3, -1, 0), new Vector3(3, 1, 0) };
    private float _elapsedTime;
    #endregion

    #region references
    [SerializeField]
    private GameObject _rndLeg;
    public GameObject[] _spiderLegs;
    [SerializeField]
    private GameObject _spiderHead;
    public BoxCollider2D _spiderHeadCollider;
    private Transform _myTransform;
    #endregion

    #region methods

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _state = 0;
        _numLegs = 4;
        _spiderLegs = new GameObject[4];
        _myTransform = transform;
        for (int i = 0; i < _spiderLegs.Length; i++)
        {
            _spiderLegs[i] = Instantiate(_rndLeg, _myTransform.position + _offsets[i], Quaternion.identity);
        }
        _spiderHeadCollider = _spiderHead.GetComponent<BoxCollider2D>();
        _spiderHeadCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case 0:
                Debug.Log("estado 0");
                _contLegs = 0;
                _elapsedTime += Time.deltaTime;
                for (int i = 0; i < _spiderLegs.Length; i++)
                {
                    if (_spiderLegs[i] != null)
                    {
                        _contLegs++;
                        Debug.Log(_contLegs);
                    }
                }
                if (_contLegs == 0)
                {
                    _state = 2;
                }
                else if (_contLegs < _numLegs)
                {
                    _numLegs--;
                    _state = 1;
                    _spiderHead.SetActive(false);
                    for (int i = 0; i < _spiderLegs.Length; i++)
                    {
                        if (_spiderLegs[i] != null)
                        {
                            _spiderLegs[i].SetActive(false);
                        }
                    }
                }
                break;

            case 1:
                Debug.Log("estado 1");
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime > _durationSecondState)
                {
                    _elapsedTime = 0;
                    _state = 0;
                    _spiderHead.SetActive(true);
                    for (int i = 0; i < _spiderLegs.Length; i++)
                    {
                        if (_spiderLegs[i] != null)
                        {
                            _spiderLegs[i].SetActive(true);
                        }
                    }
                }
                break;

            case 2:
                Debug.Log("estado 2");
                _spiderHeadCollider.enabled = true;
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime > _durationThirdState)
                {
                    _state = 0;
                    _spiderHeadCollider.enabled = false;
                    for (int i = 0; i < _spiderLegs.Length; i++)
                    {
                        _spiderLegs[i] = Instantiate(_rndLeg, _myTransform.position + _offsets[i], Quaternion.identity);
                    }
                    _numLegs = 4;
                }
                break;
        }
    }
}
