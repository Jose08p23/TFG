using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotonNivelUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public Button boton;
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textoPorcentaje;
    public TextMeshProUGUI textoTiempo;

    private string nombreEscena;
    private int indiceNivel;
    private System.Action<int, string, float, float> onSeleccionarNivel;

    public void Configurar(int nivelIndex, string nombreNivel, bool desbloqueado, float porcentaje, float tiempo, string escena, System.Action<int, string, float, float> callback)
    {
        Debug.Log($"[BotonNivelUI] Configurando botón para {nombreNivel}");

        indiceNivel = nivelIndex;
        nombreEscena = escena;
        onSeleccionarNivel = callback;

        textoNivel.text = nombreNivel;
        textoPorcentaje.text = desbloqueado ? $"{porcentaje:0}%" : "--";
        textoTiempo.text = desbloqueado && tiempo > 0 ? FormatearTiempo(tiempo) : "--";

        Debug.Log($"[BotonNivelUI] Estado: {(desbloqueado ? "Desbloqueado" : "Bloqueado")} | Porcentaje: {porcentaje} | Tiempo: {tiempo}");

        boton.interactable = desbloqueado;

        boton.onClick.RemoveAllListeners();
        if (desbloqueado)
        {
            Debug.Log($"[BotonNivelUI] Añadiendo listener para cargar escena: {nombreEscena}");
            boton.onClick.AddListener(() =>
            {
                Debug.Log($"[BotonNivelUI] Botón nivel {nombreNivel} pulsado. Ejecutando callback...");
                onSeleccionarNivel?.Invoke(indiceNivel, nombreEscena, porcentaje, tiempo);
            });
        }
        else
        {
            Debug.Log($"[BotonNivelUI] Botón desactivado por estar bloqueado.");
        }
    }

    private string FormatearTiempo(float t)
    {
        int minutos = Mathf.FloorToInt(t / 60);
        int segundos = Mathf.FloorToInt(t % 60);
        return $"{minutos:00}:{segundos:00}";
    }
}
// Este script es un componente de UI que representa un botón para un nivel específico en el juego.
// Permite configurar el texto del botón, el estado de desbloqueo y manejar la lógica al pulsar el botón.