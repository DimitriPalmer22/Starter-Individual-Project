using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GlobalScript : MonoBehaviour
{
    [Header("Text")] 
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _gameFinishedText;
    [SerializeField] private TMP_Text _gameIntroText;
    [SerializeField] private TMP_Text _garbageCountText;
    [SerializeField] private TMP_Text _restartText;

    [Header("Audio")] 
    [SerializeField] private AudioClip _introAudio;
    [SerializeField] private AudioClip _gameplayAudio;
    [SerializeField] private AudioClip _winAudio;
    [SerializeField] private AudioClip _loseAudio;
    
    private AudioSource _audioSource;
    

    private int _garbageCount;

    private bool _gameStarted;
    private bool _gameLost, _gameWon;

    public bool GameStarted => _gameStarted;
    public bool GameFinished => _gameLost || _gameWon;

    private float _introTimer = 2f;
    private float _gameplayTimer = 10;
    private float _postGameTimer = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        PlayAudioClip(_introAudio);
            
        CountGarbage();
        SetTimerText(_introTimer);

        _gameIntroText.enabled = true;
        _gameFinishedText.enabled = false;
        _restartText.enabled = false;
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

        if (GameFinished && _postGameTimer > 0)
        {
            _postGameTimer -= Time.deltaTime;
            SetTimerText(_postGameTimer);
            if (_postGameTimer <= 0)
            {
                _postGameTimer = 0;

                _timerText.enabled = false;
                _gameIntroText.enabled = false;
                _garbageCountText.enabled = false;
                _gameFinishedText.enabled = false;
                _restartText.enabled = true;
            }
        }
        
        else if (_postGameTimer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(0);
        }
    }

    private void StartGame()
    {
        _gameIntroText.enabled = false;
        _gameStarted = true;
        
        PlayAudioClip(_gameplayAudio);
    }

    /// <summary>
    /// Updates the garbageCount variable.
    /// Only needs to be called in the start function
    /// </summary>
    private void CountGarbage()
    {
        _garbageCount = GameObject.FindGameObjectsWithTag("Mess").Length;
        UpdateGarbageCountText();
    }

    /// <summary>
    /// Called by the player script.
    /// Decrements the garbage count variable.
    /// Checks if the player has won the game.
    /// </summary>
    public void GarbageCleaned()
    {
        _garbageCount -= 1;
        UpdateGarbageCountText();
        
        // Win Condition
        if (_garbageCount <= 0 && _gameplayTimer > 0)
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
        
        _gameplayTimer -= Time.deltaTime;

        if (_gameplayTimer < 0)
        {
            _gameplayTimer = 0;

            // Lose Condition
            if (_garbageCount > 0)
            {
                _gameLost = true;
                _gameWon = false;
                SetGameFinishedText("You Lose.\nYou failed to clean all the garbage within 10 seconds.", false);
            }
            
        }
        
        SetTimerText(_gameplayTimer);
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

    private void UpdateGarbageCountText()
    {
        _garbageCountText.text = $"Garbage Remaining: {_garbageCount}";
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
