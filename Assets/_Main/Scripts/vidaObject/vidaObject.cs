using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vidaObject : MonoBehaviour
{
    public AudioClip sonidoVida;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
           bool vidaRecuperada = GameManager.Instance.RecuperarVida();
           if (vidaRecuperada){
            
            AudioManager.Instance.ReproducirSonido(sonidoVida);                  
            Destroy(this.gameObject);
           }
        }

    }

}
