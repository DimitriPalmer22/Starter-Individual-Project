using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalScript : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private PlayerScript _player;

    private float _timeLeft = 10;

    private int messCount;
    
    // Start is called before the first frame update
    void Start()
    {
        CountMesses();
        SetTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    private void CountMesses()
    {
        messCount = GameObject.FindGameObjectsWithTag("Mess").Length;
    }

    private void UpdateTimer()
    {
        _timeLeft -= Time.deltaTime;

        if (_timeLeft < 0)
        {
            _timeLeft = 0;

            messCount = GameObject.FindGameObjectsWithTag("Mess").Length;

            // Lose Condition
            if (messCount > 0)
            {
                
            }
            
        }
        
        SetTimerText();
        
    }
    
    private void SetTimerText()
    {
        _timerText.text = $"Time Left: {_timeLeft}";
    }
}
