using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAutoBuilder : MonoBehaviour
{
    protected void Awake()
    {
        Terrain terrain = Terrain.activeTerrain;
        Vector3 terrainPosition = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        var bounds = new Bounds(
            terrainPosition + new Vector3(terrainSize.x / 2, 0f, terrainSize.z / 2),
            terrainSize);

        var markups = new List<NavMeshBuildMarkup>();
        var sources = new List<NavMeshBuildSource>();

        NavMeshBuilder.CollectSources(transform,
            LayerMask.GetMask("Ground"),
            NavMeshCollectGeometry.PhysicsColliders,
            NavMesh.GetAreaFromName("Walkable"),
            markups,
            sources);

        var settings = NavMesh.GetSettingsByIndex(0);
        var data = NavMeshBuilder.BuildNavMeshData(
            settings,
            sources,
            bounds,
            terrainPosition,
            Quaternion.identity);

        NavMesh.AddNavMeshData(data);
    }

}

