using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeRecAnimation : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private Animator _myAnimator;
    #endregion

    #region references
    private PlayerLifeComponent _myPlayerLife;
    #endregion

    #region methods
    public void HealAni()
    {
        if (_myPlayerLife.Heal())
        {
            _myAnimator.ResetTrigger("Null");
            _myAnimator.SetTrigger("Life");
        }
        else
        {
            _myAnimator.ResetTrigger("Life");
            _myAnimator.SetTrigger("Null");
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myPlayerLife = GetComponent<PlayerLifeComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
