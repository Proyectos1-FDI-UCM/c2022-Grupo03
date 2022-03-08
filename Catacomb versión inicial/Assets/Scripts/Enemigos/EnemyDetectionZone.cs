using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionZone : MonoBehaviour
{

    #region references
    Transform _myTransform;
    [SerializeField]
    private GameObject _enemyAttackZone;
    private GameObject _enemyExplosion;
    private EnemyKamikaze _enemyKamikaze;
    private bool explota = false;
    private float _elapsedTime;
    private float _tiempoexplosion = 3f;
    private EnemyLifeComponent _enemyLifeComponent;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // duck typing
        Quaternion rotation = _myTransform.rotation;

        Vector3 instPoint = transform.position;

        PlayerLifeComponent _playerLifeComponent = collider.GetComponent<PlayerLifeComponent>();
        if (_playerLifeComponent != null)
        {
            _enemyExplosion = Instantiate(_enemyAttackZone, instPoint, rotation, _myTransform);
            explota = true;   
        }

    }
    public bool HaExplotado()
    {
        return explota;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _enemyKamikaze = GetComponent<EnemyKamikaze>();
        _enemyKamikaze.Explosion();
    }

    // Update is called once per frame
    void Update()
    {
        if (explota)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _tiempoexplosion)
            {
                GameObject.Destroy(gameObject);              
            }            
        }       
    }
}
