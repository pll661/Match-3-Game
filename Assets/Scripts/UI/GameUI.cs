using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI movesText;
    private Tween scoreTween;
    private int displayScore;

    private void Awake()
    {
        ScoreManager.Instance.onMovesChanged += ChangedMovesText;
        ScoreManager.Instance.onScoreChanged += UpdateScore;
        ScoreManager.Instance.onScoreSet += SetScore;
    }
    public void BackHome()
    {
        GameManager.Instance.QuitToMainMenu();
    }
    public void UpdateScore(int score)
    {
        scoreTween?.Kill();
        scoreTween = DOTween.To(() => displayScore,
            (value) =>
            {
                displayScore = value;
                scoreText.text = "Score:" + value.ToString();
            }, score, 1.5f);
    }
    public void SetScore(int score)
    {
        scoreTween?.Kill();

        displayScore = score;
        scoreText.text = "Score:" + score;
    }
    public void ChangedMovesText(int moves)
    {
        movesText.text = "Moves:" + moves;
    }
    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.onMovesChanged -= ChangedMovesText;
            ScoreManager.Instance.onScoreChanged -= UpdateScore;
            ScoreManager.Instance.onScoreSet -= SetScore;
        }
    }
}
