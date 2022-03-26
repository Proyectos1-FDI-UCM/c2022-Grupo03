using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderNest : MonoBehaviour
{
    #region parameters
    // tiempo que transcurre entre que se genera un nuevo enemigo
    [SerializeField]
    private float _timeBtwEnemies;
    #endregion

    #region properties
    // índice de los enemigos
    // 0 - Ranged
    // 1 - Melee
    // 2 - Kamikaze
    // 3 - Tanque
    // se ha tratado de hacer de forma que se escriban los nombres
    // de los enemigos en un archivo de texto, pero no funciona
    private int[] _enemiesIndex;
    // alrededores del nido de araña
    private Vector3[] _offsets = { Vector3.up, Vector3.down, Vector3.right, Vector3.left };
    #endregion

    #region references
    // prefas de los diferentes tipos de enemigos
    [SerializeField]
    private GameObject[] _enemies;
    private Transform _myTransform;
    [SerializeField]
    private TextAsset _text;
    #endregion

    #region methods
    private int[] LeerArchivo()
    {
        // separar el texto ignorando los saltos de línea
        string[] subs = _text.text.Split("\n"[0]);

        int[] vector;
        vector = new int[subs.Length];
        for (int i = 0; i < vector.Length; i++)
        {
            vector[i] = int.Parse(subs[i]);
        }

        return vector;
    }

    // spawnea la misma secuencia de enemigos de forma infinita
    private IEnumerator SpawnEnemies(float time)
    {
        int i = 0;
        while (i < _enemiesIndex.Length)
        {
            yield return new WaitForSeconds(time);
            int numRandom = GameManager.Instance.NumRandom(0, 3);
            Vector3 spawnPoint = _myTransform.position + _offsets[numRandom];
            Instantiate(_enemies[_enemiesIndex[i]], spawnPoint, Quaternion.identity);
            i++;
            if (i == _enemiesIndex.Length)
            {
                i = 0;
            }
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _enemiesIndex = LeerArchivo();
        StartCoroutine(SpawnEnemies(_timeBtwEnemies));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
