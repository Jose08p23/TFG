using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int PuntosTotales { get { return puntosTotales; } }
    private int puntosTotales;
    private int vidas;
    public int vidasIniciales = 3;

    public HUD hud;
    public static GameManager Instance { get; private set; }

    // Variable para el timer (en segundos)
    private float timer;
    public float Timer { get { return timer; } }

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        hud = FindObjectOfType<HUD>();

        if (hud != null) {
            hud.ActualizarPuntos(puntosTotales);
            // Actualiza el timer en el TextMeshPro que se actualiza continuamente
            hud.ActualizarTimer(timer);
            // Actualiza el nuevo campo que solo se actualiza al cambiar de escena
            hud.ActualizarTimerStatic(timer);
        }

        // Reinicia valores (puntos, vidas y timer) solo al volver a la escena del juego (buildIndex 1)
        if (scene.buildIndex == 1) {
            ReiniciarValores();
        }
    }

    void Update() {
        // Incrementa el timer con el tiempo transcurrido en cada frame
        timer += Time.deltaTime;
        // Actualiza la visualización del timer en el HUD (campo que se actualiza cada centésima de segundo)
        if (hud != null) {
            hud.ActualizarTimer(timer);
        }
    }

    public void SumarPuntos(int puntosASumar) {
        puntosTotales += puntosASumar;
        if (hud != null) {
            hud.ActualizarPuntos(puntosTotales);
        }
    }

    public void PerderVida() {
        if (vidas <= 0) return;

        vidas--;

        if (hud != null && vidas >= 0 && vidas < hud.vidas.Length) {
            hud.DesactivarVida(vidas);
        }

        if (vidas <= 0) {
            SceneManager.LoadScene(2); // Cargar escena de muerte
        }
    }

    public bool RecuperarVida() {
        if (vidas >= vidasIniciales) {
            return false;
        }
        if (hud != null) {
            hud.ActivarVida(vidas);
        }
        vidas++;
        return true;
    }

    public void ReiniciarJuego() {
        SceneManager.LoadScene(1); // Volver a la escena de juego
    }

    private void ReiniciarValores() {
        puntosTotales = 0; // Reinicia los puntos al volver a la escena 1
        vidas = vidasIniciales; 
        timer = 0f; // Reinicia el timer

        if (hud != null) {
            hud.ActualizarPuntos(puntosTotales);
            // Reactiva todas las vidas
            for (int i = 0; i < hud.vidas.Length; i++) {
                hud.vidas[i].SetActive(true);
            }
            // Actualiza ambos campos de timer: el continuo y el estático (solo al cambiar escena)
            hud.ActualizarTimer(timer);
            hud.ActualizarTimerStatic(timer);
        }
    }
}
