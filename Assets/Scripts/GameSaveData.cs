using System.Collections.Generic;
[System.Serializable]
public class BoardPieceData
{
    public int x;
    public int y;
    public PieceType pieceType;
}
[System.Serializable]
public class GameSaveData
{
    public int score;
    public int moves;
    public List<BoardPieceData> piecesData=new List<BoardPieceData>();
   
}

