using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    #region references
    private Rigidbody2D _myRigidBody;
    #endregion
    #region methods
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.GetComponent<ProjectileMovement>()!=null)
        {
            
            Destroy(collision.gameObject);
            _myRigidBody.velocity = Vector3.zero;
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
