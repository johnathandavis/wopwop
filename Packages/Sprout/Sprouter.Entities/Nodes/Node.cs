namespace Sprouter.Entities.Nodes;

public abstract class Node
{
    public List<Node> SourceNodes { get; set; }
    public List<Node> DestinationNodes { get; set; }
}