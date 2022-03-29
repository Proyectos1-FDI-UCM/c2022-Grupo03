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
    private BoxCollider2D[] _myEnemyBoxCollider2D;
    private EnemyMelee _myEnemyMelee;
    private Transform _myEnemyTransform;
    #endregion

    #region methods

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
                _myEnemyBoxCollider2D[1].enabled = true;
                _myEnemyMelee.enabled = true;
                float negXScale = -_myEnemyTransform.localScale.x;
                _myEnemyTransform.localScale = new Vector3(negXScale, 1, 1);
                GameObject.Destroy(gameObject);
                break;
        }
    }
}
