using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalScript : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _gameFinishedText;
    [SerializeField] private TMP_Text _gameIntroText;
    [SerializeField] private TMP_Text _messCountText;
    
    [SerializeField] private PlayerScript _player;

    private float _timeLeft = 10;

    private int _messCount;

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
        _messCount = GameObject.FindGameObjectsWithTag("Mess").Length;
        UpdateMessCountText();
    }

    /// <summary>
    /// Called by the player script.
    /// Decrements the mess count variable.
    /// Checks if the player has won the game.
    /// </summary>
    public void MessCleaned()
    {
        _messCount -= 1;
        UpdateMessCountText();
        
        // Win Condition
        if (_messCount <= 0 && _timeLeft > 0)
        {
            _gameWon = true;
            _gameLost = false;
            SetGameFinishedText("You Win!\nYou cleaned all the messes!", true);
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
            if (_messCount > 0)
            {
                _gameLost = true;
                _gameWon = false;
                SetGameFinishedText("You Lose.\nYou failed to clean all the messes in 10 seconds.", false);
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

    /// <summary>
    /// Function used to change text displayed at the end of the game.
    /// </summary>
    /// <param name="text">The words of the text.</param>
    /// <param name="gameWon">True = game won. False = game lost</param>
    private void SetGameFinishedText(string text, bool gameWon)
    {
        _gameFinishedText.enabled = true;
        _gameFinishedText.text = text;
    }

    private void UpdateMessCountText()
    {
        _messCountText.text = $"Dirt Remaining: {_messCount}";
    }
    
}
