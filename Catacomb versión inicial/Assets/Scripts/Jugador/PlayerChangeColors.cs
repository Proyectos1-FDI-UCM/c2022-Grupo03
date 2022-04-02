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
        if (indice < GameManager.Instance.NumActiveCols)
        {
            _currentColor = IndexToColor(indice);
            GameManager.Instance.OnPlayerChangeColor(GameManager.Instance.LightColors[indice]);
        }
    }

    public void ChangeColor(float variation)
    {
        int limIndexCol = GameManager.Instance.NumActiveCols - 1;
        if (variation > 0f)
        {
            if (_currentColor >= colors.Rosa || _currentColor >= IndexToColor(limIndexCol))
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
                _currentColor = IndexToColor(limIndexCol);
            }
            else
            {
                _currentColor--;
            }
        }
        int indice = ColorToIndex(_currentColor);
        GameManager.Instance.OnPlayerChangeColor(GameManager.Instance.LightColors[indice]);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _currentColor = 0;
        GameManager.Instance.OnPlayerChangeColor(GameManager.Instance.LightColors[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
