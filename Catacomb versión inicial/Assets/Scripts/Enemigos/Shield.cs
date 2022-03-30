using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties
    private bool _primerColor;
    private bool _segundoColor;
    #endregion

    #region references
    private EnemyLifeComponent _myShieldLifeComponent;
    // es un array porque tanto el escudo como el enemigo cuerpo a cuerpo tienen BoxCollider2D
    private BoxCollider2D[] _myEnemyBoxCollider2D;
    private EnemyMelee _myEnemyMelee;
    private Transform _myEnemyTransform;
    #endregion

    #region methods
    private void RemoveShield()
    {
        _myEnemyBoxCollider2D[1].enabled = true;    // se activa el box collider del enemigo cuerpo a cuerpo
        _myEnemyMelee.enabled = true;   // se activa el script de ataque del enemigo cuerpo a cuerpo
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
        _myEnemyBoxCollider2D = GetComponentsInParent<BoxCollider2D>();
        _myEnemyMelee = GetComponentInParent<EnemyMelee>();
        _primerColor = false;
        _segundoColor = false;
        _myEnemyTransform = _myEnemyMelee.gameObject.transform;
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
                    Debug.Log("primer color");
                    GameObject.Destroy(GetComponent<Red>());
                    gameObject.AddComponent<Yellow>();
                    _primerColor = true;
                }
                break;
            case 2:
                if (!_segundoColor)
                {
                    Debug.Log("segundo color");
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
