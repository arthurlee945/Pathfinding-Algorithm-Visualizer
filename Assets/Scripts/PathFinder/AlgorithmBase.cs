using UnityEngine;
using Unity.Entities;
abstract class AlgorithmBase{
    protected EntityManager entityManager;
    protected Vector2Int startCoors, endCoors;
    protected Entity startZone, endZone, currentSearchZone;
    public PathFinderBase(EntityManager entityManager){
        this.entityManager = entityManager;
    }
    public abstract void FindPath(Vector2Int startCoors, Vector2Int endCoors, ref bool IsRunning);
    public abstract void Algorithm();
}