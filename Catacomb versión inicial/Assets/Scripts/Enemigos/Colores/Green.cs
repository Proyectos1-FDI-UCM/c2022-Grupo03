using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _increasedSpeed = 1;
    #endregion

    #region properties

    #endregion

    #region references
    private SpriteRenderer _mySpriteRenderer;
    #endregion

    #region methods
    public int IncreasedSpeed()
    {
        return _increasedSpeed;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _mySpriteRenderer.color=GameManager.Instance.Colors[2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
