using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Dijkstra : MonoBehaviour
{
    EntityManager entityManager;
    Vector2Int startCoors, endCoors;
    Entity startZone, endZone, currentSearchZone;
    Dictionary<int, Entity> reachedVertex = new Dictionary<int, Entity>();

    void Awake()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void FindPath(Vector2Int startCoors, Vector2Int endCoors, ref bool isRunning)
    {
        isRunning = true;
        this.startCoors = startCoors;
        this.endCoors = endCoors;
        startZone = ZoneStore.Instance.Zones[startCoors];
        endZone = ZoneStore.Instance.Zones[endCoors];
        //-------------------- start prep
        reachedVertex.Clear();
        //--------------------run algorithm
        Algorithm(ref isRunning);
    }
    void Algorithm(ref bool isRunning)
    {

    }
}