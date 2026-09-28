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
    [SerializeField] private GameObject GamePanel;
    [SerializeField] private GameObject OverPanel;
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
        GamePanel.SetActive(true);
        OverPanel.SetActive(false);
    }
    public void ShowMainPanel()
    {
        mainMenuPanel.SetActive(true);
        GamePanel.SetActive(false);
        OverPanel.SetActive(false);
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
        GamePanel.SetActive(false);
        OverPanel.SetActive(false);
        chooseStylePanel.SetActive(true);
    }
    public void ShowOverPanel()
    {
        mainMenuPanel.SetActive(false);
        GamePanel.SetActive(false);
        OverPanel.SetActive(true);
    }
    public void ShowOverScoreUI()
    {
        scoreText.text="Score:"+ScoreManager.Instance.GetScore();
        bestScoreText.text = "BestScore:" + ScoreManager.Instance.GetBestScore();
    }

}
