using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikaze : MonoBehaviour
{
    #region parameters
    private Quaternion rotation;
    private Vector3 instPoint;
    #endregion

    #region references
    Transform _myTransform;
    [SerializeField]
    private GameObject _enemyDetectionZone;
    private GameObject _enemyZone;
    private bool exploto = false;
    private EnemyDetectionZone _myEnemyDetectionZone;
    #endregion

    #region methods
    public void Explosion()
    {
        exploto = true;
    }
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        rotation = _myTransform.rotation;
        instPoint = transform.position;
        _enemyZone = Instantiate(_enemyDetectionZone, instPoint, rotation, _myTransform);

    }

    // Update is called once per frame
    void Update()
    {
        if (exploto)
        {
            Destroy(gameObject);
        }
    }
}
