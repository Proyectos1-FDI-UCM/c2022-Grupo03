using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _projectileSpeed = 5.0f;
    private float lifespan = 8.0f;
    private float _elapsedTime = 0;
    #endregion
    #region references
    private GameObject target;
    private Transform _myTransform;
    Rigidbody2D hitbox;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        if (target != null)
        {
            _myTransform = transform;
            Vector3 temp = (target.transform.position - _myTransform.position).normalized;
            _myTransform.up = temp;
            hitbox = GetComponent<Rigidbody2D>();
            hitbox.velocity = (_myTransform.up * _projectileSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > lifespan)
        {
            Destroy(this.gameObject);
        }
    }
}
