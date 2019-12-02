using Microsoft.Azure.SpatialAnchors.Unity;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    [SerializeField]
    private SpatialAnchorManager spatialAnchorManager;

    private ObjectSpawner objectSpawner;

    private void Awake()
    {
        objectSpawner = GetComponent<ObjectSpawner>();

        objectSpawner.OnObjectSpawned += ObjectSpawner_OnObjectSpawned;
    }

    private async void Start()
    {
        try
        {
            await spatialAnchorManager.StartSessionAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }
    }

    private void OnDestroy()
    {
        if (objectSpawner != null)
        {
            objectSpawner.OnObjectSpawned -= ObjectSpawner_OnObjectSpawned;
        }

        if (spatialAnchorManager != null)
        {
            spatialAnchorManager.DestroySession();
        }
    }

    private async void ObjectSpawner_OnObjectSpawned(GameObject spawnedObject)
    {
        await OnObjectSpawned(spawnedObject);   
    }

    private async Task OnObjectSpawned(GameObject spawnedObject)
    {
        var cloudNativeAnchor = spawnedObject.GetComponent<CloudNativeAnchor>();

        if (cloudNativeAnchor.CloudAnchor == null)
        {
            cloudNativeAnchor.NativeToCloud();
        }

        var cloudSpatialAnchor = cloudNativeAnchor.CloudAnchor;

        // TODO: add expirtaion, add properties

        while (!spatialAnchorManager.IsReadyForCreate)
        {
            await Task.Delay(300);

            // TODO: display RecommendedForCreateProgress feedback

            var createProgress = spatialAnchorManager.SessionStatus.RecommendedForCreateProgress;

            Debug.Log($"Create Progress: {createProgress}");
        }

        try
        {
            await spatialAnchorManager.CreateAnchorAsync(cloudSpatialAnchor);


            // TODO: save that anchor to sharing service if successful

            Debug.Log("Anchor saved: " + cloudSpatialAnchor.Identifier);
        }
        catch (Exception ex)
        {
            // TODO: handle exception
        }
    }
}
