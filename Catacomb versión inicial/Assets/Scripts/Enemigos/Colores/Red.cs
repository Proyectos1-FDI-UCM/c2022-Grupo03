using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _increasedDamage = 1;
    #endregion

    #region properties
    #endregion

    #region references
    
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
