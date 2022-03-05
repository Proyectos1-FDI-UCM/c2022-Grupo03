using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMelee : MonoBehaviour
{

    #region parameters
    private float diferenciax;
    //private float diferenciay;
    private bool ataca = false;
    private float _elapsedTime;
    [SerializeField]
    private float _attackDuration = 2f;
    [SerializeField]
    private float _duration = 1f;
    private Vector3 offset = new Vector3(1f,0f,0f);
    private float x_scale, y_scale, z_scale;
    [SerializeField]
    private int _damage;
    #endregion

    #region properties
    private int _dañoTotal;
    #endregion

    #region references
    Transform _myTransform;
    GameObject player;
    [SerializeField]
    private GameObject _enemyAttackZone;
    private GameObject _enemyAttack;
    private Red _myRedComponent;
    #endregion

    #region methods
    public void meleeAttack()
    {
        if (ataca)
        {
            Quaternion rotation = _myTransform.rotation;
            
            Vector3 instPoint = transform.position + offset;
            _enemyAttack = Instantiate(_enemyAttackZone, instPoint, rotation);
        }
    }
    public bool atacando()
    {
        return ataca;
    }
    public int dañoAtaque()
    {
        return _dañoTotal;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        x_scale = _myTransform.localScale.x;
        y_scale = _myTransform.localScale.y;
        z_scale = _myTransform.localScale.z;
        player = GameObject.Find("Player");
        _myRedComponent = GetComponent<Red>();
        if (_myRedComponent != null)
        {
            _dañoTotal += _myRedComponent.IncreasedDamage();
        }
        _dañoTotal = _damage;
    }

    // Update is called once per frame
    void Update()
    {
        diferenciax = Math.Abs(transform.position.x) - Math.Abs(player.transform.position.x);
        if (diferenciax < 0 && transform.position.x < 0 && player.transform.position.x < 0)
        {
            _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
            offset = new Vector3(-1f, 0f, 0f);
        }
        else if (diferenciax > 0 && transform.position.x < 0 && player.transform.position.x < 0)
        {
            _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
            offset = new Vector3(1f, 0f, 0f);
        }
        else if (diferenciax < 0 && transform.position.x > 0 && player.transform.position.x > 0)
        {
            _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
            offset = new Vector3(1f, 0f, 0f);
        }
        else if (diferenciax > 0 && transform.position.x > 0 && player.transform.position.x > 0)
        {
            _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
            offset = new Vector3(-1f, 0f, 0f);
        }

        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _attackDuration)
        {
            ataca = true;
            meleeAttack();
            _elapsedTime = 0;
        }
        else if (_elapsedTime > _duration)
        {
            GameObject.Destroy(_enemyAttack);
            ataca = false;
        }        
    }
}
