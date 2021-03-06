using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    #region references
    private Red _myRedComponent;
    private Transform _myTransform;
    #endregion

    #region parameters
    private float _fireRate;
    private float _elapsedTime = 0;
    private float range = 0;
    private float playerDistance = 0;
    [SerializeField]
    private GameObject ammo;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private int _maxrate = 6;
    [SerializeField]
    private int _minrate = 3;

    #endregion

    #region methods
    public int dañoAtaque()
    {
        return damage;
    }
    #endregion

    #region properties
    GameObject target;
    Transform targetTransform;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        target = GameObject.Find("Player");
        _myRedComponent = GetComponent<Red>();
        targetTransform = target.transform;
        Vector3 temp = (targetTransform.position - this.transform.position);
        if (_myRedComponent != null)
        {
            damage = _myRedComponent.IncreasedDamage();
        }

        _fireRate = GameManager.Instance.NumRandom(_minrate, _maxrate);
        //range = camera.pixelHeight/2;
        range = GetComponentInParent<EnemyMovement>().GetRange();
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = GetComponentInParent<EnemyMovement>().GetPlayerDistance();
        if (playerDistance <= range)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _fireRate)
            {
                _elapsedTime = 0;
                GameObject shotAmmo = Instantiate(ammo, this.transform.position, Quaternion.identity, _myTransform);
                shotAmmo.GetComponent<ProjectileMovement>().SetDamage(damage);
            }
        }
    }
}
