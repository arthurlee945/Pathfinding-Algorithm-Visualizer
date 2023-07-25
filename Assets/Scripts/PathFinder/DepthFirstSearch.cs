using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

class DepthFirstSearch{
    EntityManager entityManager;
    Vector2Int startCoors, endCoors;
    Entity startZone, endZone, currentSearchZone;
    Dictionary<Vector2Int, Entity> reached = new Dictionary<Vector2Int, Entity>();
    Stack<Entity> eStack = new Stack<Entity>();
    Vector2Int[] directions = { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };
    ref bool isRunning;
    public float SearchSpeed { get; set; } = 0.02f;
    public DepthFirstSearch(EntityManager entityManager){
        this.entityManager = entityManager;
    }
    public  void FindPath(Vector2Int startCoors, Vector2Int endCoors, ref bool IsRunning){
        isRunning = IsRunning;
        isRunning = true;
        this.startCoors = startCoors;
        this.endCoors = endCoors;
        startZone = ZoneStore.Instance.Zones[startCoors];
        endZone = ZoneStore.Instance.Zones[endCoors];
        //------------------Start Running the Algo
        Algorithm();
    }

    /// <summary>
    /// <code>
    /// procedure DFS(G, v) is
    ///     label v as discovered
    ///     for all directed edges from v to w that are in G.adjacentEdges(v) do
    ///         if vertex w is not labeled as discovered then
    ///             recursively call DFS(G, w)
    /// </code>
    /// <code>
    /// procedure DFS_iterative(G, v) is
    ///     let S be a stack
    ///     S.push(v)
    ///     while S is not empty do
    ///         v = S.pop()
    ///         if v is not labeled as discovered then
    ///             label v as discovered
    ///             for all edges from v to w in G.adjacentEdges(v) do 
    ///                 S.push(w)
    /// </code>
    /// <code>
    /// procedure DFS_iterative(G, v) is
    ///     let S be a stack
    ///     label v as discovered
    ///     S.push(iterator of G.adjacentEdges(v))
    ///     while S is not empty do
    ///         if S.peek().hasNext() then
    ///             w = S.peek().next()
    ///             if w is not labeled as discovered then
    ///                 label w as discovered
    ///                 S.push(iterator of G.adjacentEdges(w))
    ///         else
    ///             S.pop()
    /// </code>
    /// </summary>
    
     void Algorithm(){
        eStack.Push(startZone);
        while(eStack.Count > 0 && isRunning){
            currentSearchZone = eStack.Pop();
            ZoneComponent currZC = entityManager.GetComponentData<ZoneComponent>(currentSearchZone);
            currZC.isExplored = true;
            entityManager.SetComponentData<ZoneComponent>(currentSearchZone, currZC);
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

            StartCoroutine(ExploreNeighbors());

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