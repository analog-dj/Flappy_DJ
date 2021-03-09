using UnityEngine;

public class DeployTerrain : MonoBehaviour
{
    public GameObject terrainPrefab;
    public Transform respawn;

    void Start()
    {
        GameObject.Instantiate(terrainPrefab, respawn.position, Quaternion.identity);
    }
}