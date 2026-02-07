using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animator;

    // Called when mouse enters button
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetTrigger("HoverTrigger");
    }

    // Called when mouse leaves button
    public void OnPointerExit(PointerEventData eventData)
    {
        // optional: return to idle
        animator.SetTrigger("IdleTrigger");
    }
}
