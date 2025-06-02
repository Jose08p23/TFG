using UnityEngine;
using TMPro;

public class TextoVictoria : MonoBehaviour
{
    public TextMeshProUGUI textoVictoria;

    void Start()
    {
        string nivelAnterior = SceneTracker.EscenaAnterior;

        if (!string.IsNullOrEmpty(nivelAnterior))
        {
            textoVictoria.text = nivelAnterior.ToUpper() + " COMPLETE";
        }
        else
        {
            textoVictoria.text = "NIVEL COMPLETE";
        }
    }
}
