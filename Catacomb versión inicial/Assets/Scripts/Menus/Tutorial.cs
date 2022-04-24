using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    [SerializeField]
    private GameObject _tutorial;
    private MainMenu _mainmenu;

    public void IniTutorial()
    {
        if (_tutorial != null) _tutorial.SetActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        _mainmenu = GetComponent<MainMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
