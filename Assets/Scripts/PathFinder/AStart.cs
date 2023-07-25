using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

class AStar{
    EntityManager entityManager;
    Vector2Int startCoors, endCoors;
    Entity startZone, endZone, currentSearchZone;
    ref bool isRunning;

    public AStar(EntityManager entityManager){
        this.entityManager = entityManager;
    }
    public void FindPath(Vector2Int startCoor, Vector2Int endCoor, ref bool IsRunning){
        isRunning = IsRunning;
        isRunning = true;
        this.startCoors = startCoors;
        this.endCoors = endCoors;
        startZone = ZoneStore.Instance.Zones[startCoors];
        endZone = ZoneStore.Instance.Zones[endCoors];
        //--------------------run algorithm
        Algorithm();
    }
    void Algorithm(){

    }
}