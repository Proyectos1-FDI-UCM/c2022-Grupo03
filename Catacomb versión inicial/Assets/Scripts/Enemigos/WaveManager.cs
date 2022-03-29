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
    [SerializeField]
    private Vector2 spawnAreaSize;
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
                Debug.Log("wave0");
                break;
            case 1:
                currentWave = wave1;
                Debug.Log("wave1");
                break;
            case 2:
                currentWave = wave2;
                Debug.Log("wave2");
                break;
            default:
                break;
        }
        for (int i = 0; i < currentWave.Length; i++)
        {
            //el tipo de enemigo que se spawnea depende de waveContent
            Instantiate(type[currentWave[i]], spawnPos[i], Quaternion.identity, _myTransform);
            GameManager.Instance.EnemySpawned();
            nEnemies++;
        }
    }
    #endregion
    void Start()
    {
        _myTransform = transform;
        int cont = 0;
        int maxCapacity = (int)(spawnAreaSize.x * spawnAreaSize.y);
        spawnPos = new Vector2[maxCapacity];
        for (int i = 0; i < spawnAreaSize.x; i++)
        {
            for (int j = 0; j < spawnAreaSize.y; j++)
            {
                spawnPos[cont] = new Vector2(transform.position.x + i, transform.position.y + j);
                cont++;
            }
        }
        Invoke("Spawn", 3.0f);
    }

    void Update()
    {
        if (GameManager.Instance.outOfTime() || nEnemies < 1)
        {
            waveNumber++;
            Spawn();
        }
    }
}
