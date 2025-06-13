using UnityEngine;
using UnityEngine.SceneManagement;

public class Lava : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadSubida = 0.5f;

    [Header("Audio")]
    [Tooltip("Sonido que se reproduce al quemar al jugador")]
    public AudioClip sonidoQuemadura;

    [Header("Escena")]
    public string nombreEscenaMuerte = "DeathMenu";
    public float delayAntesDeMorir = 0.5f;

    private bool haQuemado = false;

    void Update()
    {
        transform.position += Vector3.up * velocidadSubida * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (haQuemado) return;

        if (other.CompareTag("Player"))
        {
            haQuemado = true;

            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.BloquearMovimientoTemporal(3f);
            }

            if (sonidoQuemadura != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.ReproducirSonido(sonidoQuemadura);
            }

            StartCoroutine(CargarPantallaMuerte(delayAntesDeMorir + 3f));
        }
    }

    private System.Collections.IEnumerator CargarPantallaMuerte(float delay)
    {
        SceneTracker.EscenaAnterior = SceneManager.GetActiveScene().name;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nombreEscenaMuerte);
    }
}
