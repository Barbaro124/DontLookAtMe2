using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    // Called by Animation Event
    public void OnHidingAnimationEnd()
    {
        // Send a message up to the parent object
        SendMessageUpwards("HandleHidingAnimationEnd", SendMessageOptions.DontRequireReceiver);
    }
}
