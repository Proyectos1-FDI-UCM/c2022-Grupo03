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
    public bool reloading = false;
    [SerializeField]
    private GameObject ammo;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private int _maxrate = 3;
    [SerializeField]
    private int _minrate = 6;

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
    int tiempodif;
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
            damage += _myRedComponent.IncreasedDamage();
        }

        _fireRate = GameManager.Instance.NumRandom(_minrate, _maxrate);
    }

    // Update is called once per frame
    void Update()
    {

        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _fireRate)
        {
            _elapsedTime = 0;
            reloading = false;
        }
        if (!reloading)
        {
            //se instancia como hijo
            GameObject shotAmmo = Instantiate(ammo, this.transform.position, Quaternion.identity, _myTransform);
            shotAmmo.GetComponent<ProjectileMovement>().SetDamage(damage);
            reloading = true;
        }
    }
}
