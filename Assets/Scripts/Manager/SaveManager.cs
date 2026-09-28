using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private GameSaveData saveData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadSaveData();
    }
    public void LoadSaveData()
    {
        string json = PlayerPrefs.GetString("SAVE");
        if (string.IsNullOrEmpty(json)) return;
        Debug.Log("ря╪сть");
        saveData = JsonUtility.FromJson<GameSaveData>(json);
    }
    public void SaveData(GridManager gridManager)
    {
        GameSaveData data = new GameSaveData();
        data.score = ScoreManager.Instance.GetScore();
        data.moves=ScoreManager.Instance.GetMoves();
        //data.styleName = StyleManager.Instance.GetcurrentStyle().styleName;
        //PlayerPrefs.SetInt("GAMESCORE", ScoreManager.Instance.GetScore());
        //PlayerPrefs.SetString("STYLENAME", StyleManager.Instance.GetcurrentStyle().styleName);
        for (int x = 0; x < gridManager.width; x++)
        {
            for (int y = 0; y < gridManager.height; y++)
            {
                Piece piece = gridManager.GetPiece(x, y);
                if (piece == null) continue;
                BoardPieceData pieceData = new BoardPieceData();
                pieceData.x = x;
                pieceData.y = y;
                pieceData.pieceType = piece.type;
                data.piecesData.Add(pieceData);
            }
        }
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SAVE", json);
        saveData = data;
        PlayerPrefs.Save();
    }
    public void SaveBestScore(int bestScore)
    {
        PlayerPrefs.SetInt("BESTSCORE",bestScore);
        PlayerPrefs.Save();
    }
    public void ClearSave()
    {
        PlayerPrefs.DeleteKey("SAVE");
        saveData = null;
        PlayerPrefs.Save();
    }
    public GameSaveData GetSaveData()
    {
        return saveData;
    }

}
