using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // se comprueba si lo que está situado en la curación es el personaje y luego, si tiene menos de la vida máxima
        PlayerLifeComponent playerLifeComponent = other.GetComponent<PlayerLifeComponent>();
        LifeRecAnimation lifeRecAnimation = other.GetComponent<LifeRecAnimation>();
        if (playerLifeComponent != null && playerLifeComponent.Heal())
        {
            GameObject.Destroy(gameObject);
            lifeRecAnimation.HealAni();
        }
    }
}
