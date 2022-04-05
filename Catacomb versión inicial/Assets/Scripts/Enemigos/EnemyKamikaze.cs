using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikaze : MonoBehaviour
{
    #region parameters
    private Quaternion rotation;
    private Vector3 instPoint;
    #endregion

    #region properties
    private int _damage = 1;
    #endregion

    #region references
    Transform _myTransform;
    [SerializeField]
    private GameObject _enemyDetectionZone;
    private GameObject _enemyZone;
    private bool exploto = false;
    private EnemyDetectionZone _myEnemyDetectionZone;
    private Red _myRedComponent;
    private EnemyLifeComponent _myEnemyLifeComponent;
    #endregion

    #region methods
    public void Explosion()
    {
        exploto = true;
    }

    public int DañoAtaque()
    {
        return _damage;
    }
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        _myEnemyLifeComponent = GetComponent<EnemyLifeComponent>();
        _myTransform = transform;
        rotation = _myTransform.rotation;
        instPoint = transform.position;
        _enemyZone = Instantiate(_enemyDetectionZone, instPoint, rotation, _myTransform);
        if (_myRedComponent != null)
        {
            _damage += _myRedComponent.IncreasedDamage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (exploto)
        {
            GameManager.Instance.OnEnemyDies(_myEnemyLifeComponent);
            GameManager.Instance.EnemyDestroyed();
            GetComponent<EnemyMovement>().StopMovement();
            Destroy(gameObject);
        }
    }
}
