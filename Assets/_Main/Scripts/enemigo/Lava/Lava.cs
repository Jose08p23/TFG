using UnityEngine;
using UnityEngine.SceneManagement;

public class Lava : MonoBehaviour
{
    [Header("Movimiento")]
    [Tooltip("Velocidad a la que la lava sube (unidades por segundo)")]
    public float velocidadSubida = 0.5f;

    [Header("Audio")]
    [Tooltip("Sonido que se reproduce al quemar al jugador")]
    public AudioClip sonidoQuemadura;

    [Header("Escena")]
    [Tooltip("Nombre de la escena que se carga al morir")]
    public string nombreEscenaMuerte = "DeathMenu";

    [Tooltip("Retraso antes de cambiar de escena (para dejar sonar el audio)")]
    public float delayAntesDeMorir = 0.5f;

    private bool haQuemado = false;

    void Update()
    {
        // Mover la lava hacia arriba constantemente
        transform.position += Vector3.up * velocidadSubida * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (haQuemado) return;

        if (other.CompareTag("Player"))
        {
            haQuemado = true;

            // Reproducir sonido desde el AudioManager
            if (sonidoQuemadura != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.ReproducirSonido(sonidoQuemadura);
            }

            // Esperar antes de cargar la escena de muerte
            StartCoroutine(CargarPantallaMuerte(delayAntesDeMorir));
        }
    }

    private System.Collections.IEnumerator CargarPantallaMuerte(float delay)
    {
        SceneTracker.EscenaAnterior = SceneManager.GetActiveScene().name;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nombreEscenaMuerte);
    }
}
