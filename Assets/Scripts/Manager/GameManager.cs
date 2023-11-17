using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameSettings gameSettings;

    // public UnityEvent OnCountDownStarted, OnGameStarted, OnGameEnded, OnShowRanking;
    public UnityEvent OnGameStarted;
    // public UnityEvent<float> OnTimerUpdate;

    [FormerlySerializedAs("gameStart")] [FormerlySerializedAs("startedGame")] public bool playGame;

    public float GetPlayTime => playTime;
    private float playTime;
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
    
    // Start is called before the first frame update
    void Start()
    {
        playTime = gameSettings.gameDurationSeconds;
        
    }

    //TODO call this function from server once API is implemented, for now, start when initialization of the arena is done
    public void StartGame()
    {
        UIManager.instance.SetLoadingPanel(false);
        playGame = true;
        OnGameStarted.Invoke();
    }


    // Update is called once per frame
    void Update()
    {
        if (playGame)
        {
            playTime += Time.deltaTime;
            UIManager.instance.UpdateTimeText(playTime);
            
            /*
             playTime -= Time.deltaTime;
            UIManager.instance.UpdateTimeText(playTime);

            if (playTime <= 0)
            {
                GameOver();
            }*/
        }
    }

    private void GameOver()
    {
        StartCoroutine(DelayedGameOver());
    }

    private IEnumerator DelayedGameOver()
    {
        playGame = false;
        
        // UIManager.instance.SetGameOverPanel(true);
        
        yield return new WaitForSeconds(2f);
        
        // UIManager.instance.SetGameOverPanel(false);

        yield return new WaitForSeconds(gameSettings.countDownTime);

        RestartGame();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
