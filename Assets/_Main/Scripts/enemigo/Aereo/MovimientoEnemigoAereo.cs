using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoAereo : MonoBehaviour
{
    public float velocidad = 3f;        // Velocidad de movimiento
    public LayerMask capaPared;         // Capa de colisión para detectar paredes
    public Transform detectorPared;     // Punto de detección de colisiones

    private Rigidbody2D rb;
    private bool moviendoDerecha = true; // Control de dirección
    private Vector3 escalaInicial;      // Guardar la escala original

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        escalaInicial = transform.localScale; // Guardamos la escala original
    }

    void FixedUpdate()
    {
        // Movimiento horizontal constante
        rb.velocity = new Vector2(velocidad * (moviendoDerecha ? 1 : -1), rb.velocity.y);

        // Detectar colisión con la pared
        if (DetectarPared())
        {
            CambiarDireccion();
        }
    }

    bool DetectarPared()
    {
        // Crea un BoxCast en la dirección de movimiento para detectar colisiones con paredes
        float distanciaDeteccion = 0.2f; // Asegura que detecte correctamente la pared
        RaycastHit2D hit = Physics2D.BoxCast(detectorPared.position, new Vector2(0.5f, 0.5f), 0f, 
                                             moviendoDerecha ? Vector2.right : Vector2.left, distanciaDeteccion, capaPared);
        return hit.collider != null;
    }

    void CambiarDireccion()
    {
        moviendoDerecha = !moviendoDerecha;
        transform.localScale = new Vector3(escalaInicial.x * (moviendoDerecha ? 1 : -1), escalaInicial.y, escalaInicial.z);
    }

    // Dibujar el detector en la vista del editor
    void OnDrawGizmos()
    {
        if (detectorPared != null)
        {
            Gizmos.color = Color.blue;
            Vector3 direccion = moviendoDerecha ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(detectorPared.position, detectorPared.position + direccion * 0.2f);
            Gizmos.DrawWireCube(detectorPared.position + direccion * 0.2f, new Vector3(0.5f, 0.5f, 0));
        }
    }
}
