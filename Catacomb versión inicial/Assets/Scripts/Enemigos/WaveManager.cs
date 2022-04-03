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
        Debug.Log("spawnentered");
        if (GameManager.Instance.GetNumEnemies() < 0) GameManager.Instance.InitEnemyNumber();
        Debug.Log("OAIHUDGUYAUDUAY  " + GameManager.Instance.GetCurrentWave());
        Debug.Log("AAAAAAAAAAAAA  " + GameManager.Instance.GetCurrentWave());
        Debug.Log("length: " + waves[1].Length);
        for (int i = 0; i < waves[GameManager.Instance.GetCurrentWave()].Length; i++)
        {
            // el tipo de enemigo que se spawnea depende de waveContent
            Instantiate(type[waves[GameManager.Instance.GetCurrentWave()][i]], spawnPos[i], Quaternion.identity, _myTransform);
            GameManager.Instance.EnemySpawned();
        }
    }

    private void NextLevel()
    {
        if (GameManager.Instance.GetCurrentWave() < waves.Length)
        {
            Spawn();
        }
        else
        {
            GameManager.Instance.nivelTerminado = true;
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
                spawnPos[cont] = new Vector2(transform.position.x + i, transform.position.y - j);
                cont++;
            }
        }
        InitializeWaveArray();

        Invoke(nameof(Spawn), 3.0f);
    }

    void Update()
    {
        if (GameManager.Instance.State)
        {
            Debug.Log("spawned");
            Invoke(nameof(NextLevel), 0.0f);
            GameManager.Instance.State = false;
        }
    }
}
