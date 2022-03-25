using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMessage : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private Vector3 _offset = new Vector3(0, 1, 0); //distancia desde el transform hasta el cartel que se instancia a los enemigos azules que esquiven un ataque
    [SerializeField]
    private float _mensajeDuration = 2;
    #endregion

    #region references
    private Transform _myTransform;
    [SerializeField]
    private GameObject _mensaje;
    #endregion

    #region methods
    public void CreaMensaje()
    {
        Vector3 pos = _myTransform.position + _offset;
        _mensaje = Instantiate(_mensaje, pos, Quaternion.identity);
        Invoke(nameof(DestruyeMensaje), _mensajeDuration);
    }

    private void DestruyeMensaje()
    {
        Destroy(_mensaje);
    }


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
