using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _increasedDamage = 2;
    #endregion

    #region properties
    #endregion

    #region references
    private SpriteRenderer _mySpriteRenderer;
    #endregion

    #region methods
    public int IncreasedDamage()
    {
        return _increasedDamage;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _mySpriteRenderer.color = GameManager.Instance.Colors[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
