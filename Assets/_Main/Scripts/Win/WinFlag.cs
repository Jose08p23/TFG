using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinFlag : MonoBehaviour
{
    // Campo público para definir el delay en segundos desde el Inspector.
    public float delay = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detectado con: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador detectado, cargando escena 3 con fade en " + delay + " segundos");
            StartCoroutine(LoadSceneAfterDelay(delay));
        }
    }
private IEnumerator LoadSceneAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);

    SceneTracker.EscenaAnterior = SceneManager.GetActiveScene().name;

    if (FadeManager.Instance != null)
    {
        FadeManager.Instance.CambiarEscena("WinMenu"); 
    }
    else
    {
        SceneManager.LoadScene("WinMenu");
    }
}

}
