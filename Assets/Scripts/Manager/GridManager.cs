using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("网格配置")]
    public int width;
    public int height;
    public float cellSpacing;
    [Space()]
    public Piece PiecePrefab;
    public Piece[,] pieces { get; private set; }
    //public PieceData currentStyleData;

    //组件
    private SpawnManager spawn;
    private Resolver resolver;

    private void Awake()
    {
       
    }

    private void Start()
    {
        spawn = new SpawnManager(this);
        resolver = new Resolver(this);
    }
    /// <summary>
    /// 初始化网格
    /// </summary>
    public Sequence InitGrid()
    {
        pieces = new Piece[width, height];
        Sequence drop = DOTween.Sequence();
        GameManager.Instance.SetGameState(GameState.Resolving);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                
                PieceType type = GetRandomPieceType(x, y);
                PieceStyle currentStyle = StyleManager.Instance.GetTypeData(type);
                Piece p = spawn.SpawnPiece(x, y, PiecePrefab, type, currentStyle);
                Vector3 startPos = DropSystem.GetWorldPosition(x,y+height*2,width,height,cellSpacing);
                Vector3 endPos=DropSystem.GetWorldPosition(x, y,width,height,cellSpacing);
                p.transform.position = startPos;
                //drop.Join(p.transform.DOMove(endPos, 0.35f).SetEase(Ease.OutQuad));
                float at = 0.35f + Random.Range(0f, 0.2f);
                drop.Insert(at, p.transform.DOMove(endPos, 0.35f).SetEase(Ease.OutQuad));
                //p.transform.position = endPos;
                pieces[x, y] = p;
            }
        }
        drop.OnComplete(() =>
        {
            GameManager.Instance.SetGameState(GameState.Playing);
        });
        return drop;

    }
    /// <summary>
    /// 获取随机类型的piece
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public PieceType GetRandomPieceType(int x, int y)
    {
        List<PieceType> pieceTypes = new List<PieceType>((PieceType[])System.Enum.GetValues(typeof(PieceType)));
        if (x > 1)
        {
            if (GetPiece(x - 2, y).type == GetPiece(x - 1, y).type)
            {
                pieceTypes.Remove(GetPiece(x - 1, y).type);
            }
        }
        if (y > 1)
        {
            if (GetPiece(x, y - 1).type == GetPiece(x, y - 2).type)
            {
                pieceTypes.Remove(GetPiece(x, y - 1).type);
            }
        }
        return pieceTypes[Random.Range(0, pieceTypes.Count)];
    }
    /// <summary>
    /// 获取指定坐标的piece
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Piece GetPiece(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return pieces[x, y];
        }
        return null;
    }
    /// <summary>
    /// 获取指定类型的piece数据
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    //public PieceStyle GetTypeData(PieceType t)
    //{
    //    foreach (PieceStyle data in currentStyleData.data)
    //    {
    //        if (data.type == t)
    //            return data;
    //    }
    //    return null;
    //}
    public Piece GetPieceByWorldPosition(Vector3 world)
    {
        float offsetX = (width - 1) * cellSpacing / 2;
        float offsetY = (height - 1) * cellSpacing / 2;
        int x = Mathf.RoundToInt((world.x + offsetX) / cellSpacing);
        int y = Mathf.RoundToInt((world.y + offsetY) / cellSpacing);
        return GetPiece(x, y);
    }
    public void SwapPiece(Piece p1, Piece p2)
    {
        //if (GameManager.Instance.gameState != GameState.Playing) return;
        int p1x = p1.x;
        int p1y = p1.y;
        int p2x = p2.x;
        int p2y = p2.y;
        Vector3 p1Pos = p1.transform.position;
        Vector3 p2Pos = p2.transform.position;
        pieces[p1x, p1y] = p2;
        pieces[p2x, p2y] = p1;
        p1.SetPos(p2x, p2y);
        p2.SetPos(p1x, p1y);
        Sequence seq = DOTween.Sequence();
        seq.Join(p1.transform.DOMove(p2Pos, 0.2f)).SetEase(Ease.OutQuad);
        seq.Join(p2.transform.DOMove(p1Pos, 0.2f)).SetEase(Ease.OutQuad);
        seq.OnComplete(() =>
        {
            List<Piece> match1 = MatchSystem.GetMatch(pieces, p1, width, height);
            List<Piece> match2 = MatchSystem.GetMatch(pieces, p2, width, height);
            //判断是否可消除
            if (match1.Count >= 3 || match2.Count >= 3)
            {
                //Debug.Log("消除")
                resolver.ResolveBoard(p1, p2);
                ScoreManager.Instance.ReduceMoves();
            }
            else
            {
                //Debug.Log("回退");
                pieces[p1x, p1y] = p1;
                pieces[p2x, p2y] = p2;
                p1.SetPos(p1x, p1y);
                p2.SetPos(p2x, p2y);
                Sequence seq = DOTween.Sequence();
                seq.Join(p1.transform.DOMove(p1Pos, 0.2f).SetEase(Ease.OutQuad));
                seq.Join(p2.transform.DOMove(p2Pos, 0.2f).SetEase(Ease.OutQuad));
                seq.OnComplete(() =>
                {
                    GameManager.Instance.OnBoardSettled(pieces,width,height);                  
                    //Debug.Log("结果" + MatchSystem.HasPossibleMove(pieces,width,height));
                });
            }
        });
    }
    public Sequence ClearPiece(List<Piece> matchs)
    {
        Sequence sequence = DOTween.Sequence();
        if (matchs == null || matchs.Count == 0) return sequence;
        foreach (Piece piece in matchs)
        {
            if (piece == null) continue;
            sequence.Join(piece.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));
        }

        return sequence;
    }
    public void RemovePiece(List<Piece> matchs)
    {
        foreach (Piece piece in matchs)
        {
            if (piece == null) continue;
            pieces[piece.x, piece.y] = null;
            Destroy(piece.gameObject);

        }
    }
    public Sequence GeneratePiece()
    {
        Sequence sequence = DOTween.Sequence();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (pieces[x, y] == null)
                {
                    PieceType type = GetRandomPieceType(x, y);
                    PieceStyle currentStyle = StyleManager.Instance.GetTypeData(type);
                    Piece p = spawn.SpawnPiece(x, y, PiecePrefab, type, currentStyle);
                    pieces[x, y] = p;
                    Vector3 pos = DropSystem.GetWorldPosition(x, y, width, height, cellSpacing);
                    p.transform.position = new Vector3(pos.x, pos.y + height, 0);
                    sequence.Join(p.transform.DOMove(pos, 0.2f));
                }
            }
        }
        return sequence;
    }
    public void ClearAllPiece()
    {
        if (pieces==null) return;
        foreach (Piece piece in pieces)
        {
            if (piece != null)
            {
                Destroy(piece.gameObject);
            }
        }
        pieces = null;
    }
    public bool IsBoardComplete()
    {
        if (pieces == null)
        {
            Debug.Log("找不到pieces！");
            return false;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (pieces[x, y] == null)
                {
                    Debug.Log("找不到piece！");
                    return false;
                }
            }
        }
        return true;
    }
    public void LoadData(GameSaveData datas)
    {
        pieces = new Piece[width, height];
        foreach (BoardPieceData data in datas.piecesData)
        {
            PieceStyle currentStyle = StyleManager.Instance.GetTypeData(data.pieceType);
            Piece p = spawn.SpawnPiece(data.x, data.y, PiecePrefab, data.pieceType, currentStyle);
            float offsetX = (width - 1) * cellSpacing / 2;
            float offsetY = (height - 1) * cellSpacing / 2;
            p.transform.position = new Vector3(cellSpacing * data.x - offsetX, cellSpacing * data.y - offsetY, 0);
            pieces[data.x, data.y] = p;
        }
    }
}
