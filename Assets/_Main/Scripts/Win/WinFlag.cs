using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinFlag : MonoBehaviour
{
    // Tiempo de espera antes de cargar la escena de victoria
    public float delay = 1.5f;

    private bool victoriaActivada = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detectado con: " + other.gameObject.name);
        if (victoriaActivada) return;

        if (other.CompareTag("Player"))
        {
            victoriaActivada = true;
            Debug.Log("Jugador detectado, cargando escena WinMenu en " + delay + " segundos");
            StartCoroutine(LoadSceneAfterDelay(delay));
        }
    }

    private IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        string escenaActual = SceneManager.GetActiveScene().name;
        SceneTracker.EscenaAnterior = escenaActual;

        // Cargar progreso desde disco
        ProgresoJugador progreso = ProgresoJugador.Cargar();

        // Detectar el modo de juego en base al nombre de la escena
        ModoJuegoData modo = null;
        if (escenaActual.StartsWith("NivelLava"))
            modo = progreso.lava;
        else if (escenaActual.StartsWith("Hitless"))
            modo = progreso.hitless;
        else
            modo = progreso.normal;

        // Extraer número del nivel (ej: "NivelLava3" => 3)
        Match match = Regex.Match(escenaActual, @"(\d+)$");
        int numeroNivel = (match.Success ? int.Parse(match.Value) - 1 : -1);

        if (modo != null && numeroNivel >= 0 && numeroNivel < modo.niveles.Count)
        {
            var nivel = modo.niveles[numeroNivel];
            nivel.completado = true;
            nivel.monedasRecogidas = GameManager.Instance.PuntosTotales;
            nivel.mejorTiempo = GameManager.Instance.Timer;

            progreso.Guardar();
            Debug.Log($"Progreso guardado para {escenaActual} - Monedas: {nivel.monedasRecogidas}, Tiempo: {nivel.mejorTiempo}");
        }
        else
        {
            Debug.LogWarning("No se pudo guardar el progreso del nivel. Datos inválidos.");
        }

        // Cambio de escena a WinMenu
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.CambiarEscena("WinMenu");
        }
        else
        {
            SceneManager.LoadScene("WinMenu");
        }
    }
}
