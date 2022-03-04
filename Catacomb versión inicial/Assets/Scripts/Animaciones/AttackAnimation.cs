using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    #region references
    [SerializeField]
    private Animator _myAnimator;
    private Transform _myTransform;
    private PlayerChangeColors _myChangeColors;
    #endregion

    #region properties
    private string[] _enemyColors = { "Red", "Yellow", "Green", "Blue", "Pink" };
    #endregion

    #region methods
    public void AttackAni()
    {
        int color = _myChangeColors.GetCurrentColorIndex();

        if(color == 0)
        {
            _myAnimator.ResetTrigger("NoCorrer");
            _myAnimator.ResetTrigger("Correr");
            _myAnimator.SetTrigger("RedAttack");
        }
        else if(color == 1)
        {
            _myAnimator.ResetTrigger("NoCorrer");
            _myAnimator.ResetTrigger("Correr");
            _myAnimator.SetTrigger("YellowAttack");
        }
        else if(color == 2)
        {
            _myAnimator.ResetTrigger("NoCorrer");
            _myAnimator.ResetTrigger("Correr");
            _myAnimator.SetTrigger("GreenAttack");
        }
        else if(color == 3)
        {
            _myAnimator.ResetTrigger("NoCorrer");
            _myAnimator.ResetTrigger("Correr");
            _myAnimator.SetTrigger("BlueAttack");
        }
        else
        {
            _myAnimator.ResetTrigger("NoCorrer");
            _myAnimator.ResetTrigger("Correr");
            _myAnimator.SetTrigger("PinkAttack");
        }

    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myChangeColors = GetComponent<PlayerChangeColors>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
