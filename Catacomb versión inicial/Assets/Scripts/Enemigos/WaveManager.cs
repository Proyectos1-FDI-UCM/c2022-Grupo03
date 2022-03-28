using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    #region properties
    [SerializeField]
    private int waveNumber = 0;
    [SerializeField]
    private int nEnemies = 3;
    [SerializeField]
    private GameObject[] type;

    [SerializeField]
    private int[] wave0;
    [SerializeField]
    private int[] wave1;
    [SerializeField]
    private int[] wave2;
    [SerializeField]
    private int[] wave3;
    private Vector2[] spawnPos;
    private float offset = 1;
    #endregion
    #region references
    private Transform _myTransform;
    #endregion
    #region methods
    private void Spawn()
    {
        int[] currentWave = wave0;

        switch (waveNumber)
        {
            case 0:
                currentWave = wave0;
                break;
            case 1:
                currentWave = wave1;
                break;
            case 2:
                currentWave = wave2;
                break;
            default:
                break;
        }
        for(int i = 0; i < currentWave.Length; i++)
        {
            //el tipo de enemigo que se spawnea depende de waveContent
            Instantiate(type[currentWave[i]], this.transform.position, Quaternion.identity, _myTransform);
        }
    }
    #endregion
    void Start()
    {
        Spawn();
        _myTransform = transform;
    }

    void Update()
    {
        if (GameManager.Instance.WaveOver())
        {
            waveNumber++;
            Spawn();
        }
    }
}
