using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    #region parameters
    private float x_scale, y_scale, z_scale;
    #endregion

    #region references
    private PlayerInputManager _myPlayerInput;
    [SerializeField]
    private Animator _myAnimator;
    private Transform _myTransform;
    private AttackAnimation _myAttackAnimation;
    private PlayerAttackController _myPlayerAttackController;
    #endregion

    #region methods
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myPlayerInput = GetComponent<PlayerInputManager>();
        _myAttackAnimation = GetComponent<AttackAnimation>();
        _myPlayerAttackController = GetComponent<PlayerAttackController>();
        _myTransform = transform;
        x_scale = _myTransform.localScale.x;
        y_scale = _myTransform.localScale.y;
        z_scale = _myTransform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_myAttackAnimation.Ataca() && !_myPlayerAttackController.AtacaRayo())
        {
            if (_myPlayerInput.HInput() == 0 && _myPlayerInput.VInput() == 0)
            {
                _myAnimator.ResetTrigger("Correr");
                _myAnimator.ResetTrigger("Roll");
                _myAnimator.SetTrigger("NoCorrer");
            }
            else if (_myPlayerInput.HInput() < 0) // izq
            {
                _myAnimator.ResetTrigger("NoCorrer");
                _myAnimator.ResetTrigger("Roll");
                _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("Correr");
            }
            else // der
            {
                _myAnimator.ResetTrigger("NoCorrer");
                _myAnimator.ResetTrigger("Roll");
                _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("Correr");
            }

            if (Input.GetButtonDown("Roll"))
            {
                _myAnimator.ResetTrigger("Correr");
                _myAnimator.ResetTrigger("NoCorrer");
                _myAnimator.SetTrigger("Roll");
            }
        }
        
    }
}
