using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region references
    private GameObject _player;
    private DeathAnimation _myDeathAnimation;
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
    public void Death()
    {
        Destroy(_player);
    }
    

    private void Awake()
    {
        _instance = this;
        
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _myDeathAnimation = GetComponent<DeathAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
