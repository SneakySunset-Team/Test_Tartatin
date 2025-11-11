using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TTButton : Button
{
    [SerializeField]
    public UnityEvent OnPressEvent;
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (interactable)
        {
            OnPressEvent?.Invoke();
        }
    }
}
