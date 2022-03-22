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
    #endregion

    #region references
    GameObject _player;
    PlayerChangeColors _playerChangeColors;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collider)
    {
        int indice = _playerChangeColors.GetComponent<PlayerChangeColors>().GetCurrentColorIndex();
        // comprobar si contra lo que choca es un enemigo y su color
        if (collider.GetComponent(_enemyColors[indice]) != null)
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
        _player = GameObject.Find("Player");
        _playerChangeColors = _player.GetComponent<PlayerChangeColors>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
