using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInitializer : MonoBehaviour
{
    #region references
    private SpriteRenderer _enemyRenderer;
    #endregion

    #region properties
    private int _randomColor;
    Color[] colourArray = { Color.red, Color.yellow, Color.green, Color.magenta, Color.blue };
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _enemyRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        _randomColor = GameManager.Instance.NumRandom(0, 4);
        switch (_randomColor)
        {
            case 0:
                gameObject.AddComponent<Red>();
                break;
            case 1:
                gameObject.AddComponent<Yellow>();
                break;
            case 2:
                gameObject.AddComponent<Green>();
                break;
            case 3:
                gameObject.AddComponent<Pink>();
                break;
            case 4:
                gameObject.AddComponent<Blue>();
                break;
        }
        _enemyRenderer.color = colourArray[_randomColor];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
