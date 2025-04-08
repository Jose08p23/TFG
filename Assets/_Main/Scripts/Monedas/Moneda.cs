using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class Moneda : MonoBehaviour
{

public int valor = 1;
public AudioClip sonidoMoneda;

void Start(){


}

void Update(){


}

private void OnTriggerEnter2D(Collider2D collision){

    if(collision.CompareTag("Player")){
        GameManager.Instance.SumarPuntos(valor);
        AudioManager.Instance.ReproducirSonido(sonidoMoneda);
        Destroy(this.gameObject);
    }
}

}
