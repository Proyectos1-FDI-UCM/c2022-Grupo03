using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebAttack : MonoBehaviour
{
    #region parameters
    [SerializeField]
    private float _oftenAttack;
    [SerializeField]
    private int _spiderWebDamage;
    [SerializeField]
    private float _speedReducedPlayer;
    [SerializeField]
    private float _durationSpeedReduced;
    #endregion

    #region properties
    private float _elapsedTime;
    public float ElapsedTime { set => _elapsedTime = value; }
    #endregion

    #region references
    [SerializeField]
    private GameObject _spiderWebBullet;
    private PlayerMovementController _playerMovementController;
    [SerializeField]
    private GameObject _spiderBody;
    [SerializeField]
    private GameObject _spiderHead;
    private Transform _spiderHeadTransform;

    #endregion

    #region methods
    public void SpiderWeb()
    {
        _elapsedTime += Time.deltaTime;
        Debug.Log(_elapsedTime);
        if (_elapsedTime > _oftenAttack)
        {
            GameObject spiderWeb = Instantiate(_spiderWebBullet, _spiderHeadTransform.position, Quaternion.identity);
            ProjectileMovement projectileMovement = spiderWeb.GetComponent<ProjectileMovement>();
            projectileMovement.SetDamage(_spiderWebDamage);
            projectileMovement.SetSpeed(_speedReducedPlayer);
            _elapsedTime = 0;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _elapsedTime = 0;
        // ataque de telaraña
        _playerMovementController = GameObject.Find("Player").GetComponent<PlayerMovementController>();
        _playerMovementController.SetDuration(_durationSpeedReduced);
        _spiderHeadTransform = _spiderHead.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
