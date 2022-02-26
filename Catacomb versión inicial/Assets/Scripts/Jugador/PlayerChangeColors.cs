using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeColors : MonoBehaviour
{
    #region parameters

    #endregion

    #region references

    #endregion

    #region properties
    private enum colors { Rojo, Amarillo, Verde, Azul, Rosa };
    private colors _currentColor;
    private int _currentIndex;
    #endregion

    #region methods
    private int ColorToIndex(colors col)
    {
        int indice = (int)col;
        return indice;
    }

    public int GetCurrentColorIndex()
    {
        return ColorToIndex(_currentColor);
    }

    public void ChangeColor(float variation)
    {
        if (variation > 0)
        {
            _currentColor++;
        }
        else if (variation < 0)
        {
            _currentColor--;
        }

        if (_currentColor < 0)
        {
            _currentColor = colors.Rosa;
        }
        else if (_currentColor > colors.Rosa)
        {
            _currentColor = 0;
        }
        Debug.Log(_currentColor);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _currentColor = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
