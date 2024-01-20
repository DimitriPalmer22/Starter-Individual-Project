using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrubScript : MonoBehaviour
{
    private const int SoapMultiplierResetScrubs = 8;

    [SerializeField] private TMP_Text _timerText;
    
    [SerializeField] private GameObject _sponge;
    [SerializeField] private Vector2 _spongePosition1;
    [SerializeField] private Vector2 _spongePosition2;

    private readonly KeyCode _scrubButton1 = KeyCode.Q;
    private readonly KeyCode _scrubButton2 = KeyCode.E;
    private readonly KeyCode _soapButton = KeyCode.Space;
    private bool _scrubState = false; // if false, press scrub button 1, else, press scrub button 2

    private float _scrubCooldown = 0;
    private float _soapCooldown = 0;
    
    private float _timeRemaining = 10;
    private int _dirtRemaining = 128;

    [SerializeField] private int _soapMultiplier = 1;
    private int _soapMultiplierResetCounter = SoapMultiplierResetScrubs;

    private bool _gameFinished = false;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameFinished)
            return;

        SoapInput();
        ScrubInput();
        
        _timeRemaining -= Time.deltaTime;
        _scrubCooldown -= Time.deltaTime;
        _soapCooldown -= Time.deltaTime;

        // Lose Condition
        if (_timeRemaining <= 0 && _dirtRemaining > 0)
        {
            _gameFinished = true;
        }
        
        UpdateTimerText();
    }

    void ScrubInput()
    {
        if (_scrubCooldown > 0)
            return;
        
        if (!_scrubState && Input.GetKeyDown(_scrubButton1))
            CleanDirt();
        else if (_scrubState && Input.GetKeyDown(_scrubButton2))
            CleanDirt();
    }

    void SoapInput()
    {
        if (_soapCooldown > 0)
            return;

        if (!Input.GetKeyDown(_soapButton))
            return;

        if (_soapMultiplier >= 4)
        {
            return;
        }

        _scrubCooldown = _soapCooldown = .5f;
        _soapMultiplier += 1;
        _soapMultiplierResetCounter = SoapMultiplierResetScrubs;
        
        Debug.Log($"SPRAYED SOAP: {_soapMultiplier}");
    }

    private void CleanDirt()
    {
        _scrubState = !_scrubState;
        _dirtRemaining -= 1 * _soapMultiplier;

        if (_soapMultiplier > 1)
        {
            _soapMultiplierResetCounter -= 1;
            if (_soapMultiplierResetCounter <= 0)
            {
                _soapMultiplierResetCounter = SoapMultiplierResetScrubs;
                _soapMultiplier -= 1;
            }
        }
        
        Debug.Log($"SCRUB {_dirtRemaining}");

        // Win condition
        if (_dirtRemaining <= 0)
            _gameFinished = true;
    }

    private void UpdateTimerText()
    {
        _timerText.text = $"Time Left: {_timeRemaining}";
    }
}
