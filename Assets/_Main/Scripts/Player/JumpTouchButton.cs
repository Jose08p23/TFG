using UnityEngine;
using UnityEngine.EventSystems;

public class JumpTouchButton : MonoBehaviour, IPointerDownHandler
{
    public PlayerController player;

    public void OnPointerDown(PointerEventData eventData)
    {
        player.PressJump();
    }
}
