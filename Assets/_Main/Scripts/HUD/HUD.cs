using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI puntos;
    public GameObject[] vidas;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerStaticText;

    // ✅ Referencia al botón que quieres ocultar si venimos del nivel 10
    public GameObject botonFinal;

    void Start()
    {
        // ✅ Comprobación para ocultar el botón si la última escena termina en "10"
        if (botonFinal != null && SceneTracker.EscenaAnterior != null)
        {
            if (SceneTracker.EscenaAnterior.EndsWith("10"))
            {
                Debug.Log("[HUD] Escena anterior termina en 10. Ocultando botón final.");
                botonFinal.SetActive(false);
            }
            else
            {
                botonFinal.SetActive(true);
            }
        }
    }

    void Update()
    {
        puntos.text = GameManager.Instance.PuntosTotales.ToString();
    }

    public void ActualizarPuntos(int puntosTotales)
    {
        puntos.text = puntosTotales.ToString();
    }

    public void ActualizarTimer(float t)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(t / 60);
            int seconds = Mathf.FloorToInt(t % 60);
            int hundredths = Mathf.FloorToInt((t * 100) % 100);
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
        }
    }

    public void ActualizarTimerStatic(float t)
    {
        if (timerStaticText != null)
        {
            int minutes = Mathf.FloorToInt(t / 60);
            int seconds = Mathf.FloorToInt(t % 60);
            int hundredths = Mathf.FloorToInt((t * 100) % 100);
            timerStaticText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
        }
    }

    public void DesactivarVida(int indice)
    {
        vidas[indice].SetActive(false);
    }

    public void ActivarVida(int indice)
    {
        vidas[indice].SetActive(true);
    }
}
