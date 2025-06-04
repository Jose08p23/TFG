using System.Collections;
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

    // Timer
    private float timer;
    public float Timer { get { return timer; } }

    // Volumen
    public float volumenMusica = 1.0f;
    public float volumenEfectos = 1.0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Cargar valores de volumen si existen
            volumenMusica = PlayerPrefs.GetFloat("VolumenMusica", 1.0f);
            volumenEfectos = PlayerPrefs.GetFloat("VolumenEfectos", 1.0f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hud = FindObjectOfType<HUD>();



        if (hud != null)
        {
            hud.ActualizarPuntos(puntosTotales);
            hud.ActualizarTimer(timer);
            hud.ActualizarTimerStatic(timer);
        }

        // Solo reiniciar si NO estamos en DeathMenu ni WinMenu
        if (scene.name != "DeathMenu" && scene.name != "WinMenu")
        {
            ReiniciarValores();
        }
    }

        void Update()
        {
            timer += Time.deltaTime;

            if (hud != null)
            {
                hud.ActualizarTimer(timer);
            }
        }

    public void SumarPuntos(int puntosASumar)
    {
        puntosTotales += puntosASumar;
        if (hud != null)
        {
            hud.ActualizarPuntos(puntosTotales);
        }
    }

    public void PerderVida()
    {
        Debug.Log("Vida perdida. Vidas restantes: " + vidas);
        if (vidas <= 0) return;

        vidas--;

        if (hud != null && vidas >= 0 && vidas < hud.vidas.Length)
        {
            hud.DesactivarVida(vidas);
        }

        if (vidas <= 0)
        {
            SceneTracker.EscenaAnterior = SceneManager.GetActiveScene().name;

            Debug.Log("Cambiando a escena de muerte...");

            SceneManager.LoadScene("DeathMenu"); // Usa el nombre exacto de tu escena
        }
    }


    public bool RecuperarVida()
    {
        if (vidas >= vidasIniciales)
        {
            return false;
        }

        if (hud != null)
        {
            hud.ActivarVida(vidas);
        }

        vidas++;
        return true;
    }

    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(1);
    }

   private void ReiniciarValores()
{
    // Detectar escena y establecer vidasIniciales
    string nombreEscena = SceneManager.GetActiveScene().name;
    if (nombreEscena.StartsWith("Hitless"))
    {
        vidasIniciales = 1;
        Debug.Log("[GameManager] Modo Hitless detectado. Vidas iniciales: 1");
    }
    else
    {
        vidasIniciales = 3;
        Debug.Log("[GameManager] Modo normal. Vidas iniciales: 3");
    }
    vidas = vidasIniciales;

    puntosTotales = 0;
    timer = 0f;

    Debug.Log("[GameManager] Valores reiniciados. Vidas: " + vidasIniciales + ", Puntos: 0, Timer: 0");

    if (hud != null)
    {
        hud.ActualizarPuntos(puntosTotales);
        for (int i = 0; i < hud.vidas.Length; i++)
        {
            hud.vidas[i].SetActive(true);
        }
        hud.ActualizarTimer(timer);
        hud.ActualizarTimerStatic(timer);
    }
}


    public void GuardarVolumenes()
    {
        PlayerPrefs.SetFloat("VolumenMusica", volumenMusica);
        PlayerPrefs.SetFloat("VolumenEfectos", volumenEfectos);
        PlayerPrefs.Save();
    }
}
