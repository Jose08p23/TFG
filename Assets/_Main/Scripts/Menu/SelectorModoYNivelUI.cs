using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectorModoYNivelUI : MonoBehaviour
{
    [Header("Pantallas")]
    public GameObject panelSelectorModo;
    public GameObject panelSelectorNiveles;
    public GameObject panelDetalleNivel;

    [Header("Botones de modo")]
    public Button botonNormal;
    public Button botonLava;
    public Button botonHitless;

    [Header("Generación dinámica")]
    public Transform contenedorBotonesNiveles;
    public GameObject prefabBotonNivel;

    [Header("Detalle de nivel")]
    public TextMeshProUGUI textoTituloDetalle;
    public TextMeshProUGUI textoPorcentajeDetalle;
    public TextMeshProUGUI textoTiempoDetalle;
    public Button botonJugar;
    public Button botonVolver;

    private ProgresoJugador progreso;
    private ModoJuegoData modoActual;
    private string nombreModoActual;

    // Corregido para que coincida con los nombres reales de las escenas
    private string[] nombreBaseEscena = { "Level ", "LavaLevel ", "HitlessLevel " };

    private int nivelSeleccionadoIndex;
    private string escenaSeleccionada;

    void Start()
    {
        Debug.Log("[SelectorModoYNivelUI] Start ejecutado.");
        progreso = ProgresoJugador.Cargar();

        // Desactiva todos los paneles al iniciar
        panelSelectorModo.SetActive(false);
        panelSelectorNiveles.SetActive(false);
        panelDetalleNivel.SetActive(false);

        Debug.Log("[SelectorModoYNivelUI] Asignando listeners a botones de modo.");
        botonNormal.onClick.AddListener(() => MostrarNiveles("Normal"));
        botonLava.onClick.AddListener(() => MostrarNiveles("Lava"));
        botonHitless.onClick.AddListener(() => MostrarNiveles("Hitless"));
        botonVolver.onClick.AddListener(() => VolverAModos());
    }

    void MostrarNiveles(string modo)
    {
        Debug.Log($"[MostrarNiveles] Modo seleccionado: {modo}");
        nombreModoActual = modo;

        switch (modo)
        {
            case "Normal": modoActual = progreso.normal; break;
            case "Lava": modoActual = progreso.lava; break;
            case "Hitless": modoActual = progreso.hitless; break;
            default:
                Debug.LogError("[MostrarNiveles] Modo no reconocido: " + modo);
                return;
        }

        panelSelectorModo.SetActive(false);
        panelDetalleNivel.SetActive(false);
        panelSelectorNiveles.SetActive(true);

        Debug.Log("[MostrarNiveles] Limpiando botones anteriores...");
        foreach (Transform child in contenedorBotonesNiveles)
        {
            Destroy(child.gameObject);
        }

        Debug.Log($"[MostrarNiveles] Generando {modoActual.niveles.Count} botones de nivel para el modo {modo}.");

        for (int i = 0; i < modoActual.niveles.Count; i++)
        {
            Debug.Log($"[MostrarNiveles] Generando botón para Nivel {i + 1}");
            GameObject nuevoBoton = Instantiate(prefabBotonNivel, contenedorBotonesNiveles);
            if (nuevoBoton == null)
            {
                Debug.LogError("[MostrarNiveles] Error al instanciar el prefab del botón de nivel.");
                continue;
            }

            BotonNivelUI boton = nuevoBoton.GetComponent<BotonNivelUI>();
            if (boton == null)
            {
                Debug.LogError("[MostrarNiveles] El prefab no tiene el componente BotonNivelUI.");
                continue;
            }

            bool desbloqueado = i == 0 || modoActual.niveles[i - 1].completado;
            var datos = modoActual.niveles[i];

            int indiceEscena = GetIndiceModo(modo);
            if (indiceEscena < 0 || indiceEscena >= nombreBaseEscena.Length)
            {
                Debug.LogError($"[MostrarNiveles] Índice de modo fuera de rango: {indiceEscena}");
                continue;
            }

            string escena = nombreBaseEscena[indiceEscena] + (i + 1);
            Debug.Log($"[MostrarNiveles] Escena objetivo: {escena}");

            if (string.IsNullOrEmpty(escena))
            {
                Debug.LogError($"[MostrarNiveles] Escena vacía en nivel {i + 1} del modo {modo}");
                continue;
            }

            Debug.Log($"[MostrarNiveles] Configurando botón: {escena} | desbloqueado: {desbloqueado} | porcentaje: {datos.PorcentajeCompletado} | tiempo: {datos.mejorTiempo}");

            boton.Configurar(i, $"LEVEL {i + 1}", desbloqueado, datos.PorcentajeCompletado, datos.mejorTiempo, escena, MostrarDetalleNivel);
        }
    }

    void MostrarDetalleNivel(int index, string nombreEscena, float porcentaje, float tiempo)
    {
        Debug.Log($"[MostrarDetalleNivel] Nivel seleccionado: {index + 1}, Escena: {nombreEscena}, Porcentaje: {porcentaje}, Tiempo: {tiempo}");

        nivelSeleccionadoIndex = index;
        escenaSeleccionada = nombreEscena;

        panelDetalleNivel.SetActive(true);
        panelSelectorNiveles.SetActive(false);

        textoTituloDetalle.text = $"{nombreModoActual.ToUpper()} - LEVEL {index + 1}";
        textoPorcentajeDetalle.text = $"Progress: {porcentaje:0}%";
        textoTiempoDetalle.text = tiempo > 0 ? $"Best Time: {FormatearTiempo(tiempo)}" : "Best Time: --";

        botonJugar.onClick.RemoveAllListeners();
        botonJugar.onClick.AddListener(() => JugarNivel());
    }

    void JugarNivel()
    {
        Debug.Log($"[JugarNivel] Cargando escena: {escenaSeleccionada}");
        if (FadeManager.Instance != null)
        {
            Debug.Log("[JugarNivel] Usando FadeManager para cambiar de escena.");
            FadeManager.Instance.CambiarEscena(escenaSeleccionada);
        }
        else
        {
            Debug.Log("[JugarNivel] Usando SceneManager para cargar escena directamente.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(escenaSeleccionada);
        }
    }

    void VolverAModos()
    {
        Debug.Log("[VolverAModos] Regresando al menú de selección de modo.");
        panelDetalleNivel.SetActive(false);
        panelSelectorNiveles.SetActive(false);
        panelSelectorModo.SetActive(true);
    }

    int GetIndiceModo(string modo)
    {
        switch (modo)
        {
            case "Normal": return 0;
            case "Lava": return 1;
            case "Hitless": return 2;
            default:
                Debug.LogError("[GetIndiceModo] Modo no reconocido: " + modo);
                return 0;
        }
    }

    string FormatearTiempo(float t)
    {
        int m = Mathf.FloorToInt(t / 60);
        int s = Mathf.FloorToInt(t % 60);
        return $"{m:00}:{s:00}";
    }

    public void AbrirSelectorModo()
    {
        Debug.Log("[SelectorModoYNivelUI] Botón Mode pulsado → Mostrando panel de modos");
        panelSelectorModo.SetActive(true);
        panelSelectorNiveles.SetActive(false);
        panelDetalleNivel.SetActive(false);
    }
    
    public void VolverAlInicio()
{
    Debug.Log("[SelectorModoYNivelUI] Volviendo al estado inicial (solo el botón Mode visible)");

    panelSelectorModo.SetActive(false);
    panelSelectorNiveles.SetActive(false);
    panelDetalleNivel.SetActive(false);
}


}
