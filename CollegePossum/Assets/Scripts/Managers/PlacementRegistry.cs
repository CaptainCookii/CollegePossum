using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementRegistry
{
    private static List<PlacementNode> nodes = new List<PlacementNode>();

    public static void Register(PlacementNode node)
    {
        if (!nodes.Contains(node))
            nodes.Add(node);
    }

    public static void Unregister(PlacementNode node)
    {
        nodes.Remove(node);
    }

    public static List<PlacementNode> GetFreeNodes()
    {
        return nodes.FindAll(n => !n.isOccupied);
    }
}
