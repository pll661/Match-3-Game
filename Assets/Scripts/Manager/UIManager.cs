using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private GameObject chooseStylePanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject overPanel;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void ShowGamePanel()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
        overPanel.SetActive(false);
    }
    public void ShowMainPanel()
    {
        mainMenuPanel.SetActive(true);
        gamePanel.SetActive(false);
        overPanel.SetActive(false);
        chooseStylePanel.SetActive(false);
        if(SaveManager.Instance.GetSaveData() == null)
        {
            continueButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
        }
    }
    public void ShowChooseStyle()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(false);
        overPanel.SetActive(false);
        chooseStylePanel.SetActive(true);
    }
    public void ShowOverPanel()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
    }
    public void ShowOverScoreUI()
    {
        scoreText.text="Score:"+ScoreManager.Instance.GetScore();
        bestScoreText.text = "BestScore:" + ScoreManager.Instance.GetBestScore();
    }

}
