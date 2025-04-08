using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoTerrestre : MonoBehaviour
{
    public float velocidad = 2f;           // Velocidad de movimiento
    public LayerMask capaSuelo;            // Capa para detectar el suelo
    public LayerMask capaPared;            // Nueva capa para detectar paredes
    public Transform detectorSuelo;        // Punto para detectar si hay suelo delante
    public Transform detectorPared;        // Nuevo punto para detectar paredes

    private Rigidbody2D rb;
    private bool moviendoDerecha = true;   // Control de dirección
    private bool puedeCambiarDireccion = true; // Para evitar cambios de dirección múltiples
    private Vector3 escalaInicial;         // Guardar la escala original del enemigo

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        escalaInicial = transform.localScale; // Guardamos la escala original
    }

    void FixedUpdate() // Se usa FixedUpdate para manejar la física
    {
        // Aplicar movimiento en la dirección actual
        rb.velocity = new Vector2(velocidad * (moviendoDerecha ? 1 : -1), rb.velocity.y);

        // Detectar si hay pared o no hay suelo delante
        if ((!DetectarSueloDelante() || DetectarParedDelante()) && puedeCambiarDireccion)
        {
            StartCoroutine(EsperarYCambiarDireccion());
        }
    }

    bool DetectarSueloDelante()
    {
        float distanciaDeteccion = 0.3f; // Distancia de detección
        RaycastHit2D hit = Physics2D.Raycast(detectorSuelo.position, Vector2.down, distanciaDeteccion, capaSuelo);
        return hit.collider != null;
    }

    bool DetectarParedDelante()
    {
        float distanciaDeteccion = 0.2f; // Distancia de detección para la pared
        Vector2 direccion = moviendoDerecha ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(detectorPared.position, direccion, distanciaDeteccion, capaPared);
        return hit.collider != null;
    }

    IEnumerator EsperarYCambiarDireccion()
    {
        puedeCambiarDireccion = false; // Evita cambios de dirección seguidos
        yield return new WaitForSeconds(0.1f); // Pequeña espera para evitar múltiples giros
        CambiarDireccion();
        puedeCambiarDireccion = true;
    }

    void CambiarDireccion()
    {
        moviendoDerecha = !moviendoDerecha;
        // Mantener la escala original pero invertir la dirección en X
        transform.localScale = new Vector3(escalaInicial.x * (moviendoDerecha ? 1 : -1), escalaInicial.y, escalaInicial.z);
    }

    // Dibujar detectores en la vista del editor
    void OnDrawGizmos()
    {
        if (detectorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(detectorSuelo.position, detectorSuelo.position + Vector3.down * 0.3f);
        }

        if (detectorPared != null)
        {
            Gizmos.color = Color.blue;
            Vector3 direccion = moviendoDerecha ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(detectorPared.position, detectorPared.position + direccion * 0.2f);
        }
    }
}
