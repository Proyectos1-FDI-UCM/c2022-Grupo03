using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pink : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _increasedLife = 2;
    #endregion

    #region properties

    #endregion

    #region references
    private SpriteRenderer _mySpriteRenderer;
    #endregion

    #region methods
    public int IncreasedLife()
    {
        return _increasedLife;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _mySpriteRenderer.color = GameManager.Instance.Colors[4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
