using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAnimation : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _fps = 30f;
    // cada cuantos fps cambia la textura
    // cuanto mayor sea el valor antes cambiará
    #endregion

    #region properties
    private int _animationStep;
    private float _fpsCounter;
    #endregion

    #region references
    private LineRenderer _myLineRenderer;
    [SerializeField]
    private Texture[] _textures;
    #endregion

    #region methods

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myLineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _fpsCounter += Time.deltaTime;
        if (_fpsCounter > 1f / _fps)
        {
            _animationStep++;
            if (_animationStep == _textures.Length)
            {
                _animationStep = 0;
            }
            _myLineRenderer.material.SetTexture("_MainTex", _textures[_animationStep]);
            _fpsCounter = 0;
        }
    }
}
