using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int spikesDamage = 1;
    [SerializeField]
    private float _durationDown;
    [SerializeField]
    private float _durationUp;
    #endregion

    #region properties
    private int _state;
    private float _elapsedTime;
    #endregion

    #region references
    private Collider2D _myCollider2D;
    private Animator _mySpikeAnimator;
    #endregion

    #region methods
    private void InitializateStateZero()
    {
        _state = 0;
        _myCollider2D.enabled = false;
    }

    private void BoxCollision()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerLifeComponent playerLifeComponent = collider.GetComponent<PlayerLifeComponent>();
        EnemyLifeComponent enemyLifeComponent = collider.GetComponent<EnemyLifeComponent>();
        if (playerLifeComponent != null) playerLifeComponent.Damage(spikesDamage);
        if (enemyLifeComponent != null) enemyLifeComponent.Damage(spikesDamage);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myCollider2D = GetComponent<Collider2D>();
        _mySpikeAnimator = GetComponent<Animator>();
        InitializateStateZero();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case 0:
                _elapsedTime += Time.deltaTime;
                // transición del estado bajo al alto
                if (_elapsedTime > _durationDown)
                {
                    _mySpikeAnimator.ResetTrigger("Down");
                    _mySpikeAnimator.SetTrigger("Up");
                    _elapsedTime = 0;
                    _state = 1;
                    _myCollider2D.enabled = true;                  
                }
                break;

            case 1:
                _elapsedTime += Time.deltaTime;
                // transición del estado alto al bajo
                if (_elapsedTime > _durationUp)
                {
                    _mySpikeAnimator.ResetTrigger("Up");
                    _mySpikeAnimator.SetTrigger("Down");
                    _elapsedTime = 0;
                    InitializateStateZero();                   
                }
                break;
        }
    }
}
