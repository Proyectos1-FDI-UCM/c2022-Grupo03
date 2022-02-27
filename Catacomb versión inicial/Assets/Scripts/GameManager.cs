using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region references
    [SerializeField]
    private GameObject player;
    #endregion


    #region properties
    static private GameManager _instance;
    static public GameManager Instance //Accesor a la instancia del game manager 
    {
        get
        {
            return _instance;
        }
    }
    #endregion


    #region methods
    public void OnPlayerDies() 
    {
        Destroy(player);
     
        //añadir más adelante que se resetea el mapa, ya sea llamando a otra función o dentro de esta
    }

    private void Awake()
    {
        _instance = this;
        
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
