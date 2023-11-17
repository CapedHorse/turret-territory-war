using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Reference
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject loadingPanel;
    
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
        
        SetLoadingPanel(true);
    }
    


    public void UpdateTimeText(float currentTime)
    {
        System.TimeSpan time = System.TimeSpan.FromSeconds(currentTime + 1);
        timerText.text = time.ToString("mm':'ss");
    }

    public void SetLoadingPanel(bool on)
    {
        loadingPanel.SetActive(on);
    }

}
