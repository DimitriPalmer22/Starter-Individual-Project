using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalScript : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _gameFinishedText;
    [SerializeField] private TMP_Text _gameIntroText;
    
    [SerializeField] private PlayerScript _player;

    private float _timeLeft = 10;

    private int messCount;

    private bool _gameStarted;
    private bool _gameLost, _gameWon;

    public bool GameStarted => _gameStarted;
    public bool GameFinished => _gameLost || _gameWon;
    
    // Start is called before the first frame update
    void Start()
    {
        CountMesses();
        SetTimerText();

        _gameIntroText.enabled = true;
        _gameFinishedText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            _gameIntroText.enabled = false;
            _gameStarted = true;
        }

        UpdateTimer();
    }

    /// <summary>
    /// Updates the messCount variable.
    /// Only needs to be called in the start function
    /// </summary>
    private void CountMesses()
    {
        messCount = GameObject.FindGameObjectsWithTag("Mess").Length;
    }

    /// <summary>
    /// Called by the player script.
    /// Decrements the mess count variable.
    /// Checks if the player has won the game.
    /// </summary>
    public void MessCleaned()
    {
        messCount -= 1;
        
        // Win Condition
        if (messCount <= 0 && _timeLeft > 0)
        {
            _gameWon = true;
            _gameLost = false;
            SetGameFinishedText("You Win.\nYou cleaned all the messes!");
        }
    }

    /// <summary>
    /// Ticks down the timer variable.
    /// Checks if the player has lost the game.
    /// </summary>
    private void UpdateTimer()
    {
        if (!GameStarted || GameFinished)
            return;
        
        _timeLeft -= Time.deltaTime;

        if (_timeLeft < 0)
        {
            _timeLeft = 0;

            // Lose Condition
            if (messCount > 0)
            {
                _gameLost = true;
                _gameWon = false;
                SetGameFinishedText("You Lose.\nYou failed to clean all the messes in 10 seconds.");
            }
            
        }
        
        SetTimerText();
    }
    
    /// <summary>
    /// Updates the UI element that has the timer info.
    /// </summary>
    private void SetTimerText()
    {
        _timerText.text = $"Time Left:\n{_timeLeft}";
    }

    private void SetGameFinishedText(string text)
    {
        _gameFinishedText.enabled = true;
        _gameFinishedText.text = text;
    }
    
}
