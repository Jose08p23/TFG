using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float parallaxSpeed = 0.5f; // Velocidad del efecto parallax
    public float smoothTime = 0.2f; // Tiempo de suavizado

    private float startY; // Posición inicial del fondo
    private float velocityY = 0f; // Velocidad para SmoothDamp

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (player != null)
        {
            // Objetivo suavizado con SmoothDamp
            float targetY = startY + (player.position.y * parallaxSpeed);
            float smoothY = Mathf.SmoothDamp(transform.position.y, targetY, ref velocityY, smoothTime);

            // Aplicar el movimiento suavizado
            transform.position = new Vector3(transform.position.x, smoothY, transform.position.z);
        }
    }
}
