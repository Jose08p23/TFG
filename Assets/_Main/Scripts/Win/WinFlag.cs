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
        if (escenaActual.StartsWith("LavaLevel"))
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

            int nuevasMonedas = GameManager.Instance.PuntosTotales;
            float nuevoTiempo = GameManager.Instance.Timer;

            // Guardar nombre del nivel si aún no está asignado
            nivel.nombreNivel = escenaActual;

            // Solo actualiza si mejora los datos actuales
            if (nuevasMonedas > nivel.monedasRecogidas)
            {
                nivel.monedasRecogidas = nuevasMonedas;
                nivel.porcentajeGuardado = nuevasMonedas * 2f;
            }

            if (nivel.mejorTiempo <= 0f || nuevoTiempo < nivel.mejorTiempo)
            {
                nivel.mejorTiempo = nuevoTiempo;
            }

            if (!nivel.completado)
            {
                nivel.completado = true;
            }

            progreso.Guardar();

            Debug.Log($"Guardado nivel: {escenaActual} | Tiempo: {nivel.mejorTiempo} | Monedas: {nivel.monedasRecogidas} | Porcentaje: {nivel.porcentajeGuardado} | Nombre: {nivel.nombreNivel}");
        

    // Marcar como completado si no lo estaba
    if (!nivel.completado)
    {
        Debug.Log("[Guardado] Marcando nivel como completado.");
        nivel.completado = true;
    }

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
