using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    #region parameters

    #endregion

    #region references
    [SerializeField]
    private Animator _myAnimator;
    #endregion

    #region methods
    public void DeathAni()
    {
        _myAnimator.ResetTrigger("NoCorrer");
        _myAnimator.SetTrigger("Death");
    }

    public void DamageAni()
    {
        _myAnimator.ResetTrigger("NoCorrer");
        _myAnimator.ResetTrigger("Correr");
        _myAnimator.SetTrigger("Damage");
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
