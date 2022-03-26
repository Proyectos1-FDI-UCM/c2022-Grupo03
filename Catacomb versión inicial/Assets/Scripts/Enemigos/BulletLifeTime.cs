using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifeTime : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _duration = 2f;
    #endregion

    #region properties

    #endregion

    #region references

    #endregion

    #region methods
    // las balas se destruyen cuando pasa cierto tiempo
    private void DestroyBullet()
    {
        GameObject.Destroy(gameObject);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestroyBullet), _duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
