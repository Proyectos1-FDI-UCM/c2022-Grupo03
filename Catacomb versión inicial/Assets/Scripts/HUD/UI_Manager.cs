using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties

    #endregion

    #region references
    [SerializeField]
    GameObject _currentColorObject;
    Text _currentColorText;
    [SerializeField]
    GameObject _spinCooldownObject;
    Text _spinCooldownText;
    [SerializeField]
    GameObject _rayCooldownObject;
    Text _rayCooldownText;
    [SerializeField]
    GameObject _enemiesLeftObject;
    Text _enemiesLeftText;
    #endregion

    #region methods
    public void UpdateCurrentColor(string newColor)
    {
        _currentColorText.text = newColor;
    }
    public void UpdateSpinCooldown(int newTime)
    {
        _spinCooldownText.text = "Spin: " + newTime;
    }
    public void UpdateRayCooldown(int newTime)
    {
        _rayCooldownText.text = "Ray: " + newTime;
    }
    public void UpdateEnemiesLeft(int numEnemies)
    {
        _enemiesLeftText.text = "Enemies: " + numEnemies;
    }
    private void Awake()
    {
        _currentColorText = _currentColorObject.GetComponent<Text>();
        _spinCooldownText = _spinCooldownObject.GetComponent<Text>();
        _rayCooldownText = _rayCooldownObject.GetComponent<Text>();
        _enemiesLeftText = _enemiesLeftObject.GetComponent<Text>();
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
