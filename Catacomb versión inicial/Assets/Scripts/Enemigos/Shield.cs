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
    private bool _primerColor;
    private bool _segundoColor;
    #endregion

    #region references
    private EnemyLifeComponent _myShieldLifeComponent;
    private GameObject _myEnemy;
    private Transform _myEnemyTransform;
    //private EnemyMelee _myEnemyMelee;
    private EnemyLifeComponent _myEnemyLifeComponent;
    #endregion

    #region methods
    private void RemoveShield()
    {
        // el enemigo cuerpo a cuerpo pasa a tener vida y, por lo tanto, puede recibir daño
        _myEnemyLifeComponent = _myEnemy.AddComponent<EnemyLifeComponent>();
        _myEnemyLifeComponent.MaxLife = _tankEnemyLife;
        // se activa el script del ataque del enemigo cuerpo a cuerpo
        //_myEnemyMelee.enabled = true;
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
        //_myEnemyMelee = _myEnemy.GetComponent<EnemyMelee>();
        _primerColor = false;
        _segundoColor = false;
    }

    // Update is called once per frame
    void Update()
    {
        int life = _myShieldLifeComponent.CurrentLife;
        switch (life)
        {
            case 3:
                if (!_primerColor)
                {
                    GameObject.Destroy(GetComponent<Red>());
                    gameObject.AddComponent<Yellow>();
                    _primerColor = true;
                }
                break;
            case 2:
                if (!_segundoColor)
                {
                    GameObject.Destroy(GetComponent<Yellow>());
                    gameObject.AddComponent<Green>();
                    _segundoColor = true;
                }
                break;
            case 1:
                // hay un pequeño error y es que si el enemigo es del mismo color
                // que el último color del escudo cuando se destruye el escudo
                // el enemigo también sufre daño
                RemoveShield();
                break;
        }
    }
}
