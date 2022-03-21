using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : MonoBehaviour
{
    #region parameteres
    [SerializeField]
    private int _recoveryPoints = 1;
    #endregion

    #region properties

    #endregion

    #region references
    private SpriteRenderer _mySpriteRenderer;
    #endregion

    #region methods
    public int RecoveryPoints()
    {
        return _recoveryPoints;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _mySpriteRenderer.color=GameManager.Instance.Colors[3];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
