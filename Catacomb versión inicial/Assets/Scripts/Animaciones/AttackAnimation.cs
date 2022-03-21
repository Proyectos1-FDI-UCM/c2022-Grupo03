using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private Animator _myAnimator;
    private float x_scale, y_scale, z_scale;
    #endregion

    #region properties
    private string[] _enemyColors = { "Red", "Yellow", "Green", "Blue", "Pink" };
    #endregion

    #region references
    private Transform _myTransform;
    private PlayerChangeColors _myChangeColors;
    #endregion

    #region methods
    public void AttackAni(int n)
    {
        int color = _myChangeColors.GetCurrentColorIndex();

        if(color == 0)
        {
            _myAnimator.ResetTrigger("NoCorrer");
            _myAnimator.ResetTrigger("Correr");
            if (n == 0) //arriba
                _myAnimator.SetTrigger("UpAttackRed");
            else if (n == 1)
            { //izquierda
                _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("RedAttack");
            }
            else if (n == 2) //abajo
                _myAnimator.SetTrigger("DownAttackRed");
            else if (n == 3) //derecha
            {
                _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("RedAttack");
            }          
        }
        else if(color == 1)
        {
            _myAnimator.ResetTrigger("NoCorrer");
            _myAnimator.ResetTrigger("Correr");
            if (n == 0) //arriba
                _myAnimator.SetTrigger("UpAttackYellow");
            else if (n == 1)
            { //izquierda
                _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("YellowAttack");
            }
            else if (n == 2) //abajo
                _myAnimator.SetTrigger("DownAttackYellow");
            else if (n == 3) //derecha
            {
                _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("YellowAttack");
            }
            
        }
        else if(color == 2)
        {
            _myAnimator.ResetTrigger("NoCorrer");
            _myAnimator.ResetTrigger("Correr");
            if (n == 0) //arriba
                _myAnimator.SetTrigger("UpAttackGreen");
            else if (n == 1)
            { //izquierda
                _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("GreenAttack");
            }
            else if (n == 2) //abajo
                _myAnimator.SetTrigger("DownAttackGreen");
            else if (n == 3) //derecha
            {
                _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("GreenAttack");
            }
        }
        else if(color == 3)
        {
            _myAnimator.ResetTrigger("NoCorrer");
            _myAnimator.ResetTrigger("Correr");
            if (n == 0) //arriba
                _myAnimator.SetTrigger("UpAttackBlue");
            else if (n == 1)
            { //izquierda
                _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("BlueAttack");
            }
            else if (n == 2) //abajo
                _myAnimator.SetTrigger("DownAttackBlue");
            else if (n == 3) //derecha
            {
                _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("BlueAttack");
            }
        }
        else
        {
            _myAnimator.ResetTrigger("NoCorrer");
            _myAnimator.ResetTrigger("Correr");
            if (n == 0) //arriba
                _myAnimator.SetTrigger("UpAttackPink");
            else if (n == 1)
            { //izquierda
                _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("PinkAttack");
            }
            else if (n == 2) //abajo
                _myAnimator.SetTrigger("DownAttackPink");
            else if (n == 3) //derecha
            {
                _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
                _myAnimator.SetTrigger("PinkAttack");
            }       
        }

    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myChangeColors = GetComponent<PlayerChangeColors>();
        _myTransform = transform;
        x_scale = _myTransform.localScale.x;
        y_scale = _myTransform.localScale.y;
        z_scale = _myTransform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
