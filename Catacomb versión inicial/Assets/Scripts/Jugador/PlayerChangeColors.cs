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
    #endregion

    #region methods
    private int ColorToIndex(colors col)
    {
        int indice = (int)col;
        return indice;
    }

    private colors IndexToColor(int indice)
    {
        colors col = (colors)indice;
        return col;
    }
    public int GetCurrentColorIndex()
    {
        return ColorToIndex(_currentColor);
    }

    // m�todos para cambiar de color
    public void SetCurrentColor(int indice)
    {
        _currentColor = IndexToColor(indice);
        Debug.Log(_currentColor);
    }

    public void ChangeColor(float variation)
    {
        if (variation > 0f)
        {
            if (_currentColor >= colors.Rosa)
            {
                _currentColor = 0;
            }
            else
            {
                _currentColor++;
            }
        }
        else if (variation < 0f)
        {
            if (_currentColor <= 0)
            {
                _currentColor = colors.Rosa;
            }
            else
            {
                _currentColor--;
            }
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
