using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeRecAnimation : MonoBehaviour
{
    #region properties
    float time = 0;
    #endregion

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
        _myAnimator.ResetTrigger("Null");
        _myAnimator.SetTrigger("Life");
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
        time += Time.deltaTime;

        if (time > 1.5)
        {
            _myAnimator.ResetTrigger("Life");
            _myAnimator.SetTrigger("Null");
            time = 0;
        }       
    }
}
