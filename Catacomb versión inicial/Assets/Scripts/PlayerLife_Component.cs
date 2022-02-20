using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife_Component : MonoBehaviour
{
    #region references

    #endregion

    #region parameters
    private int _playerLife;
    private int _maxLife = 5; //esto luego se puede parametrizar diferente
    #endregion

    #region methods

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _playerLife = _maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
