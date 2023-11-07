using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Reference
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject countDownPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject nextRoundPanel;
    public TextMeshProUGUI debugSpawnOnScreen;
    
    //caches
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    


    public void UpdateTimeText(float currentTime)
    {
        System.TimeSpan time = System.TimeSpan.FromSeconds(currentTime + 1);
        timerText.text = time.ToString("mm':'ss");
    }

    public void SetCountDownPanel(bool on)
    {
        countDownPanel.SetActive(on);
    }

    public void SetGameOverPanel(bool on)
    {
        gameOverPanel.SetActive(on);
    }

    public void SetNextRoundPanel(bool on)
    {
        nextRoundPanel.SetActive(on);
    }

}
