using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderColor : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties
    #endregion

    #region references
    #endregion

    #region methods
    private void Awake()
    {
        int randomColor = GameManager.Instance.NumRandom(0, 3);
        switch (randomColor)
        {
            case 0:
                gameObject.AddComponent<Red>();
                break;
            case 1:
                gameObject.AddComponent<Yellow>();
                break;
            case 2:
                gameObject.AddComponent<Green>();
                break;
            case 3:
                gameObject.AddComponent<Pink>();
                break;
        }
    }
    #endregion

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}