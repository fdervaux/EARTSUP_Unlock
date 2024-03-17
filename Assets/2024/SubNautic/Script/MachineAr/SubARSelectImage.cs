using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SubARSelectImage : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Image manager on the AR Session Origin")]
    ARTrackedImageManager m_ImageManager;

    public ARTrackedImageManager ImageManager
    {
        get => m_ImageManager;
        set => m_ImageManager = value;
    }

    [SerializeField]
    [Tooltip("Reference Image Library")]
    XRReferenceImageLibrary m_ImageLibrary;

    public XRReferenceImageLibrary ImageLibrary
    {
        get => m_ImageLibrary;
        set => m_ImageLibrary = value;
    }

    [SerializeField] private List<GameObject> _prefabs;

    private Dictionary<Guid, ImageTracked> _spawnedPrefabs = new Dictionary<Guid, ImageTracked>();
    private Dictionary<Guid, GameObject> _prefabsDictionary = new Dictionary<Guid, GameObject>();

    private Craft _currentCraft;
    public Craft CurrentCraft => _currentCraft;


    public void UpdatePrefabsDictionary()
    {
        if (m_ImageLibrary.count != _prefabs.Count)
        {
            Debug.LogError("The number of images in the library and the number of prefabs are different");
            return;
        }

        for (int i = 0; i < m_ImageLibrary.count; i++)
        {
            print("Add");
            _prefabsDictionary.Add(m_ImageLibrary[i].guid, _prefabs[i]);
        }
    }


    void OnEnable()
    {
        UpdatePrefabsDictionary();
        m_ImageManager.trackedImagesChanged += ImageManagerOnTrackedImagesChanged;
    }

    void OnDisable()
    {
        m_ImageManager.trackedImagesChanged -= ImageManagerOnTrackedImagesChanged;

        foreach (var spawnedPrefab in _spawnedPrefabs.Values)
        {
            Destroy(spawnedPrefab._prefab);
        }
        _spawnedPrefabs.Clear();
    }

    void ImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        // added, spawn prefab
        foreach (ARTrackedImage image in obj.added)
        {
            if (_prefabsDictionary.TryGetValue(image.referenceImage.guid, out GameObject prefab))
            {
                GameObject spawnedPrefab = Instantiate(prefab, image.transform.position, image.transform.rotation);
                _spawnedPrefabs.Add(image.referenceImage.guid, new ImageTracked(spawnedPrefab));
            }
        }

        // updated, set prefab position and rotation
        foreach (ARTrackedImage image in obj.updated)
        {
            // image is tracking or tracking with limited state, show visuals and update it's position and rotation
            if (image.trackingState == TrackingState.Tracking)
            {
                if (_spawnedPrefabs.TryGetValue(image.referenceImage.guid, out ImageTracked spawnedPrefab))
                {
                    spawnedPrefab._prefab.SetActive(true);
                    spawnedPrefab._prefab.transform.position = spawnedPrefab._positionFilter.Update(image.transform.position);
                    Vector3 up = spawnedPrefab._upFilter.Update(image.transform.up);
                    Vector3 forward = spawnedPrefab._forwardFilter.Update(image.transform.forward);
                    Quaternion rotation = Quaternion.LookRotation(forward, up);
                    spawnedPrefab._prefab.transform.rotation = rotation;
                }
            }
            // image is no longer tracking, disable visuals TrackingState.Limited TrackingState.None
            else
            {
                if (_spawnedPrefabs.TryGetValue(image.referenceImage.guid, out ImageTracked spawnedPrefab))
                {
                    spawnedPrefab._prefab.SetActive(false);
                    spawnedPrefab._positionFilter.Reset();
                    spawnedPrefab._upFilter.Reset();
                    spawnedPrefab._forwardFilter.Reset();
                }
            }
        }

        // removed, destroy spawned instance
        foreach (ARTrackedImage image in obj.removed)
        {
            if (_spawnedPrefabs.TryGetValue(image.referenceImage.guid, out ImageTracked spawnedPrefab))
            {
                Destroy(spawnedPrefab._prefab);
                _spawnedPrefabs.Remove(image.referenceImage.guid);
            }
        }
    }

    public int NumberOfTrackedImages()
    {
        return _spawnedPrefabs.Count;
    }
}
