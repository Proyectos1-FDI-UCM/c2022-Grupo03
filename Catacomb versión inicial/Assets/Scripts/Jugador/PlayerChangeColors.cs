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

    // métodos para cambiar de color
    public void SetCurrentColor(int indice)
    {
        _currentColor = IndexToColor(indice);
        GameManager.Instance.OnPlayerChangeColor(ColorToString(_currentColor));
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
        GameManager.Instance.OnPlayerChangeColor(ColorToString(_currentColor));
    }

    private string ColorToString(colors col)
    {
        string colString = "";
        switch (col)
        {
            case colors.Rojo:
                colString = "Rojo";
                break;
            case colors.Amarillo:
                colString = "Amarillo";
                break;
            case colors.Azul:
                colString = "Azul";
                break;
            case colors.Verde:
                colString = "Verde";
                break;
            case colors.Rosa:
                colString = "Rosa";
                break;
        }
        return colString;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _currentColor = 0;
        GameManager.Instance.OnPlayerChangeColor(ColorToString(_currentColor));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
