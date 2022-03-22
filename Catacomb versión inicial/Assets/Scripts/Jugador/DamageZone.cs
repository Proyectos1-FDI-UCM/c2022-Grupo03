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
    GameObject _player;
    PlayerChangeColors _playerChangeColors;
    SpriteRenderer _mySpriteRenderer;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // comprobar si contra lo que choca es un enemigo y su color
        if (collider.GetComponent(_enemyColors[_indice]) != null)
        {
            collider.GetComponent<EnemyLifeComponent>().Damage(_damage);
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
        _mySpriteRenderer.color = GameManager.Instance.TranslucentColors[_indice];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
