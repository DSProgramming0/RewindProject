using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private TimeManager timeManager;

    [Header("UI Objects")]
    [SerializeField] private Image timePointsBar;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private CanvasGroup HUD;
    [SerializeField] private CanvasGroup endscreen;
    [SerializeField] private CanvasGroup blackout;
    [SerializeField] private TextMeshProUGUI endScreenPrompt;
    [SerializeField] private TextMeshProUGUI timeCompletedInResult;
    [SerializeField] private TextMeshProUGUI pointsEarnedFromTime;
    [SerializeField] private TextMeshProUGUI targetsHitResult;
    [SerializeField] private TextMeshProUGUI endResult;

    private int currentCountDownValue;

    void Awake()
    {
        instance = this;        
    }  

    // Update is called once per frame
    void Update()
    {
        countdownText.text = currentCountDownValue.ToString();

        timePointsBar.fillAmount = timeManager.TimePoints / 100f;
        timePointsBar.fillAmount = Mathf.Clamp(timePointsBar.fillAmount, 0, 1);

        setEndMenuState();
    }

    public void setEndMenuState()
    {
        if (GameManager.instance.currentGameState == GameState.INMENU)
        {
            blackout.alpha += 0.9f * Time.unscaledDeltaTime;
            blackout.alpha = Mathf.Clamp(blackout.alpha, 0f, 1F);
            blackout.interactable = true;
            blackout.blocksRaycasts = true;
        }
        else if (GameManager.instance.currentGameState == GameState.INGAME)
        {
            blackout.alpha -= 0.9f * Time.unscaledDeltaTime; 
            blackout.alpha = Mathf.Clamp(blackout.alpha, 0f, 1F);
            blackout.interactable = false;
            blackout.blocksRaycasts = false;
        }
    }

    public void setEndscreenValues(bool _playerFinished, float _timeCompleted, int _pointsEarnedFromTime, float _targetsHit, float _endResult)
    {
        int timeCompletedInSeconds = Mathf.RoundToInt(_timeCompleted);
        int targeHitValue = Mathf.RoundToInt(_targetsHit);
        int endResultInPoints = Mathf.RoundToInt(_endResult);

        if (_playerFinished)
        {
            endScreenPrompt.text = string.Format("Congratulations on finishing!");            
        } 
        else if (!_playerFinished)
        {
            endScreenPrompt.text = string.Format("Unlucky! You didn't finish Loser!");           
        }

        timeCompletedInResult.text = timeCompletedInSeconds.ToString();
        pointsEarnedFromTime.text = string.Format("{0} points", _pointsEarnedFromTime);
        targetsHitResult.text = targeHitValue.ToString();
        endResult.text = endResultInPoints.ToString();
    }

    public void setCountdown(float value)
    {
        currentCountDownValue = Mathf.RoundToInt(value);
    }

    public void setTimePointsValue(float value)
    {
        currentCountDownValue = Mathf.RoundToInt(value);
    }   
    
    public void toggleUIElements(bool isHUD, bool showHUD, bool isEndScreen, bool showEndScreen)
    {
        if (isHUD)
        {
            if(showHUD)
                LeanTween.moveLocalY(HUD.gameObject, 410f, 1f).setEaseInOutBack();
            else if(!showHUD)
                LeanTween.moveLocalY(HUD.gameObject, 700F, 1f).setEaseOutBack();
        }
        else if (isEndScreen)
        {
            if(showEndScreen)
            {
                LeanTween.moveLocalX(endscreen.gameObject, 0f, 1f).setEaseInOutBack();
                endscreen.interactable = true;
                endscreen.blocksRaycasts = true;

            }
            else if(!showHUD)
            {
                LeanTween.moveLocalX(endscreen.gameObject, -1650f, 1f).setEaseOutBack();
                endscreen.interactable = false;
                endscreen.blocksRaycasts = false;
            }
        }
    }

    
}

