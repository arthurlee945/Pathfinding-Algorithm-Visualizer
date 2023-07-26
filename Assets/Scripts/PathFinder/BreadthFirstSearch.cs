using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;

public class BreadthFirstSearch : MonoBehaviour
{
    EntityManager entityManager;
    Vector2Int startCoors, endCoors;
    Entity startZone, endZone, currentSearchZone;
    Dictionary<Vector2Int, Entity> reached = new Dictionary<Vector2Int, Entity>();
    Queue<Entity> eQueue = new Queue<Entity>();
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
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
        //------------------Start Prep
        eQueue.Clear();
        reached.Clear();
        //------------------Start Running the Algo
        // Algorithm();
        StartCoroutine(Algorithm());
    }

    /// <summary>
    /// <code>
    ///  procedure BFS(G, root) is
    ///    let Q be a queue
    ///    label root as explored
    ///    Q.enqueue(root)
    ///    while Q is not empty do
    ///        v := Q.dequeue()
    ///        if v is the goal then
    ///            return v
    ///        for all edges from v to w in G.adjacentEdges(v) do
    ///           if w is not labeled as explored then
    ///              label w as explored
    ///              w.parent := v
    ///              Q.enqueue(w)
    /// </code>
    /// </summary>

    IEnumerator Algorithm()
    {
        eQueue.Enqueue(startZone);
        reached.Add(startCoors, startZone);
        bool isRunning = true;
        while (eQueue.Count > 0 && isRunning)
        {
            currentSearchZone = eQueue.Dequeue();
            ZoneComponent currZC = entityManager.GetComponentData<ZoneComponent>(currentSearchZone);
            currZC.isExplored = true;
            entityManager.SetComponentData<ZoneComponent>(currentSearchZone, currZC);
            //----------------------Set Explored Color
            if (currentSearchZone != startZone && currentSearchZone != endZone)
            {
                URPMaterialPropertyBaseColor baseColor = entityManager.GetComponentData<URPMaterialPropertyBaseColor>(currentSearchZone);
                baseColor.Value = StateColors.Instance.ExploredColor;
                entityManager.SetComponentData<URPMaterialPropertyBaseColor>(currentSearchZone, baseColor);
            }
            //-----------------------
            ExploreNeighbors();
            yield return new WaitForSeconds(PathFinder.Instance.SearchSpeed);
            if (currentSearchZone != endZone) continue;
            //------------------End Algo
            isRunning = false;
        }
        //-------------display path
        foreach (Entity e in BuildPath())
        {
            URPMaterialPropertyBaseColor baseColor = entityManager.GetComponentData<URPMaterialPropertyBaseColor>(e);
            baseColor.Value = StateColors.Instance.PathColor;
            entityManager.SetComponentData<URPMaterialPropertyBaseColor>(e, baseColor);
            yield return new WaitForSeconds(PathFinder.Instance.SearchSpeed);
        }
        PathFinder.Instance.IsRunning = false;
        PathFinder.Instance.IsPreview = true;
    }

    void ExploreNeighbors()
    {
        foreach (Vector2Int dir in directions)
        {
            ZoneComponent currZC = entityManager.GetComponentData<ZoneComponent>(currentSearchZone);
            Vector2Int neighborCoor = new Vector2Int(currZC.coordinates.x, currZC.coordinates.y) + dir;

            if (!ZoneStore.Instance.Zones.ContainsKey(neighborCoor)) continue;
            Entity neighbor = ZoneStore.Instance.Zones[neighborCoor];
            ZoneComponent neighborZC = entityManager.GetComponentData<ZoneComponent>(neighbor);

            if (reached.ContainsKey(neighborCoor) || !neighborZC.isWalkable) continue;
            neighborZC.connectedTo = currentSearchZone;
            entityManager.SetComponentData<ZoneComponent>(neighbor, neighborZC);
            reached.Add(neighborCoor, neighbor);
            eQueue.Enqueue(neighbor);
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
            path.Add(currentZone);
            zc = entityManager.GetComponentData<ZoneComponent>(currentZone);
            zc.isPath = true;
            entityManager.SetComponentData<ZoneComponent>(currentZone, zc);
        }
        if (path[0] == null || path[0] == endZone)
        {
            //return something or validate this on the other class;
        }
        path.Reverse();
        return path;
    }
}