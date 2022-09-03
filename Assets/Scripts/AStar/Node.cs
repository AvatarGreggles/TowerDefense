using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public Point GridPosition { get; private set; }

    public TileScript TileRef { get; private set; }

    public Node Parent { get; private set; }


    public Vector2 WorldPosition { get; set; }
    public int G { get; private set; }
    public int H { get; private set; }
    public int F { get; private set; }

    public Node(TileScript tileRef)
    {
        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition;
        this.WorldPosition = tileRef.WorldPosition;
    }

    public void CalcValues(Node parent, Node goal, int gCost)
    {
        this.Parent = parent;
        this.G = parent.G + gCost;
        this.H = ((Mathf.Abs(GridPosition.x - goal.GridPosition.x)) + Mathf.Abs((goal.GridPosition.y - GridPosition.y))) * 10;
        this.F = this.G + this.H;

    }

}
