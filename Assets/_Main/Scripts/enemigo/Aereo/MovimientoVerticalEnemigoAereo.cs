using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoVerticalEnemigoAereo : MonoBehaviour
{
    public float velocidad = 3f;            // Velocidad de movimiento
    public LayerMask capaSuelo;             // Capa para detectar suelo y techo
    public Transform detectorSuelo;         // Punto de detección del suelo
    public Transform detectorTecho;         // Punto de detección del techo

    private bool moviendoArriba = true;     // Control de dirección

    void FixedUpdate()
    {
        // Aplicar velocidad en la dirección actual
        transform.position += new Vector3(0, velocidad * Time.fixedDeltaTime * (moviendoArriba ? 1 : -1), 0);

        // Detectar colisión con el suelo o techo
        if (DetectarColision(detectorSuelo, Vector2.down) || DetectarColision(detectorTecho, Vector2.up))
        {
            CambiarDireccion();
        }
    }

    bool DetectarColision(Transform detector, Vector2 direccion)
    {
        float distanciaDeteccion = 0.2f; // Distancia para detectar colisión
        RaycastHit2D hit = Physics2D.Raycast(detector.position, direccion, distanciaDeteccion, capaSuelo);
        return hit.collider != null;
    }

    void CambiarDireccion()
    {
        moviendoArriba = !moviendoArriba;
    }

    void OnDrawGizmos()
    {
        // Dibujar líneas de detección en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawLine(detectorSuelo.position, detectorSuelo.position + Vector3.down * 0.2f);
        Gizmos.DrawLine(detectorTecho.position, detectorTecho.position + Vector3.up * 0.2f);
    }
}
