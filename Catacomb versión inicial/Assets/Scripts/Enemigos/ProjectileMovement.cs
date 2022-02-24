using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField]
    private float _projectileSpeed = 5.0f;
    private GameObject target;
    private Transform _myTransform;
    Rigidbody2D hitbox;
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
}
