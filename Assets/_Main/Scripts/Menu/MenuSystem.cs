using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject menuPausa;
    public bool juegoPausado = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
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
        menuPausa.SetActive(false);
        Time.timeScale = 1;
        juegoPausado = false;
    }

    public void Pausar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0;
        juegoPausado = true;
    }

    // Ahora carga siempre la escena 1.
    public void Jugar()
    {
        SceneManager.LoadScene(1);
    }

    // Función que carga la escena 0 (Menú Principal).
    public void MenuPrincipalFunc()
    {
        SceneManager.LoadScene(0);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del Juego...");
        Application.Quit();
    }

      public void Reiniciar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1;
        juegoPausado = false;
        SceneManager.LoadScene(1);
    }
}
