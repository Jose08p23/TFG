using UnityEngine;
using UnityEngine.EventSystems;

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType { Left, Right }
    public ButtonType buttonType;
    public PlayerController player;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonType == ButtonType.Left)
            player.SetHorizontalInput(-1f);
        else
            player.SetHorizontalInput(1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        player.SetHorizontalInput(0f);
    }
}
