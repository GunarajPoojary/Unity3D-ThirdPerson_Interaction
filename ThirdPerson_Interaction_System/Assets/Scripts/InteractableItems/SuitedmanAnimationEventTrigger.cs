using System;
using UnityEngine;

public class SuitedmanAnimationEventTrigger : MonoBehaviour
{
    public Action OnInteractAnimationEndAction { private get; set; } = delegate { };
    
    public void OnInteractAnimationEnd()
    {
        OnInteractAnimationEndAction?.Invoke();
    }
}