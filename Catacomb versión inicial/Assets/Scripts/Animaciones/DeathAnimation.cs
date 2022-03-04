using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimation : MonoBehaviour
{
    #region parameters
    private bool muriendo = false;
    private float _elapsedTime;
    [SerializeField]
    private int tiempoMuerte = 1000;
    #endregion

    #region methods
    public void DeathAni()
    {
        _myAnimator.ResetTrigger("NoCorrer");
        _myAnimator.SetTrigger("Death");
        muriendo = true;
        
    }
    #endregion

    #region references
    [SerializeField]
    private Animator _myAnimator;
    private Transform _myTransform;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (!muriendo)
            _elapsedTime = 0;
        if(muriendo && _elapsedTime > tiempoMuerte)
            GameManager.Instance.Death();
    }
}
