using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;

public class Dijkstra : MonoBehaviour
{
    EntityManager entityManager;
    Vector2Int startCoors, endCoors;
    Entity startZone, endZone;
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
        //-------------------- start prep
        visited.Clear();
        unvisited.Clear();
        //--------------------run algorithm
        Algorithm();
    }
    void Algorithm()
    {
        Entity searchedZone;
        unvisited.Add(startZone);
        isRunning = true;
        while(unvisited.Count>0 && isRunning){
            searchedZone = unvisited[0];
            ZoneComponent searchedZC = entityManager.GetComponentData<ZoneComponent>(searchedZone);
            for(int i = 1; i < unvisited.Count; i++){
                ZoneComponent otherZC = entityManager.GetComponent<ZoneComponent>(unvisited[i]);
                if(otherZC.gCost <= searchedZC.gCost){
                    searchedZone = unvisited[i];
                    searchedZC = otherZC;
                }
            }
            unvisited.Remove(searchedZone);
            visited.Add(searchedZone);
            searchedZC.isExplored = true;
            entityManager.SetComponentData<ZoneComponent>(searchedZone, searchedZC);

            if(selectedZone == endZone) {
                isRunning = false;
                continue;
            }
            //----------------------Set Explored Color
            if (searchedZone != startZone && searchedZone != endZone)
            {
                URPMaterialPropertyBaseColor baseColor = entityManager.GetComponentData<URPMaterialPropertyBaseColor>(searchedZone);
                baseColor.Value = StateColors.Instance.ExploredColor;
                entityManager.SetComponentData<URPMaterialPropertyBaseColor>(searchedZone, baseColor);
            }
            ExploreNeighbors(searchedZone, searchedZC);
        }
        PathFinder.Instance.IsRunning = false;
        PathFinder.Instance.IsPreview = true;
    }
    void ExploreNeighbors(Entity searchedZone, ZoneComponent searchedZC){
        for(int x = -1; x <=1; x++){
            for(int y = -1; y< -1; y++){
                if(x ==0 && y == 0) continue;
                Vector2Int neighborCoor = new Vector2Int(searchedZC.coordinates.x + x, searchedZC.coordinates.y + y);
                if(!ZoneStore.Instance.Zones.ContainsKey(neighborCoor)) continue;
                Entity neighbor = ZoneStore.Instance.Zones[neighborCoor];
            }
        }
    }
    int GetDistance(ZoneComponent first, ZoneComponent second){
        int distX = Mathf.Abs(first.coordinates.x - second.coordinates.x);
        int distY = Mathf.Abs(first.coordinates.y - second.coordinates.y);
        //------> 14 == Sqrt(2) * 10 || 10 == straight line
        if(distX < distY) return 14 * distX + 10 * (distY - distX);
        return 14* distY + 10 * (distX - distY);
    }
}