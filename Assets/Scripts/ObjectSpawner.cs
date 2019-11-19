using Microsoft.Azure.SpatialAnchors.Unity;
using System;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public event Action<GameObject> OnObjectSpawned;

    [SerializeField]
    private GameObject anchoredObjectPrefab;

    private InputInteraction inputInteraction;

    private void Awake()
    {
        inputInteraction = GetComponent<InputInteraction>();

        inputInteraction.OnSelectObjectInteraction += InputInteraction_OnSelectObjectInteraction;
    }

    private void OnDestroy()
    {
        if (inputInteraction != null)
        {
            inputInteraction.OnSelectObjectInteraction -= InputInteraction_OnSelectObjectInteraction;
        }
    }

    private void InputInteraction_OnSelectObjectInteraction(Vector3 position)
    {
        var spawnedObject = SpawnAt(position);

        OnObjectSpawned?.Invoke(spawnedObject);
    }

    private GameObject SpawnAt(Vector3 position)
    {
        var newAnchoredGameObject = Instantiate(anchoredObjectPrefab, position, Quaternion.identity);

        newAnchoredGameObject.CreateNativeAnchor();
        newAnchoredGameObject.AddComponent<CloudNativeAnchor>();

        return newAnchoredGameObject;
    }
}
