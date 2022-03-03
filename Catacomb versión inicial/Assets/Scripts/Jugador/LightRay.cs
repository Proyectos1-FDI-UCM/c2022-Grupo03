using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRay : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _length;
    [SerializeField]
    private float _cooldown;
    #endregion

    #region properties
    private string[] _enemyColors = { "Red", "Yellow", "Green", "Blue", "Pink" };
    private bool _attackMade;
    private float _elapsedTime;
    #endregion

    #region references
    PlayerChangeColors _myPlayerChangeColors;
    Transform _myTransform;
    [SerializeField]
    GameObject _dirArrow;
    Transform _dirArrowTransform;
    PlayerMovementController _myPlayerMovementController;
    PlayerInputManager _myPlayerInputManager;
    #endregion

    #region methods
    public void Shoot()
    {
        if (!_attackMade)
        {
            Debug.DrawRay(_myTransform.position, _dirArrowTransform.right.normalized * _length, Color.red, 2f);
            RaycastHit2D hitInfo;
            hitInfo = Physics2D.Raycast(_myTransform.position, _dirArrowTransform.right, _length);
            if (hitInfo)
            {
                int indice = _myPlayerChangeColors.GetCurrentColorIndex();
                if (hitInfo.collider.GetComponent(_enemyColors[indice]) != null)
                {
                    hitInfo.collider.GetComponent<EnemyLifeComponent>().Damage();
                }
            }
            _attackMade = true;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _myPlayerChangeColors = GetComponent<PlayerChangeColors>();
        _myTransform = transform;
        _dirArrowTransform = _dirArrow.transform;
        _attackMade = false;
        _myPlayerMovementController = GetComponent<PlayerMovementController>();
        _myPlayerInputManager = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _cooldown)
        {
            _attackMade = false;
            _elapsedTime = 0;
        }
    }
}
