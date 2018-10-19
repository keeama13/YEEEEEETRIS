using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

public class TetrisBlocks
{
    public  bool[,] curBool;
    public Color curColor;
    public static TetrisBlocks curBlock;
    public Vector2 Location, SpawnLocation;
    public int blockSize;
    public TetrisBlocks(Vector2 Location)
    {
        this.Location = Location;
    }

    public TetrisBlocks GetRandomBlock(int x)
    {

        switch (x)
        {
            case 0:
                return new BlockI(SpawnLocation);
            case 1:
                return new Square(SpawnLocation);
            case 2:
                return new BlockS(SpawnLocation);
            case 3:
                return new BlockZ(SpawnLocation);
            case 4:
                return new BlockT(SpawnLocation);
            case 5:
                return new BlockL(SpawnLocation);
            default:
                return new BlockInvL(SpawnLocation);
        }
    }

    


}

public class BlockI : TetrisBlocks
{
    public BlockI(Vector2 Location) : base(Location)
    {
        SpawnLocation = new Vector2(3, -4);
        blockSize = 4;
        curColor = Color.LightBlue;
        curBool = new bool[4, 4]
        {
            {false, false, true, false},
            {false, false, true, false},
            {false, false, true, false},
            {false, false, true, false},
        };
    }
}

public class Square : TetrisBlocks
{
    public Square(Vector2 Location) : base(Location)
    {
        SpawnLocation = new Vector2(5, -2);
        blockSize = 2;
        curColor = Color.Yellow;
        curBool = new bool[2, 2]
        {
            {true, true},
            {true, true},
        };
    }
}

public class BlockZ : TetrisBlocks
{
    public BlockZ(Vector2 Location) : base(Location)
    {
        SpawnLocation = new Vector2(4, -3);
        blockSize = 3;
        curColor = Color.Green;
        curBool = new bool[3, 3]
        {
            {false, false, false},
            {true,  true,  false},
            {false, true,  true},

        };
    }
}

public class BlockS : TetrisBlocks
{
    public BlockS(Vector2 Location) : base(Location)
    {
        SpawnLocation = new Vector2(4, -3);
        blockSize = 3;
        curColor = Color.Red;
        curBool = new bool[3, 3]
        {
            {false, false, false},
            {false, true,  true},
            {true,  true,  false},
        };
    }
}

public class BlockL : TetrisBlocks
{
    public BlockL(Vector2 Location) : base(Location)
    {
        SpawnLocation = new Vector2(4, -3);
        blockSize = 3;
        curColor = Color.Orange;
        curBool = new bool[3, 3]
        {
            {false, true, false},
            {false, true, false},
            {false, true, true},

        };
    }
}

public class BlockInvL : TetrisBlocks
{
    public BlockInvL(Vector2 Location) : base(Location)
    {
        SpawnLocation = new Vector2(4, -3);
        blockSize = 3;
        curColor = Color.Blue;
        curBool = new bool[3, 3]
        {
            {false, true, false},
            {false, true, false},
            {true, true, false},
            
        };
    }
}

public class BlockT : TetrisBlocks
{
    public BlockT(Vector2 Location) : base(Location)
    {
        SpawnLocation = new Vector2(4, -3);
        blockSize = 3;
        curColor = Color.Purple;
        curBool = new bool[3, 3]
        {
            {false, false, false},
            {false, true,  false},
            {true,  true,  true},

        };

    }
}

