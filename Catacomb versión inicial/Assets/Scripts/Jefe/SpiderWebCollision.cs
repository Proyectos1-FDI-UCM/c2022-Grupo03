using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebCollision : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties

    #endregion

    #region references
    private Transform _myTransform;
    private GameObject _spiderHead;
    private Transform _spiderHeadTransform;
    private LineRenderer _myLineRenderer;
    private EdgeCollider2D _myEdgeCollider2D;
    private List<Vector2> _edges;
    #endregion

    #region methods
    private void SetEdgeFirstPoint()
    {
        _myLineRenderer.positionCount = 2;
        _myLineRenderer.SetPosition(0, _myTransform.position + new Vector3(0, -0.5f, 0));
        _edges.Add(Vector2.zero);
    }

    private void SetEdgeSecondPoint()
    {
        _myLineRenderer.SetPosition(1, _myTransform.position + new Vector3(0, -0.5f, 0));
        Vector2 webDir = _myTransform.position - _spiderHeadTransform.position;
        _edges.Add(new Vector2(0, -webDir.magnitude));
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _spiderHead = GameObject.Find("SpiderHead");
        _spiderHeadTransform = _spiderHead.transform;
        _myLineRenderer = GetComponent<LineRenderer>();
        _myEdgeCollider2D = GetComponent<EdgeCollider2D>();
        _edges = new List<Vector2>();

        SetEdgeFirstPoint();
    }

    // Update is called once per frame
    void Update()
    {
        SetEdgeSecondPoint();
        // se crea el collider a partir de los puntos que hay en la lista
        _myEdgeCollider2D.SetPoints(_edges);
        // se elimina el último punto de la lista, que es el que se va a modificar
        _edges.RemoveAt(1);
    }
}
