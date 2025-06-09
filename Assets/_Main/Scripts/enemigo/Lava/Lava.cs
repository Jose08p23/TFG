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

    [Tooltip("Sonido ambiente que se reproduce en bucle mientras la lava está activa")]
    public AudioClip sonidoAmbienteLava;

    [Header("Escena")]
    [Tooltip("Nombre de la escena que se carga al morir")]
    public string nombreEscenaMuerte = "DeathMenu";

    [Tooltip("Retraso antes de cambiar de escena (para dejar sonar el audio)")]
    public float delayAntesDeMorir = 0.5f;

    private bool haQuemado = false;
    private AudioSource audioSource;

    void Start()
    {
        // Configura el AudioSource para el sonido ambiente de la lava
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoAmbienteLava;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0.8f; // Puedes ajustar el volumen aquí

        if (sonidoAmbienteLava != null)
        {
            audioSource.Play();
        }
    }

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

            // Bloquear movimiento del jugador 3 segundos
            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.BloquearMovimientoTemporal(3f); // 3 segundos
            }

            // Reproducir sonido desde el AudioManager
            if (sonidoQuemadura != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.ReproducirSonido(sonidoQuemadura);
            }

            // Esperar antes de cargar la escena de muerte
            StartCoroutine(CargarPantallaMuerte(delayAntesDeMorir + 3f)); // Espera 3s más
        }
    }

    private System.Collections.IEnumerator CargarPantallaMuerte(float delay)
    {
        SceneTracker.EscenaAnterior = SceneManager.GetActiveScene().name;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nombreEscenaMuerte);
    }
}
