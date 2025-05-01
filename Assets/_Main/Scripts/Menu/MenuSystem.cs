using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (menuPausa != null) menuPausa.SetActive(false);
        if (opcionesPanel != null) opcionesPanel.SetActive(false);
        Time.timeScale = 1;
        juegoPausado = false;
    }

    public void Pausar()
    {
        if (menuPausa != null) menuPausa.SetActive(true);
        Time.timeScale = 0;
        juegoPausado = true;
    }

    public void Jugar()
    {
        Time.timeScale = 1;
        juegoPausado = false;

        // Transición con FadeManager
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.CambiarEscena("Nivel 1"); // Usa el nombre exacto de tu escena del juego
        }
        else
        {
            SceneManager.LoadScene("Nivel 1");
        }
    }

    public void MenuPrincipalFunc()
    {
        Time.timeScale = 1;
        juegoPausado = false;

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.CambiarEscena("Menú"); // Usa el nombre exacto de la escena del menú
        }
        else
        {
            SceneManager.LoadScene("Menú");
        }
    }

    public void Salir()
    {
        Debug.Log("Saliendo del Juego...");
        Application.Quit();
    }

    public void Reiniciar()
    {
        if (menuPausa != null) menuPausa.SetActive(false);
        if (opcionesPanel != null) opcionesPanel.SetActive(false);
        Time.timeScale = 1;
        juegoPausado = false;

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.CambiarEscena("Nivel 1"); // Usa el nombre exacto de tu escena
        }
        else
        {
            SceneManager.LoadScene("Nivel 1");
        }
    }

    public void AbrirOpciones()
    {
        if (opcionesPanel != null)
            opcionesPanel.SetActive(true);
    }

    public void CerrarOpciones()
    {
        if (opcionesPanel != null)
            opcionesPanel.SetActive(false);
    }
}
