using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private int _tankEnemyLife;
    #endregion

    #region properties
    private bool _primerCol;
    private bool _segundoCol;
    #endregion

    #region references
    private EnemyLifeComponent _myShieldLifeComponent;
    private GameObject _myEnemy;
    private Transform _myEnemyTransform;
    private EnemyLifeComponent _myEnemyLifeComponent;
    #endregion

    #region methods
    private void RemoveShield()
    {
        // el enemigo cuerpo a cuerpo pasa a tener vida y, por lo tanto, puede recibir daño
        _myEnemyLifeComponent = _myEnemy.AddComponent<EnemyLifeComponent>();
        _myEnemyLifeComponent.MaxLife = _tankEnemyLife;
        // se rota al enemigo xq cuando se le destruye el escudo rota solo
        float negXScale = -_myEnemyTransform.localScale.x;
        _myEnemyTransform.localScale = new Vector3(negXScale, 1, 1);
        // se destruye el escudo
        GameObject.Destroy(gameObject);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myShieldLifeComponent = GetComponent<EnemyLifeComponent>();
        _myEnemy = transform.parent.gameObject;
        _myEnemyTransform = transform.parent;
        _primerCol = false;
        _segundoCol = false;
    }

    // Update is called once per frame
    void Update()
    {
        int life = _myShieldLifeComponent.CurrentLife;
        Debug.Log(life);
        if (!_primerCol)
        {
            if (life == 3)
            {
                GameObject.Destroy(GetComponent<Red>());
                gameObject.AddComponent<Yellow>();
                _primerCol = true;
            }
        }
        if (!_segundoCol)
        {
            if (life == 2)
            {
                GameObject.Destroy(GetComponent<Yellow>());
                gameObject.AddComponent<Green>();
                _segundoCol = true;
            }
        }
        if (life <= 1)
        {
            RemoveShield();
        }
    }
}
