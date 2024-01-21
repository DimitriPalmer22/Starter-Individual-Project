using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalScript : MonoBehaviour
{
    [Header("Text")] 
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _gameFinishedText;
    [SerializeField] private TMP_Text _gameIntroText;
    [SerializeField] private TMP_Text _messCountText;

    [Header("Audio")] 
    [SerializeField] private AudioClip _introAudio;
    [SerializeField] private AudioClip _gameplayAudio;
    [SerializeField] private AudioClip _winAudio;
    [SerializeField] private AudioClip _loseAudio;
    
    private AudioSource _audioSource;
    
    private float _timeLeft = 10;

    private int _messCount;

    private bool _gameStarted;
    private bool _gameLost, _gameWon;

    public bool GameStarted => _gameStarted;
    public bool GameFinished => _gameLost || _gameWon;

    private float _introTimer = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        PlayAudioClip(_introAudio);
            
        CountMesses();
        SetTimerText(_introTimer);

        _gameIntroText.enabled = true;
        _gameFinishedText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameStarted)
        {
            _introTimer -= Time.deltaTime;
            SetTimerText(_introTimer);
            
            if (_introTimer <= 0)
                StartGame();
        }
        
        UpdateTimer();
    }

    private void StartGame()
    {
        _gameIntroText.enabled = false;
        _gameStarted = true;
        
        PlayAudioClip(_gameplayAudio);
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
            SetGameFinishedText("You Win!\nYou cleaned all the garbage!", true);
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
                SetGameFinishedText("You Lose.\nYou failed to clean all the garbage within 10 seconds.", false);
            }
            
        }
        
        SetTimerText(_timeLeft);
    }
    
    /// <summary>
    /// Updates the UI element that has the timer info.
    /// </summary>
    private void SetTimerText(float time)
    {
        _timerText.text = $"Time Left:\n{time}";
    }

    /// <summary>
    /// Function used to change text displayed at the end of the game.
    /// Also used to call the function that plays the winning and losing sounds.
    /// </summary>
    /// <param name="text">The words of the text.</param>
    /// <param name="gameWon">True = game won. False = game lost</param>
    private void SetGameFinishedText(string text, bool gameWon)
    {
        _gameFinishedText.enabled = true;
        _gameFinishedText.text = text;

        AudioClip clip = _winAudio;
        if (!gameWon)
            clip = _loseAudio;

        PlayAudioClip(clip);
    }

    private void UpdateMessCountText()
    {
        _messCountText.text = $"Garbage Remaining: {_messCount}";
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (clip == null)
            return;

        if (clip == _gameplayAudio)
            _audioSource.volume = .4f;
        else
            _audioSource.volume = .8f;
        
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
    
}
