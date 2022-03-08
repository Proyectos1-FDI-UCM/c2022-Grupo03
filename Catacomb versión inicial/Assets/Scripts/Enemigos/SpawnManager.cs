using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region references
    [SerializeField]
    private GameObject _enemyMelee;
    [SerializeField]
    private GameObject _enemyRanged;
    [SerializeField]
    private GameObject _enemyKamikaze;
    #endregion

    #region properties
    private int _randomEnemy;
    private float _elapsedTime;
    #endregion

    #region parameters
    #endregion

    #region methods
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > 5) //si el tiempo supera 5 segundos, se instancia un tipo de enemigo aleatorio entre los dos posibles
        {
            _randomEnemy = Random.Range(0, 3);
            if (_randomEnemy == 0) //instancia un enemigo a Melee
            {
                float _rposy = Random.Range(-4f, 4f);
                float _rposx = Random.Range(-10f, 10f);
                Instantiate(_enemyMelee, new Vector3(_rposx, _rposy, 0), Quaternion.identity);
            }
            else if (_randomEnemy==1) //instancia un enemigo shooter
            {
                float _rposy = Random.Range(-4f, 4f);
                float _rposx = Random.Range(-10f, 10f);
                Instantiate(_enemyRanged, new Vector3(_rposx, _rposy, 0), Quaternion.identity);
            }
            else //instancia un enemigo kamikaze
            {
                float _rposy = Random.Range(-4f, 4f);
                float _rposx = Random.Range(-10f, 10f);
                Instantiate(_enemyKamikaze, new Vector3(_rposx, _rposy, 0), Quaternion.identity);
            }

            _elapsedTime = 0;
        }
        
    }
}
