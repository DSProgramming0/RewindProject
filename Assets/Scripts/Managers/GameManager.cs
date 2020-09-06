using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState;

    public bool roundRestarting = false;
    [SerializeField] private Transform player;
    [SerializeField] private Transform startPoint;

    private bool levelActive;
    [SerializeField] private float levelTimer;

    [SerializeField] private float endTimerResult;
    [SerializeField] private int pointsEarnedFromTime;
    [SerializeField] private float targetsHit;
    [SerializeField] private float playerStateBuff;
    [SerializeField] private float result;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        registerEvents();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentGameState = GameState.INGAME;
    }

    void Start()
    {
        UIManager.instance.toggleUIElements(true, true, false, false);
    }

    private void TogglePlayerComponents(bool _shouldDisable)
    {
        if (_shouldDisable)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<TimeBody>().enabled = false;
            player.GetComponentInChildren<MouseLook>().enabled = false;
            player.GetComponentInChildren<ShootExplosion>().enabled = false;
        }
        else if (!_shouldDisable)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<TimeBody>().enabled = true;
            player.GetComponentInChildren<MouseLook>().enabled = true;
            player.GetComponentInChildren<ShootExplosion>().enabled = true;
        }      
    }   

    private void registerEvents()
    {
        onLevelStart += startLevelTimer;
        onFinishLineReached += stopLevelTimer;
        onFinishLineReached += CallStartToSetFinishMenu;
        onPlayerDeath += CallStartToSetDeathMenu;
        onLevelReset += levelReset;
    }

    public void Update()
    {
        if(levelActive == true)
        {
            levelTimer += Time.unscaledDeltaTime;
        }

        UIManager.instance.setCountdown(levelTimer);        
    }

    //Event has one suscriber/ startLevelTimer which will start the level timer
    public event Action onLevelStart;
    public void LevelStart()
    {
        if(onLevelStart != null)
        {
            onLevelStart();
        }
    }

    //Event has two suscribers/ stopLevel timer and callStartToSetEndMenu/ Stops timer and starts the process to activate end menu
    public event Action onFinishLineReached;
    public void FinishLineReached()
    {
        if (onFinishLineReached != null)
        {
            calculateEndResult(false, levelTimer, targetsHit);
            onFinishLineReached();
        }
    }

    public event Action onPlayerDeath;
    public void PlayerDied()
    {
        if (onPlayerDeath != null)
        {
            calculateEndResult(true, levelTimer, targetsHit);
            onPlayerDeath();
        }
    }

    public event Action onLevelReset;
    public void ResetLevel()
    {
        if (onLevelReset != null)
        {
            onLevelReset();
        }
    }   

    public void recieveTargetHitPoints(float _targetHitValue)
    {
        targetsHit += _targetHitValue;
    }

    private void calculateEndResult(bool didPlayerDie, float _endTimerResult, float _targetsHit)
    {
        UIManager.instance.toggleUIElements(true, false, false, false);

        if (didPlayerDie)
            playerStateBuff = 1.8f;
        else
            playerStateBuff = 0.8f;

        if (!didPlayerDie)
        {
            if (_endTimerResult <= 10)
                pointsEarnedFromTime = 60;
            else if (_endTimerResult > 11 && _endTimerResult <= 20)
                pointsEarnedFromTime = 50;
            else if (_endTimerResult > 21 && _endTimerResult <= 30)
                pointsEarnedFromTime = 40;
            else if (_endTimerResult > 31 && _endTimerResult <= 40)
                pointsEarnedFromTime = 30;
            else if (_endTimerResult > 41 && _endTimerResult <= 50)
                pointsEarnedFromTime = 20;
            else if (_endTimerResult > 51 && _endTimerResult <= 60)
                pointsEarnedFromTime = 10;
            else
                pointsEarnedFromTime = 3;
        }
        else if (didPlayerDie)
        {
            pointsEarnedFromTime = 2;
        }        

        endTimerResult = _endTimerResult;
        targetsHit = _targetsHit;

        result = endTimerResult * targetsHit / playerStateBuff;
    }

    private void ResetResults()
    {
        roundRestarting = false;
        endTimerResult = 0;
        pointsEarnedFromTime = 0;
        targetsHit = 0;
        playerStateBuff = 0;
        result = 0;
    }

    private IEnumerator startToSetFinishEndMenu()
    {
        player.GetComponent<TimeBody>().enabled = false;
        yield return new WaitForSeconds(1f);
        currentGameState = GameState.INMENU;
        UIManager.instance.setEndscreenValues(true, endTimerResult, pointsEarnedFromTime, targetsHit, result);
        TogglePlayerComponents(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.instance.toggleUIElements(false, false, true, true);

    }

    private IEnumerator startToSetDeathEndMenu()
    {
        player.GetComponent<TimeBody>().enabled = false;
        yield return new WaitForSeconds(1f);
        currentGameState = GameState.INMENU;
        UIManager.instance.setEndscreenValues(false, endTimerResult, pointsEarnedFromTime, targetsHit, result);
        TogglePlayerComponents(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.instance.toggleUIElements(false, false, true, true);
    }

    private IEnumerator startToResetLevel()
    {
        player.transform.position = startPoint.transform.position;
            yield return new WaitForSeconds(1f);
        currentGameState = GameState.INGAME;
            yield return new WaitForSeconds(.5f);
        TogglePlayerComponents(false);
        UIManager.instance.toggleUIElements(true, true, false, false);
        ResetLevel();
    }

    private void CallStartToSetFinishMenu()
    {
        StartCoroutine(startToSetFinishEndMenu());
    }

    private void CallStartToSetDeathMenu()
    {
        StartCoroutine(startToSetDeathEndMenu());
    }

    public void playerPressedContinue()
    {
        roundRestarting = true;
        SoundManager.Play3DSound(SoundManager.Sound.Continue, player.position);
        UIManager.instance.toggleUIElements(false, false, true, false);

        StartCoroutine(startToResetLevel());
    }  

    private void levelReset()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ResetResults();
        levelTimer = 0;
    }

    private void startLevelTimer()
    {
        Debug.Log("Timer Started");
        levelActive = true;
    }

    private void stopLevelTimer()
    {
        Debug.Log("Timer Stopped");
        levelActive = false;
    }
}

public enum GameState
{
    INGAME,
    INMENU
}
