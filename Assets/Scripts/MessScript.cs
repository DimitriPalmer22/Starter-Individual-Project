using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessScript : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private int _health = 8;

    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _healthText.enabled = false;
        UpdateHealthText();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Take away 1 health from this mess.
    /// This function is only supposed to be called by the player script.
    /// </summary>
    /// <returns>A boolean representing whether this mess is fully clean.</returns>
    public bool Scrub()
    {
        if (_health <= 0)
            return true;
        
        _health -= 1;
        UpdateHealthText();
        
        if (_health > 0)
            return false;
        
        Destroy(gameObject);

        return true;
    }

    private void UpdateHealthText()
    {
        _healthText.text = $"Press E\n{_health} time(s)!";
    }

    public void EnableHealthText()
    {
        _healthText.enabled = true;
    }

    public void DisableHealthText()
    {
        _healthText.enabled = false;
    }
    
}
