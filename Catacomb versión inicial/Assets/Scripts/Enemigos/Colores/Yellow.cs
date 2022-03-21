using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yellow : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _increasedRange;
    #endregion

    #region properties

    #endregion

    #region references
    private SpriteRenderer _mySpriteRenderer;
    #endregion

    #region methods
    public int IncreasedRange()
    {
        return _increasedRange;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _mySpriteRenderer.color=GameManager.Instance.Colors[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
