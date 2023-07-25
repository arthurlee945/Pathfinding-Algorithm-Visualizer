using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

class BreadthFirstSearch{
    EntityManager entityManager;
    Vector2Int startCoors, endCoors;
    Entity startZone, endZone, currentSearchZone;
    Dictionary<Vector2Int, Entity> reached = new Dictionary<Vector2Int, Entity>();
    Queue<Entity> eQueue = new Queue<Entity>();
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    ref bool isRunning;
    public float SearchSpeed { get; set; } = 0.02f;
    public BreadthFirstSearch(EntityManager entityManager){
        this.entityManager = entityManager;
    }
    public void FindPath(Vector2Int startCoors, Vector2Int endCoors, ref bool IsRunning)
    {
        isRunning = IsRunning;
        isRunning = true;
        CleanUp();
        this.startCoors = startCoors;
        this.endCoors = endCoors;
        startZone = ZoneStore.Instance.Zones[startCoors];
        endZone = ZoneStore.Instance.Zones[endCoors];
        //------------------Start Running the Algo
        Algorithm();
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
    
    void Algorithm()
    {
        eQueue.Enqueue(startZone);
        reached.Add(startCoors, startZone);

        while (eQueue.Count > 0 && isRunning)
        {
            currentSearchZone = eQueue.Dequeue();
            ZoneComponent currZC = entityManager.GetComponentData<ZoneComponent>(currentSearchZone);
            currZC.isExplored = true;
            entityManager.SetComponentData<ZoneComponent>(currentSearchZone, currZC);

            StartCoroutine(ExploreNeighbors());
            if (new Vector2Int(currZC.coordinates.x, currZC.coordinates.y) != endCoors) continue;
            //------------------End Algo
            isRunning = false;
        }
    }

    IEnumerator ExploreNeighbors()
    {
        foreach (Vector2Int dir in directions)
        {
            ZoneComponent currZC = entityManager.GetComponentData<ZoneComponent>(currentSearchZone);
            Vector2Int neighborCoor = new Vector2Int(currZC.coordinates.x, currZC.coordinates.y) + dir;
            if (!ZoneStore.Instance.Zones.ContainsKey(neighborCoor)) continue;
            Entity neighbor = ZoneStore.Instance.Zones[neighborCoor];
            ZoneComponent neighborZC = entityManager.GetComponentData<ZoneComponent>(neighbor);
            Vector2Int neightborCoor = new Vector2Int(neighborZC.coordinates.x, neighborZC.coordinates.y);

            if (reached.ContainsKey(neightborCoor) || !neighborZC.isWalkable) continue;
            neighborZC.connectedTo = currentSearchZone;
            entityManager.SetComponentData<ZoneComponent>(neighbor, neighborZC);
            reached.Add(neightborCoor, neighbor);
            eQueue.Enqueue(neighbor);
            yield return new WaitForSeconds(SearchSpeed);
        }
    }

    List<Entity> BuildPath(){
        List<Entity> path = new List<Entity>();
        Entity currentZone = endZone;
        ZoneComponent zc = entityManager.GetComponentData<ZoneComponent>(currentZone);
        while(zc.connectedTo != Entity.Null){
            currentZone = zc.connectedTo;
            path.Add(currentZone);
            zc = entityManager.GetComponentData<ZoneComponent>(currentZone);
            zc.isPath = true;
            entityManager.SetComponentData<ZoneComponent>(currentZone, zc);
        }
        return path.Reverse();
    }
    void CleanUp(){
        eQueue.Clear();
        reached.Clear();
    }
}