using Unity.Entities;
using Unity.Mathematics;

[System.Serializable]
public struct Node
{
    public int2 coordinates;
    public bool isWalkable, isExplored, isPath;
    public int2 connectedTo;

    public Node(int2 coordinates, bool isWalkable = true)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
        this.isExplored = false;
        this.isWalkable = false;
        this.isPath = false;
        connectedTo = coordinates;
    }
}
