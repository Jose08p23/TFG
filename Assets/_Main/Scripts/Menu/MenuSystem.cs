using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject menuPausa;
    public GameObject opcionesPanel;
    public bool juegoPausado = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado && opcionesPanel.activeSelf)
            {
                CerrarOpciones();
            }
            else if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        menuPausa?.SetActive(false);
        opcionesPanel?.SetActive(false);
        Time.timeScale = 1;
        juegoPausado = false;
    }

    public void Pausar()
    {
        menuPausa?.SetActive(true);
        Time.timeScale = 0;
        juegoPausado = true;
    }


    public void MenuPrincipalFunc()
    {
        Time.timeScale = 1;
        juegoPausado = false;

        SceneTracker.EscenaAnterior = SceneManager.GetActiveScene().name;

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.CambiarEscena("Menú");
        }
        else
        {
            Debug.LogWarning("FadeManager no disponible para MenuPrincipalFunc.");
        }
    }

    public void Salir()
    {
        Debug.Log("Saliendo del Juego...");
        Application.Quit();
    }

    public void Reiniciar()
    {
        menuPausa?.SetActive(false);
        opcionesPanel?.SetActive(false);
        Time.timeScale = 1;
        juegoPausado = false;

        string escenaActual = SceneManager.GetActiveScene().name;

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.CambiarEscena(escenaActual);
        }
        else
        {
            Debug.LogWarning("FadeManager no disponible para Reiniciar.");
        }
    }

    public void AbrirOpciones()
    {
        opcionesPanel?.SetActive(true);
    }

    public void CerrarOpciones()
    {
        opcionesPanel?.SetActive(false);
    }

    public void VolverAEscenaAnterior()
    {
        if (!string.IsNullOrEmpty(SceneTracker.EscenaAnterior))
        {
            if (FadeManager.Instance != null)
            {
                FadeManager.Instance.CambiarEscena(SceneTracker.EscenaAnterior);
            }
            else
            {
                Debug.LogWarning("FadeManager no disponible para VolverAEscenaAnterior.");
            }
        }
        else
        {
            Debug.LogWarning("No hay escena anterior registrada. No se puede volver.");
        }
    }

    public void AvanzarNivel()
    {
        string escenaAnterior = SceneTracker.EscenaAnterior;

        if (string.IsNullOrEmpty(escenaAnterior))
        {
            Debug.LogWarning("No hay escena anterior registrada. No se puede avanzar.");
            return;
        }

        Debug.Log("Escena anterior: " + escenaAnterior);

        Match match = Regex.Match(escenaAnterior, @"(\d+)$");
        if (!match.Success)
        {
            Debug.LogWarning("No se encontró un número al final del nombre de la escena.");
            return;
        }

        int nivelActual = int.Parse(match.Value);
        int siguienteNivel = nivelActual + 1;

        string nombreSiguiente = Regex.Replace(escenaAnterior, @"\d+$", siguienteNivel.ToString());

        Debug.Log("Cargando siguiente nivel: " + nombreSiguiente);

        if (!Application.CanStreamedLevelBeLoaded(nombreSiguiente))
        {
            Debug.LogWarning("El nivel '" + nombreSiguiente + "' no está en Build Settings.");
            return;
        }

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.CambiarEscena(nombreSiguiente);
        }
        else
        {
            Debug.LogWarning("FadeManager no disponible para AvanzarNivel.");
        }
    }
}
