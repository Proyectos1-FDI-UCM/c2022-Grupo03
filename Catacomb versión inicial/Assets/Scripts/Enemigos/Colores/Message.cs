using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private Vector3 _offset = new Vector3(0, 1, 0);
    [SerializeField]
    private float _messageDuration = 2;
    #endregion

    #region properties

    #endregion

    #region references
    private Transform _myTransform;
    private Transform _myEnemyTransform;
    private GameObject _message;
    private Text _text;
    #endregion

    #region methods
    public void ActivateMessage()
    {
        _message.SetActive(true);
        Invoke(nameof(DeactivateMessage), _messageDuration);
    }

    private void DeactivateMessage()
    {
        _message.SetActive(false);
    }

    private void MessageRotation(int value, float rotY)
    {
        if (_myEnemyTransform.localScale.x == value)
        {
            _myTransform.rotation = Quaternion.Euler(0, rotY, 0);
        }
    }

    public void SetMessage(string message)
    {
        _text.text = message;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _myEnemyTransform = transform.parent;
        _message = _myTransform.GetChild(0).gameObject;
        _text = _message.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _myTransform.position = _myEnemyTransform.position + _offset;
        MessageRotation(-1, 180);
        MessageRotation(1, 0);
    }
}
