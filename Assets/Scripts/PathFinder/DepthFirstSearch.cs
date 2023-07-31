using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;

public class DepthFirstSearch : MonoBehaviour
{
    EntityManager entityManager;
    Vector2Int startCoors, endCoors;
    Entity startZone, endZone, currentSearchZone;
    Dictionary<Vector2Int, Entity> reached = new Dictionary<Vector2Int, Entity>();
    Stack<Entity> eStack = new Stack<Entity>();
    Vector2Int[] directions = { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };
    public float SearchSpeed { get; set; } = 0.02f;
    Coroutine algorithm;

    void Awake()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    public void FindPath(Vector2Int startCoors, Vector2Int endCoors)
    {
        if (algorithm != null) StopCoroutine(algorithm);
        PathFinder.Instance.IsRunning = true;
        this.startCoors = startCoors;
        this.endCoors = endCoors;
        startZone = ZoneStore.Instance.Zones[startCoors];
        endZone = ZoneStore.Instance.Zones[endCoors];
        //------------------Start Prep
        eStack.Clear();
        reached.Clear();
        //------------------Start Running the Algo
        algorithm = StartCoroutine(Algorithm());
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

    IEnumerator Algorithm()
    {
        eStack.Push(startZone);
        reached.Add(startCoors, startZone);
        bool isRunning = true;
        while (eStack.Count > 0 && isRunning)
        {
            currentSearchZone = eStack.Pop();
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
            //--------------------
            ExploreNeighbors();
            yield return new WaitForSeconds(PathFinder.Instance.SearchSpeed);
            if (currentSearchZone != endZone) continue;
            //------------------End Algo
            isRunning = false;
        }

        //-------------display path
        List<Entity> paths = BuildPath();
        if (paths.Count <= 0) StateChangeDisplay.Instance.DisplayState("No Path Found");
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
        algorithm = null;
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
            eStack.Push(neighbor);
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
    public void ResetPath()
    {
        if (algorithm != null) StopCoroutine(algorithm);
    }
}