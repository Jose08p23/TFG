using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AplicarVolumenAudio : MonoBehaviour
{
    public enum TipoVolumen { Musica, Efectos }
    public TipoVolumen tipo;

    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();

        if (GameManager.Instance != null)
        {
            if (tipo == TipoVolumen.Musica)
                audio.volume = GameManager.Instance.volumenMusica;
            else
                audio.volume = GameManager.Instance.volumenEfectos;
        }
    }
}
