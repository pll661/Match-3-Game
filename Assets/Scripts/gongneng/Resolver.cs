using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolver
{
    private GridManager grid;
    public Resolver(GridManager grid)
    {
        this.grid = grid;
    }

    public void ResolveBoard(Piece p1, Piece p2)
    {
        List<Piece> match1 = MatchSystem.GetMatch(grid.pieces, p1, grid.width, grid.height);
        List<Piece> match2 = MatchSystem.GetMatch(grid.pieces, p2, grid.width, grid.height);
        HashSet<Piece> matchs = new HashSet<Piece>();
        matchs.UnionWith(match1);
        matchs.UnionWith(match2);
        if (matchs.Count <= 0) return;
        grid.StartCoroutine(ResolveMatches(new List<Piece>(matchs)));

    }
    //private void ResolveMatches(List<Piece> matchs)
    //{
    //    //if (matchs.Count <= 0) return;
    //    //grid.ClearPiece(matchs);
    //    ScoreManager.Instance.AddScore(matchs.Count);
    //    Sequence clearPiece = grid.ClearPiece(matchs);
    //    clearPiece.OnComplete(() =>
    //    {
    //        grid.RemovePiece(matchs);
    //        //Debug.Log("ï¿½ï¿½Ò»ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½");
    //        Sequence dropSequence = DropSystem.DropPiece(grid.width, grid.height, grid.cellSpacing, grid.pieces);
    //        dropSequence.OnComplete(() =>
    //        {
    //            //Debug.Log("ï¿½ï¿½Ò»ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½");
    //            Sequence generateSequence = grid.GeneratePiece();
    //            generateSequence.OnComplete(() =>
    //            {
    //                //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
    //                //Debug.Log("ï¿½ï¿½Ò»ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½");
    //                List<Piece> match = MatchSystem.CheckAllMatch(grid.pieces, grid.width, grid.height);
    //                if (match.Count <= 0)
    //                {
    //                    GameManager.Instance.SetGameState(GameState.Playing);
    //                    return;
    //                }
    //                //Debug.Log("ï¿½Ø¸ï¿½Ö´ï¿½ï¿½");
    //                ResolveMatches(match);
    //            });
    //        });
    //    });
    //    //Debug.Log("ï¿½ï¿½ï¿½Ì½ï¿½ï¿½ï¿½");
    //}
    private IEnumerator ResolveMatches(List<Piece> matchs)
    {
        //if (matchs.Count <= 0) return;
        //grid.ClearPiece(matchs);
        while (matchs.Count >= 3)
        {
            ScoreManager.Instance.AddScore(matchs.Count);
            Sequence clearPiece = grid.ClearPiece(matchs);
            yield return clearPiece.WaitForCompletion();
            grid.RemovePiece(matchs);
            Sequence dropSequence = DropSystem.DropPiece(grid.width, grid.height, grid.cellSpacing, grid.pieces);
            yield return dropSequence.WaitForCompletion();
            Sequence generateSequence = grid.GeneratePiece();
            yield return generateSequence.WaitForCompletion();
            
            matchs = MatchSystem.CheckAllMatch(grid.pieces, grid.width, grid.height);
        }

        GameManager.Instance.OnBoardSettled(grid.pieces, grid.width, grid.height);       
        //Debug.Log("½á¹û"+MatchSystem.HasPossibleMove(grid.pieces, grid.width, grid.height));
        //Debug.Log("ÖØ¸´Ö´ÐÐ");
        //ResolveMatches(match);
    }
}
