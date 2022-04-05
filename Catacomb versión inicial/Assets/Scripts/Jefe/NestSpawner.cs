using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestSpawner : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private Vector3[] _nestPositions;
    [SerializeField]
    private int _numNestsSpawn;
    #endregion

    #region properties
    // indica el nido de ara�a que aparece en la escena
    private int _nestIndex;
    // vector de booleanos que sirve para indicar si un nido est� desactivado o no
    // true --> est� activado
    // false --> est� desactivado
    private bool[] _nestsActivated;
    // n�mero de nidos que aparecen en cada segunda fase
    private int _numNests;
    #endregion

    #region references
    private GameObject[] _nests;
    [SerializeField]
    private GameObject _nestPrefab;
    #endregion

    #region methods
    // l�gica del spawn de los nidos de ara�a
    public void SpawnNests()
    {
        CreateNests();
        bool todosActivados = false;
        // el bucle termina cuando todos los nido est�n activados
        // o cuando se han activado como m�ximo el n�mero de nidos que hab�amos determinado
        int i = 0;
        while (i < _numNestsSpawn && !todosActivados)
        {
            // si todos los nidos est�n activados no se
            // puede activar ninguno y, por lo tanto, el bucle termina
            todosActivados = CheckAllNests();
            ActivateNests(todosActivados, ref i);
        }
    }

    private void CreateNests()
    {
        // los nidos que no est�n en escena se instancian y se desactivan
        // posteriormente se ir�n activando
        for (int i = 0; i < _numNests; i++)
        {
            if (_nests[i] == null)
            {
                _nests[i] = Instantiate(_nestPrefab, _nestPositions[i], Quaternion.identity);
                _nests[i].SetActive(false);
                _nestsActivated[i] = false;
            }
        }
    }
    
    public void DestroyNests()
    {
        for (int i = 0; i < _numNests; i++)
        {
            if (_nests[i] != null)
            {
                GameObject.Destroy(_nests[i]);
            }
        }
    }

    private bool CheckAllNests()
    {
        // comprueba si todos los nidos est�n activados
        // si lo est�n devuelve true
        // en caso contrario, devuelve false
        bool check = true;
        int j = 0;
        while (j < _numNests && check)
        {
            if (!_nestsActivated[j])
            {
                check = false;
            }
            j++;
        }
        return check;
    }

    private void ActivateNests(bool allActivated, ref int index)
    {
        // si no est�n todos los nidos activados procede a activar varios
        if (!allActivated)
        {
            // si el nido no est� activado
            // lo activa, lo marco como activado y
            // aumenta index en 1 para saber que se ha activado un nido
            if (!_nestsActivated[_nestIndex])
            {
                _nests[_nestIndex].SetActive(true);
                _nestsActivated[_nestIndex] = true;
                _nestIndex++;
                index++;
            }
            // en caso de que el nido est� activado pasa a buscar otro que no lo est�
            else
            {
                _nestIndex++;
            }
            // si llega al �ltimo nido pasa al primero
            if (_nestIndex == _numNests)
            {
                _nestIndex = 0;
            }
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // inicializar las referencias los nidos de ara�a
        _numNests = _nestPositions.Length;
        _nests = new GameObject[_numNests];
        _nestsActivated = new bool[_numNests];
        _nestIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
