using UnityEngine;

public class XRCameraPicker : MonoBehaviour
{
    [SerializeField]
    private GameObject HololensCamera;

    [SerializeField]
    private GameObject ARFoundationCamera;

    [SerializeField]
    private GameObject EditorCamera;

    private void Awake()
    {
        var targetCamera = EditorCamera;

#if UNITY_WSA
        targetCamera = HololensCamera;
#elif UNITY_ANDROID || UNITY_IOS
        targetCamera = ARFoundationCamera;
#elif !UNITY_EDITOR
        Debug.LogError("Unexpected platform for XRCameraPicker.");
#endif

        Instantiate(targetCamera);
    }
}
