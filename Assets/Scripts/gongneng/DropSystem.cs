using DG.Tweening;
using UnityEngine;

public class DropSystem
{
    public static Vector3 GetWorldPosition(int x, int y, int width, int height, float cellSpacing)
    {
        float offsetX = (width - 1) * cellSpacing / 2;
        float offsetY = (height - 1) * cellSpacing / 2;

        return new Vector3(
            cellSpacing * x - offsetX,
            cellSpacing * y - offsetY,
            0
        );
    }
    public static Sequence DropPiece(int width, int height, float cellSpacing, Piece[,] pieces)
    {
        Sequence sequence = DOTween.Sequence();
        for (int x = 0; x < width; x++)
        {
            int writeY = 0;
            for (int y = 0; y < height; y++)
            {
                if (pieces[x, y] == null)
                {
                    continue;
                }
                if (writeY != y)
                {
                    Piece top = pieces[x, y];
                    pieces[x, writeY] = top;
                    pieces[x, y] = null;   
                    top.SetPos(x, writeY);
                    Vector3 pos = GetWorldPosition(x, writeY, width, height, cellSpacing);
                    sequence.Join(top.transform.DOMove(pos, 0.2f));
                }
                //Debug.Log("ÏÂÂä");
                writeY++;
            }
        }
        return sequence;
    }



}
