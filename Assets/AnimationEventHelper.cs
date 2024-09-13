using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    public void OnHidingAnimationEnd()
    {
        // Send a message up to the parent object
        SendMessageUpwards("OnHidingAnimationEnd", SendMessageOptions.DontRequireReceiver);
    }
}
