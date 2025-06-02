using UnityEngine;
using UnityEngine.UI;

public class DeleteSaveManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject confirmationPanel;
    public Button deleteButton;
    public Button yesButton;
    public Button noButton;

    void Start()
    {
        // Asegura que el panel está oculto al iniciar
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);

        // Asigna eventos a los botones
        deleteButton.onClick.AddListener(OpenConfirmation);
        yesButton.onClick.AddListener(DeleteSaveData);
        noButton.onClick.AddListener(CloseConfirmation);
    }

    void OpenConfirmation()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(true);
    }

    void CloseConfirmation()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
    }

    void DeleteSaveData()
{
    Debug.Log("[DeleteSaveManager] Deleting player save data...");
    ProgresoJugador.Borrar();
    

    // Recarga la escena actual con FadeManager
    string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    if (FadeManager.Instance != null)
    {
        FadeManager.Instance.CambiarEscena(currentScene);
    }
    else
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }
}

}
