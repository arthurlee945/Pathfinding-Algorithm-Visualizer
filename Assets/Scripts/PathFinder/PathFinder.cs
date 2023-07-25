using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PathFinder : MonoBehaviour
{
    public PathFinder Instance { get; private set; }
    [SerializeField] Vector2Int startCoors, endCoors;
    EntityManager entityManager;
    Entity startZone, endZone, currentSearchZone;
    Dictionary<Vector2Int, Entity> reached = new Dictionary<Vector2Int, Entity>();
    Queue<Entity> frontier = new Queue<Entity>();
    //---------Breadth First Search Dir
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    public bool IsRunning { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            StartFindPath();
        }
    }
    public void StartFindPath()
    {
        IsRunning = true;
        if (!ZoneStore.Instance.Zones.ContainsKey(startCoors)) startCoors = new Vector2Int(0, 0);
        if (!ZoneStore.Instance.Zones.ContainsKey(endCoors)) endCoors = GameManager.GM.panelSize;
        startZone = ZoneStore.Instance.Zones[startCoors];
        endZone = ZoneStore.Instance.Zones[endCoors];
        frontier.Clear();
        reached.Clear();
        if (GameManager.GM.SelectedAlgo == Algorithms.BreadthFirstSearch) BreadthFirstSearch();
        IsRunning = false;
    }

    //---------------------BreadthFirstSearch Methods
    void BreadthFirstSearch()
    {
        frontier.Enqueue(startZone);
        reached.Add(startCoors, startZone);

        while (frontier.Count > 0 && IsRunning)
        {
            currentSearchZone = frontier.Dequeue();
            ZoneComponent currZC = entityManager.GetComponentData<ZoneComponent>(currentSearchZone);
            currZC.isExplored = true;
            entityManager.SetComponentData<ZoneComponent>(currentSearchZone, currZC);
            ExploreNeighbors();
            if (new Vector2Int(currZC.coordinates.x, currZC.coordinates.y) != endCoors) continue;
        }
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
            Vector2Int neightborCoor = new Vector2Int(neighborZC.coordinates.x, neighborZC.coordinates.y);

            if (reached.ContainsKey(neightborCoor) || !neighborZC.isWalkable) continue;
            neighborZC.connectedTo = currentSearchZone;
            entityManager.SetComponentData<ZoneComponent>(neighbor, neighborZC);
            reached.Add(neightborCoor, neighbor);
            frontier.Enqueue(neighbor);
        }
    }

    //---------------------Dijkstra Methods

    void Dijkstra()
    {

    }

    //---------------------AStar Methods
    void AStar()
    {

    }

    public void ResetZones(){
        foreach(Entity e in ZoneStore.Instance.Zones.Values){
            ZoneComponent zc = entityManager.GetComponentData<ZoneComponent>(e);
            zc.isPath = false;
            zc.isExplored = false;
            entityManager.SetComponentData<ZoneComponent>(e, zc);
        }
    }
}
