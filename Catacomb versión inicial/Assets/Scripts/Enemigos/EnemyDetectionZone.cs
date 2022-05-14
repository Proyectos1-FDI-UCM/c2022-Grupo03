using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionZone : MonoBehaviour
{

    #region references
    Transform _myTransform;
    [SerializeField]
    private GameObject _enemyAttackZone;
    [SerializeField]
    private GameObject _enemyAttackZoneYellow;
    private GameObject _enemyExplosion;
    private EnemyKamikaze _enemyKamikaze;
    private bool explota = false;
    private float _elapsedTime = 0;
    private float _tiempoexplosion = 0.4f;
    #endregion

    #region properties

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
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _elapsedTime = 0;
        _myTransform = transform;
        _enemyKamikaze = GetComponentInParent<EnemyKamikaze>();
    }

    // Update is called once per frame
    void Update()
    {
        if (explota)
        {           
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _tiempoexplosion)
            {
                _enemyKamikaze.Explosion();
                _elapsedTime = 0;
                Destroy(gameObject);
            }            
        }       
    }
}
