using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColor : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties
    Color[] _colors = { Color.red, Color.yellow, Color.green, Color.blue, Color.magenta};
    #endregion

    #region references
    private SpriteRenderer _enemyRenderer;
    #endregion

    #region methods

    #endregion

    void Start()
    {
        int randomColor = GameManager.Instance.NumRandom(0, 4);
        switch (randomColor)
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
        _enemyRenderer = gameObject.GetComponent<SpriteRenderer>();
        _enemyRenderer.color = _colors[randomColor];
    }

    // Update is called once per frame
    void Update()
    {

    }
}