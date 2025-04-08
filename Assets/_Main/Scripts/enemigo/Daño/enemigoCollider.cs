using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemigoCollider : MonoBehaviour
{
    private bool puedeAtacar = true;
    private SpriteRenderer spriteRenderer;
    private Color colorOriginal; // Guarda el color original del enemigo

    private float coolDownAtaque = 1.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorOriginal = spriteRenderer.color; // Guarda el color original al inicio
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!puedeAtacar) return;
            puedeAtacar = false;

            // Cambia el color a rojo (conservando la tonalidad original)
            spriteRenderer.color = new Color(1f, spriteRenderer.color.g * 0.5f, spriteRenderer.color.b * 0.5f);

            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            // Si el jugador es vulnerable, se le aplica el daño (vida) y se desactiva la vulnerabilidad
            if (player.puedeRecibirDaño)
            {
                GameManager.Instance.PerderVida();
                player.puedeRecibirDaño = false;
                player.coolDownParpadeo = coolDownAtaque;
                player.StartCoroutine(player.Parpadear());
            }
            
            // En cualquier caso, se aplica el golpe (efecto knockback)
            player.AplicarGolpe();

            Invoke("ReactivarAtaque", coolDownAtaque);
        }
    }

    void ReactivarAtaque()
    {
        puedeAtacar = true;
        spriteRenderer.color = colorOriginal; // Restablece el color original
    }
}