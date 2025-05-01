using UnityEngine;
using UnityEngine.UI;

public class OpcionesMenu : MonoBehaviour
{
    public Slider sliderMusica;
    public Slider sliderEfectos;
    public AudioSource audioMusica;  // Referencia al MusicManager
    public AudioSource audioEfectos; // Puede ser temporal solo para prueba

    void Start()
    {
        // Establecer valores iniciales desde GameManager
        if (GameManager.Instance != null)
        {
            sliderMusica.value = GameManager.Instance.volumenMusica;
            sliderEfectos.value = GameManager.Instance.volumenEfectos;

            AplicarVolumen();
        }

        // Añadir listeners si no están ya desde el Inspector
        sliderMusica.onValueChanged.AddListener(CambiarVolumenMusica);
        sliderEfectos.onValueChanged.AddListener(CambiarVolumenEfectos);
    }

    public void CambiarVolumenMusica(float valor)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.volumenMusica = valor;
            GameManager.Instance.GuardarVolumenes();
        }

        if (audioMusica != null)
        {
            audioMusica.volume = valor;
        }
    }

    public void CambiarVolumenEfectos(float valor)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.volumenEfectos = valor;
            GameManager.Instance.GuardarVolumenes();
        }

        if (audioEfectos != null)
        {
            audioEfectos.volume = valor;
        }
    }

    private void AplicarVolumen()
    {
        if (audioMusica != null)
        {
            audioMusica.volume = GameManager.Instance.volumenMusica;
        }

        if (audioEfectos != null)
        {
            audioEfectos.volume = GameManager.Instance.volumenEfectos;
        }
    }

    public void CerrarOpciones()
    {
        gameObject.SetActive(false);
    }
}
