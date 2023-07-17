using Unity.Mathematics;
public class Node
{
    public int2 coordinates;
    public bool isWalkable, isExplored, isPath;
    public Node connectedTo;

    public Node(int2 coordinates, bool isWalkable = true)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
    }
}
