using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;

public class AStar : MonoBehaviour
{
    EntityManager entityManager;
    Vector2Int startCoors, endCoors;
    Entity startZone, endZone, currentSearchZone;
    ZoneComponent endZoneZC;
    List<Entity> unvisited = new List<Entity>();
    HashSet<Entity> visited = new HashSet<Entity>();
    void Awake()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    public void FindPath(Vector2Int startCoors, Vector2Int endCoors)
    {
        PathFinder.Instance.IsRunning = true;
        this.startCoors = startCoors;
        this.endCoors = endCoors;
        startZone = ZoneStore.Instance.Zones[startCoors];
        endZone = ZoneStore.Instance.Zones[endCoors];
        endZoneZC = entityManager.GetComponentData<ZoneComponent>(endZone);
        //--------------------reset 
        unvisited.Clear();
        visited.Clear();
        //--------------------run algorithm
        StartCoroutine(Algorithm());
    }
    IEnumerator Algorithm()
    {
        unvisited.Add(startZone);
        bool isRunning = true;
        while (unvisited.Count > 0 && isRunning)
        {
            currentSearchZone = unvisited[0];

            ZoneComponent currentSearchZC = entityManager.GetComponentData<ZoneComponent>(currentSearchZone);
            for (int i = 1; i < unvisited.Count; i++)
            {
                ZoneComponent otherZC = entityManager.GetComponentData<ZoneComponent>(unvisited[i]);
                if (otherZC.fCost <= currentSearchZC.fCost)
                {
                    currentSearchZone = unvisited[i];
                    currentSearchZC = otherZC;
                }
            }
            unvisited.Remove(currentSearchZone);
            visited.Add(currentSearchZone);
            currentSearchZC.isExplored = true;
            entityManager.SetComponentData<ZoneComponent>(currentSearchZone, currentSearchZC);

            if (currentSearchZone == endZone)
            {
                isRunning = false;
                continue;
            }
            //----------------------Set Explored Color
            if (currentSearchZone != startZone && currentSearchZone != endZone)
            {
                URPMaterialPropertyBaseColor baseColor = entityManager.GetComponentData<URPMaterialPropertyBaseColor>(currentSearchZone);
                baseColor.Value = StateColors.Instance.ExploredColor;
                entityManager.SetComponentData<URPMaterialPropertyBaseColor>(currentSearchZone, baseColor);
            }
            ExploreNeighbors8D(currentSearchZC);
        }
        List<Entity> paths = BuildPath();

        if (paths.Count <= 0)
        {
            //do something
        }
        else
        {
            foreach (Entity e in paths)
            {
                URPMaterialPropertyBaseColor baseColor = entityManager.GetComponentData<URPMaterialPropertyBaseColor>(e);
                baseColor.Value = StateColors.Instance.PathColor;
                entityManager.SetComponentData<URPMaterialPropertyBaseColor>(e, baseColor);
                yield return new WaitForSeconds(PathFinder.Instance.SearchSpeed);
            }
        }
        PathFinder.Instance.IsRunning = false;
        PathFinder.Instance.IsPreview = true;
    }
    ///<summary>
    ///Explore diagnal neighbors + top-bottom-right-left
    ///</summary>
    void ExploreNeighbors8D(ZoneComponent currentSearchZC)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y < -1; y++)
            {
                if (x == 0 && y == 0) continue;
                Vector2Int neighborCoor = new Vector2Int(currentSearchZC.coordinates.x + x, currentSearchZC.coordinates.y + y);

                if (!ZoneStore.Instance.Zones.ContainsKey(neighborCoor)) continue;
                Entity neighbor = ZoneStore.Instance.Zones[neighborCoor];
                ZoneComponent neighborZC = entityManager.GetComponentData<ZoneComponent>(neighbor);

                if (visited.Contains(neighbor) || !neighborZC.isWalkable) continue;
                float newCostToNeighbor = currentSearchZC.gCost + GetDistance(currentSearchZC, neighborZC);

                if (newCostToNeighbor >= neighborZC.gCost && unvisited.Contains(neighbor)) continue;
                neighborZC.gCost = newCostToNeighbor;
                neighborZC.hCost = GetDistance(neighborZC, endZoneZC);
                neighborZC.connectedTo = currentSearchZone;
                if (!unvisited.Contains(neighbor)) unvisited.Add(neighbor);
            }
        }
    }
    void ExploreNeighbors4D(ZoneComponent currentSearchZC)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborCoor = new Vector2Int(currentSearchZC.coordinates.x, currentSearchZC.coordinates.y) + dir;

            if (!ZoneStore.Instance.Zones.ContainsKey(neighborCoor)) continue;
            Entity neighbor = ZoneStore.Instance.Zones[neighborCoor];
            ZoneComponent neighborZC = entityManager.GetComponentData<ZoneComponent>(neighbor);

            if (visited.Contains(neighbor) || !neighborZC.isWalkable) continue;
            float newCostToNeighbor = currentSearchZC.gCost + GetDistance(currentSearchZC, neighborZC);

            if (newCostToNeighbor >= neighborZC.gCost && unvisited.Contains(neighbor)) continue;
            neighborZC.gCost = newCostToNeighbor;
            neighborZC.hCost = GetDistance(neighborZC, endZoneZC);
            neighborZC.connectedTo = currentSearchZone;
            if (!unvisited.Contains(neighbor)) unvisited.Add(neighbor);
        }
    }
    List<Entity> BuildPath()
    {
        List<Entity> path = new List<Entity>();
        Entity currentZone = endZone;
        ZoneComponent zc = entityManager.GetComponentData<ZoneComponent>(currentZone);
        while (zc.connectedTo != Entity.Null)
        {
            currentZone = zc.connectedTo;
            zc = entityManager.GetComponentData<ZoneComponent>(currentZone);
            if (zc.isStart) continue;
            path.Add(currentZone);
            zc.isPath = true;
            entityManager.SetComponentData<ZoneComponent>(currentZone, zc);
        }
        path.Reverse();
        return path;
    }
    float GetDistance(ZoneComponent first, ZoneComponent second)
    {
        float distX = Mathf.Abs(first.coordinates.x - second.coordinates.x);
        float distY = Mathf.Abs(first.coordinates.y - second.coordinates.y);
        //------> 14 == Sqrt(2) * 10 || 10 == straight line
        if (distX < distY) return 14 * distX + 10 * (distY - distX);
        return 14 * distY + 10 * (distX - distY);
    }
}