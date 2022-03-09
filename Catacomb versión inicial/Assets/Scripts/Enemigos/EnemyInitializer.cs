using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyInitializer : MonoBehaviour
{
    #region references
    private SpriteRenderer _enemyRenderer;
    #endregion
    #region properties
    private int _randomEnemy;
    Color[] colourArray = { Color.red, Color.yellow, Color.green, Color.magenta, Color.blue };
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _enemyRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        _randomEnemy = Random.Range(0, 5);
        if (_randomEnemy == 0)
        {
            this.gameObject.AddComponent<Red>();
        }
        else if (_randomEnemy == 1)
        {
            this.gameObject.AddComponent<Yellow>();
        }
        else if (_randomEnemy == 2)
        {
            this.gameObject.AddComponent<Green>();
        }
        else if (_randomEnemy == 3)
        {
            this.gameObject.AddComponent<Pink>();
        }
        else if (_randomEnemy == 4)
        {
            this.gameObject.AddComponent<Blue>();
        }
        _enemyRenderer.color = colourArray[_randomEnemy];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
