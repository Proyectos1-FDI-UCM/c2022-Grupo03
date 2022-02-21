using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    #region references
    private PlayerInputManager _myPlayerInput;
    [SerializeField]
    private Animator _myAnimator; 
    #endregion

    #region methods
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myPlayerInput = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
       if(_myPlayerInput.HInput() == 0 && _myPlayerInput.VInput() == 0)
       {
            _myAnimator.ResetTrigger("Correr");
            _myAnimator.SetTrigger("NoCorrer");
       } 
       else
       {
            _myAnimator.ResetTrigger("NoCorrer");
            _myAnimator.SetTrigger("Correr");
            
            /*if (_myPlayerInput.HInput() < 0)
                transform.Rotate(0.0f, 180.0f, 0.0f);
            else
                transform.Rotate(0.0f, 0.0f, 0.0f);*/
        }
    }
}
