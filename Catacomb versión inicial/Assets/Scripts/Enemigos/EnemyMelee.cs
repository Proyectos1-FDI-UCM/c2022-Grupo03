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
    private bool yellow = false; //booleano para ver si es amarillo
    private float _elapsedTime;
    private float _elapsedTime2 = 0;
    [SerializeField]
    private float _attackDuration = 2f;
    [SerializeField]
    private float _duration = 1f;
    private Vector3 offset = new Vector3(1f,0f,0f);
    private float x_scale, y_scale, z_scale;
    [SerializeField]
    private int _damage = 1;
    [SerializeField]
    private int _minrate = 2;
    [SerializeField]
    private int _maxrate = 4;
    #endregion

    #region properties
    private int _da�oTotal;
    private int _attackRate;
    private float range = 0;
    private float playerDistance = 0;
    #endregion

    #region references
    Transform _myTransform;
    GameObject player;
    [SerializeField]
    private GameObject _enemyAttackZone;
    [SerializeField]
    private GameObject _enemyAttackZoneYellow;

    private GameObject _enemyAttack;
    private Red _myRedComponent;
    private Yellow _myYellowComponent;
    private SoundEnemysManager _soundEnemysManager;
    #endregion

    #region methods
    public void meleeAttack()
    {
        if (ataca)
        {
            Quaternion rotation = _myTransform.rotation;
            
            Vector3 instPoint = transform.position + offset;
            if(!yellow)
            {
                _enemyAttack = Instantiate(_enemyAttackZone, instPoint, rotation, _myTransform);
            }
            else
            {
                _enemyAttack = Instantiate(_enemyAttackZoneYellow, instPoint, rotation, _myTransform);
            }        
        }
    }
    public bool atacando()
    {
        return ataca;
    }
    public int Da�oAtaque()
    {
        return _da�oTotal;
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
        _myYellowComponent = GetComponent<Yellow>();
        if (_myRedComponent != null)
        {
            _da�oTotal += _myRedComponent.IncreasedDamage();
        }
        if(_myYellowComponent!=null)
        {
            yellow = true;
        }
        _da�oTotal = _damage;

        _attackRate = GameManager.Instance.NumRandom(_minrate, _maxrate);
        range = GetComponent<EnemyMovement>().GetRange();
        _soundEnemysManager = GetComponent<SoundEnemysManager>();
    }

    // Update is called once per frame
    void Update()
    {

        _elapsedTime2 += Time.deltaTime;
        //para ver hacia donde mira el enemigo y hacia que lado se instancia la zona de ataque
        if (_elapsedTime2 > 0.2f)
        {
            _elapsedTime2 = 0;
            diferenciax = Math.Abs(transform.position.x) - Math.Abs(player.transform.position.x);
            if (diferenciax < 0 && transform.position.x < 0 && player.transform.position.x < 0)
            {
                _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
                offset = new Vector3(-1f, 0f, 0f);
            }
            else if (diferenciax > 0 && transform.position.x < 0 && player.transform.position.x < 0)
            {
                _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
                offset = new Vector3(1f, 0f, 0f);
            }
            else if (diferenciax < 0 && transform.position.x > 0 && player.transform.position.x > 0)
            {
                _myTransform.localScale = new Vector3(x_scale, y_scale, z_scale);
                offset = new Vector3(1f, 0f, 0f);
            }
            else if (diferenciax > 0 && transform.position.x > 0 && player.transform.position.x > 0)
            {
                _myTransform.localScale = new Vector3(-x_scale, y_scale, z_scale);
                offset = new Vector3(-1f, 0f, 0f);
            }          
        }
        
        playerDistance = GetComponent<EnemyMovement>().GetPlayerDistance();
        if (playerDistance <= range)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _attackDuration)
            {
                ataca = true;
                meleeAttack();
                _elapsedTime = 0;
                _soundEnemysManager.EligeAudioE(0,0.25f);
            }
        }
        else if (_elapsedTime > _duration)
        {
            //cuando termina de atacar destruye la zona de ataque
            GameObject.Destroy(_enemyAttack);
            ataca = false;
        }        
    }
}
