using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    #region parameters
    [SerializeField]
    int _numStates = 3;
    #endregion

    #region properties
    int _state;
    private int[] _numCols;
    #endregion

    #region references
    private SpriteRenderer _mySpriteRenderer;
    Object _activeCol;
    #endregion

    #region methods
    private void Desordenar(int[] v)
    {
        int n = v.Length;
        for (int i = 0; i < n; i++)
        {
            int j = GameManager.Instance.NumRandom(i, n - 1);
            int aux = v[i];
            v[i] = v[j];
            v[j] = aux;
        }
    }

    private void AddCol(int numCol)
    {
        switch (numCol)
        {
            case 0:
                _activeCol = gameObject.AddComponent<Red>();
                break;
            case 1:
                _activeCol = gameObject.AddComponent<Yellow>();
                break;
            case 2:
                _activeCol = gameObject.AddComponent<Green>();
                break;
            case 3:
                _activeCol = gameObject.AddComponent<Blue>();
                break;
            case 4:
                _activeCol = gameObject.AddComponent<Pink>();
                break;
        }
    }

    public void ChangeShieldCol()
    {
        _state++;
        if (_state >= _numStates)
        {
            RemoveShield();
        }
        else
        {
            GameObject.Destroy(_activeCol);
            ApplyCol(_state);
        }
    }

    private void ApplyCol(int state = 0)
    {
        _state = state;
        AddCol(_numCols[_state]);
    }

    private void RemoveShield()
    {
        GameObject.Destroy(gameObject);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _numCols = new int[3];
        for (int i = 0; i < _numCols.Length; i++)
        {
            _numCols[i] = i;
        }
        Desordenar(_numCols);

        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        ApplyCol();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
