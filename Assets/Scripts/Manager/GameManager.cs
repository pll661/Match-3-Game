using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    Resolving,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState gameState { get; private set; }
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int startMove = 5;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        gameState = GameState.MainMenu;
        UIManager.Instance.ShowMainPanel();
    }
    public void NewGame()
    {
        gridManager.ClearAllPiece();
        UIManager.Instance.ShowGamePanel();
        ScoreManager.Instance.SetScore(0);
        ScoreManager.Instance.SetMoves(startMove);
        gridManager.InitGrid();
    }

    public void ContinueGame()
    {
        if (SaveManager.Instance.GetSaveData() == null)
        {
            Debug.Log("数据为空");
            return;
        }
        SetGameState(GameState.Playing);
        UIManager.Instance.ShowGamePanel();
        //PieceData data = StyleManager.Instance.GetStyleDataByName(SaveManager.Instance.GetSaveData().styleName);
        //StyleManager.Instance.ChangedStyle(data);
        gridManager.LoadData(SaveManager.Instance.GetSaveData());
        ScoreManager.Instance.SetScore(SaveManager.Instance.GetSaveData().score);
        ScoreManager.Instance.SetMoves(SaveManager.Instance.GetSaveData().moves);

    }
    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        //Debug.Log("改变游戏状态"+gameState);
    }
    public void QuitToMainMenu()
    {
        SaveCheckPoint();
        SetGameState(GameState.MainMenu);
        gridManager.ClearAllPiece();
        UIManager.Instance.ShowMainPanel();
    }
    public void SaveCheckPoint()
    {
        if (gridManager.IsBoardComplete())
        {
            Debug.Log("检查点");
            SaveManager.Instance.SaveData(gridManager);
        }
    }
    public void OnBoardSettled(Piece[,] pieces, int width, int height)
    {
        if (ScoreManager.Instance.GetMoves() <= 0)
        {
            ScoreManager.Instance.UpdateBestScore();
            UIManager.Instance.ShowOverPanel();
            UIManager.Instance.ShowOverScoreUI();
            gameState = GameState.GameOver;
            SaveManager.Instance.ClearSave();
            gridManager.ClearAllPiece();
            return;
        }
        if (MatchSystem.HasPossibleMove(pieces, width, height))
        {
            gameState = GameState.Playing;
        }
        else
        {
            gridManager.ClearAllPiece();
            gridManager.InitGrid();
        }

        SaveCheckPoint();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
