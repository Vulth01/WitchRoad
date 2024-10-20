using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarriageSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float spawnTimer;
    [SerializeField] private float sTimer;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private float zOffset;
    [SerializeField] private int instanceCount;
    [SerializeField] private GameObject prefab1;
    [SerializeField] private GameObject prefab2;

    private void Awake()
    {

        InteractionManager interactionManager = FindObjectOfType<InteractionManager>();
        if (interactionManager != null) interactionManager.OnNextRoom += OnNextRoom;
    }
    void OnNextRoom()
    {
        DoSpawn();
    }
    private void DoSpawn()
    {
        instanceCount++;
        var pos = gameObject.transform.position;
        var spawnPos = new Vector3(pos.x + (xOffset * instanceCount), pos.y + (yOffset * instanceCount), pos.z + (zOffset * instanceCount));
        Destroy(prefab1);
        prefab1 = prefab2;
        prefab2 = Instantiate(prefab, spawnPos, Quaternion.identity, gameObject.transform);
    }

}
