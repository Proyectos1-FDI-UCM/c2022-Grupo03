using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _fireRate = 3.0f;
    private float _elapsedTime = 0;
    public bool reloading = false;
    private Quaternion playerDir;
    [SerializeField]
    private GameObject ammo;
    #endregion

    #region methods

    #endregion

    #region properties
    GameObject target;
    Transform targetTransform;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        targetTransform = target.transform;
        Vector3 temp = (targetTransform.position - this.transform.position);
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
            GameObject shotAmmo = Instantiate(ammo, this.transform.position, Quaternion.identity);
            reloading = true;
        }
    }
}
