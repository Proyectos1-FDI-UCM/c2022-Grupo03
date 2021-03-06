using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    #region parameters

    #endregion

    #region properties
    private float _elapsedTime;
    private string[] _enemyColors = { "Red", "Yellow", "Green", "Blue", "Pink" };
    private int _damage;
    private int _indice;
    #endregion

    #region references
    private GameObject _player;
    private PlayerChangeColors _playerChangeColors;
    private SpriteRenderer _mySpriteRenderer;
    private BossManager _bossManager;
    private GameObject _bossManagerObject;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Shield enemyTankShield = collider.GetComponent<Shield>();
        EnemyLifeComponent enemyLifeComponent = collider.GetComponent<EnemyLifeComponent>();
        if (enemyTankShield != null)
        {
            if (enemyTankShield.gameObject.GetComponent(_enemyColors[_indice]) != null)
            {
                enemyTankShield.ChangeShieldCol();
            }
        }
        // comprobar si contra lo que choca es un enemigo y su color
        else if (enemyLifeComponent != null && collider.gameObject.GetComponentInChildren<Shield>() == null)
        {
            if (collider.GetComponent(_enemyColors[_indice]) != null)
            {
                // solo se puede da?ar al jefe si se encuentra en el segundo estado
                if (collider.name != "SpiderHead" || _bossManager.State == 2)
                {
                    enemyLifeComponent.Damage(_damage);
                }
            }
        }
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // obtener el color actual de la espada
        _player = GameObject.Find("Player");
        _playerChangeColors = _player.GetComponent<PlayerChangeColors>();
        _indice = _playerChangeColors.GetCurrentColorIndex();

        // ajustar las diferentes cosas al color de la espada
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        if (_mySpriteRenderer != null)
        {
            _mySpriteRenderer.color = GameManager.Instance.TranslucentColors[_indice];
        }

        _bossManagerObject = GameObject.Find("BossManager");
        if (_bossManagerObject)
        {
            _bossManager = GameObject.Find("BossManager").GetComponent<BossManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
