using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebAttack : MonoBehaviour
{
    #region parameters
    // tiempo que transcurre entre ataque y ataque
    [SerializeField]
    private float _oftenAttack;
    // daño del ataque
    [SerializeField]
    private int _spiderWebDamage;
    // velocidad reducida del jugdador
    [SerializeField]
    private float _speedReducedPlayer;
    // duración de la reducción de velocidad
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
    [SerializeField]
    private GameObject _shotPoint;
    private Transform _shotPointTransform;
    private PlayerMovementController _playerMovementController;
    #endregion

    #region methods
    public void SpiderWeb()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _oftenAttack)
        {
            GameObject spiderWeb = Instantiate(_spiderWebBullet, _shotPointTransform.position, Quaternion.identity);
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
        _shotPointTransform = _shotPoint.transform;
        _playerMovementController = GameObject.Find("Player").GetComponent<PlayerMovementController>();
        _playerMovementController.SetDuration(_durationSpeedReduced);
        _elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
