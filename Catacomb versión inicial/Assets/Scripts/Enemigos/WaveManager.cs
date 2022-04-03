using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    #region properties
    [SerializeField]
    private GameObject[] type;

    private int[][] waves;
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
    #endregion
    #region references
    private Transform _myTransform;
    #endregion
    #region methods
    private void InitializeWaveArray()
    {
        waves = new int[4][];
        waves[0] = wave0;
        waves[1] = wave1;
        waves[2] = wave2;
        waves[3] = wave3;
    }
    private void Spawn()
    {
        if (GameManager.Instance.GetNumEnemies() < 0) GameManager.Instance.InitEnemyNumber();
        int[] currentWave = waves[GameManager.Instance.GetCurrentWave()];
        Debug.Log(GameManager.Instance.GetCurrentWave());
        for (int i = 0; i < currentWave.Length; i++)
        {
            //el tipo de enemigo que se spawnea depende de waveContent
            Instantiate(type[currentWave[i]], spawnPos[i], Quaternion.identity, _myTransform);
            GameManager.Instance.EnemySpawned();
        }
        Debug.Log(GameManager.Instance.GetNumEnemies());
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
                spawnPos[cont] = new Vector2(transform.position.x + i, transform.position.y - j);
                cont++;
            }
        }
        Invoke("Spawn", 3.0f);
        
        InitializeWaveArray();
    }

    void Update()
    {
        Debug.Log("enemies: " + GameManager.Instance.GetNumEnemies());
        if (GameManager.Instance.WaveTimer() || GameManager.Instance.GetNumEnemies() == 0)
        {
            GameManager.Instance.NextWave();
            Invoke("Spawn", 3.0f);
        }
    }
}
