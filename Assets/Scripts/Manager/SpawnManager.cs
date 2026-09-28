using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{

    public GridManager grid;

    public SpawnManager(GridManager grid)
    {
        this.grid = grid;

    }

    public Piece SpawnPiece(int x,int y,Piece piecePrefab,PieceType type, PieceStyle data)
    {
        Piece p = GameObject.Instantiate(piecePrefab, grid.transform);
        p.InitPiece(x, y, type, data.Sprite);
        return p;
    }

}
