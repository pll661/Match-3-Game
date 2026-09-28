using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchSystem
{

    public static List<Piece> GetMatchLine(Piece[,] pieces, Piece centerPiece, int width, int height, int dx, int dy)
    {
        List<Piece> result = new List<Piece>();
        if (centerPiece == null) return result;
        result.Add(centerPiece);
        int x = centerPiece.x + dx;
        int y = centerPiece.y + dy;
        while (IsValid(x, y, width, height) && pieces[x, y] != null && pieces[x, y].type == centerPiece.type)
        {
            result.Add(pieces[x, y]);
            x += dx;
            y += dy;
        }
        x = centerPiece.x - dx;
        y = centerPiece.y - dy;
        while (IsValid(x, y, width, height) && pieces[x, y] != null && pieces[x, y].type == centerPiece.type)
        {
            result.Add(pieces[x, y]);
            x -= dx;
            y -= dy;
        }
        return result;
    }
    public static bool IsValid(int x, int y, int width, int height)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    /// <summary>
    /// 检查移动的piece
    /// </summary>
    /// <param name="pieces"></param>
    /// <param name="centerPiece"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static List<Piece> GetMatch(Piece[,] pieces, Piece centerPiece, int width, int height)
    {
        HashSet<Piece> result = new HashSet<Piece>();
        List<Piece> matchX = GetMatchLine(pieces, centerPiece, width, height, 1, 0);
        List<Piece> matchY = GetMatchLine(pieces, centerPiece, width, height, 0, 1);
        if (matchX.Count >= 3)
        {
            result.UnionWith(matchX);
        }
        if (matchY.Count >= 3)
        {
            result.UnionWith(matchY);
        }
        return new List<Piece>(result);
    }
    /// <summary>
    /// 检查所有piece
    /// </summary>
    /// <param name="pieces"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static List<Piece> CheckAllMatch(Piece[,] pieces, int width, int height)
    {
        HashSet<Piece> result = new HashSet<Piece>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Piece piece = pieces[x, y];
                if (piece == null) continue;
                List<Piece> matchX = GetMatchLine(pieces, piece, width, height, 1, 0);
                List<Piece> matchY = GetMatchLine(pieces, piece, width, height, 0, 1);
                if (matchX.Count >= 3)
                {
                    result.UnionWith(matchX);
                }
                if (matchY.Count >= 3)
                {
                    result.UnionWith(matchY);
                }
            }
        }
        return new List<Piece>(result);
    }
    public static bool HasPossibleMove(Piece[,] pieces, int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Piece piece = pieces[x, y];
                if (piece == null) continue;
                if (x + 1 < width && TrySwapMakesMatch(pieces, x, y, x + 1, y, width, height))
                {
                    return true;
                }
                if (y - 1 >= 0 && TrySwapMakesMatch(pieces, x, y, x, y - 1, width, height))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static bool TrySwapMakesMatch(Piece[,] pieces, int ax, int ay, int bx, int by, int width, int height)
    {
        Piece a = pieces[ax, ay];
        Piece b = pieces[bx, by];
        pieces[ax, ay] = b;
        pieces[bx, by] = a;
        a.SetPos(bx, by);
        b.SetPos(ax, ay);
        List<Piece> matchsA = GetMatch(pieces, a, width, height);
        List<Piece> matchsB = GetMatch(pieces, b, width, height);
        pieces[ax, ay] = a;
        pieces[bx, by] = b;
        a.SetPos(ax, ay);
        b.SetPos(bx, by);
        if (matchsA.Count >= 3 || matchsB.Count >= 3) return true;
        return false;
    }
}
