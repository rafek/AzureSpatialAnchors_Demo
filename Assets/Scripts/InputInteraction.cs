using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


#if WINDOWS_UWP || UNITY_WSA
using UnityEngine.XR.WSA.Input;
#endif

public class InputInteraction : MonoBehaviour
{
    public event Action<Vector3> OnSelectObjectInteraction;

#if UNITY_ANDROID || UNITY_IOS
    private ARRaycastManager arRaycastManager;
#endif

    private void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        arRaycastManager = FindObjectOfType<ARRaycastManager>();

        if (arRaycastManager == null)
        {
            Debug.Log("Missing ARRaycastManager in the scene.");
        }
#endif

#if WINDOWS_UWP || UNITY_WSA
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;
#endif
    }

    private void Update()
    {
        TriggerInteractions();
    }

    private void OnDestroy()
    {
#if WINDOWS_UWP || UNITY_WSA
        InteractionManager.InteractionSourcePressed -= InteractionManager_InteractionSourcePressed;
#endif
    }

#if WINDOWS_UWP || UNITY_WSA
    private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs obj)
    {
        if (obj.pressType == InteractionSourcePressType.Select)
        {
            OnSelectInteraction();
        }
    }

    private void OnSelectInteraction()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            OnSelectObjectInteraction?.Invoke(hit.point);
        }
    }
#endif

    private void TriggerInteractions()
    {
        // TODO: gaze interactions

        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            // TODO: ignore UI events

            OnTouchInteraction(touch);
        }
    }

    private void OnTouchInteraction(Touch touch)
    {
        if (touch.phase == TouchPhase.Ended)
        {
            OnTouchInteractionEnded(touch);
        }
    }

    private void OnTouchInteractionEnded(Touch touch)
    {
#if UNITY_ANDROID || UNITY_IOS
        var arRaycastHits = new List<ARRaycastHit>();

        if (arRaycastManager.Raycast(touch.position, arRaycastHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneEstimated) && arRaycastHits.Count > 0)
        {
            var hit = arRaycastHits[0];

            OnSelectObjectInteraction?.Invoke(hit.pose.position);
        }
#elif WINDOWS_UWP || UNITY_WSA
#endif
    }
}
