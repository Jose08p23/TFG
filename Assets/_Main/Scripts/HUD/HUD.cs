using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI puntos;
    public GameObject[] vidas;
    // Campo para el timer que se actualiza cada centésima de segundo
    public TextMeshProUGUI timerText;
    // Nuevo campo para el timer que solo se actualiza al cambiar de escena
    public TextMeshProUGUI timerStaticText;

    void Start()
    {
        // Puedes inicializar aquí si lo requieres
    }

    void Update()
    {
        // Actualiza el marcador de puntos
        puntos.text = GameManager.Instance.PuntosTotales.ToString();
        // Solo actualizamos timerText cada frame (el campo que muestra el timer en tiempo real)
        // No se actualiza timerStaticText aquí.
    }

    public void ActualizarPuntos(int puntosTotales){
        puntos.text = puntosTotales.ToString();
    }

    // Método para actualizar el timer que se actualiza continuamente
    public void ActualizarTimer(float t) {
        if(timerText != null) {
            int minutes = Mathf.FloorToInt(t / 60);
            int seconds = Mathf.FloorToInt(t % 60);
            int hundredths = Mathf.FloorToInt((t * 100) % 100);
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
        }
    }

    // Nuevo método para actualizar el timer que se actualiza solo al cambiar de escena
    public void ActualizarTimerStatic(float t) {
        if(timerStaticText != null) {
            int minutes = Mathf.FloorToInt(t / 60);
            int seconds = Mathf.FloorToInt(t % 60);
            int hundredths = Mathf.FloorToInt((t * 100) % 100);
            timerStaticText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
        }
    }

    public void DesactivarVida(int indice){
        vidas[indice].SetActive(false);
    }

    public void ActivarVida(int indice){
        vidas[indice].SetActive(true);
    }
}
