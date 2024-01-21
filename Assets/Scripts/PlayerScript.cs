using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // how close the player has to be to a mess before they are able to start cleaning it
    private const float MessCleanProximity = 1.5f;

    private GlobalScript _globalScript;
    
    private MessScript _currentlyCleaning;

    [SerializeField] private float movementSpeed = 4;
    
    private KeyCode _scrubButton = KeyCode.E;

    private void Start()
    {
        _globalScript = GameObject.FindWithTag("Global").GetComponent<GlobalScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_globalScript.GameStarted || _globalScript.GameFinished)
            return;
        
        MovementInput();
        ProximityCheck();
        ScrubInput();
    }

    /// <summary>
    /// Use WASD to move around.
    /// Disables movement while cleaning.
    /// </summary>
    private void MovementInput()
    {
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");

        var movementVector = new Vector3(horizontalMovement, verticalMovement, 0).normalized;

        transform.position += movementVector * (movementSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Finds the closest mess to the player.
    /// Sets the "currentlyCleaning" variable.
    /// </summary>
    private void ProximityCheck()
    {
        // Get all mess objects
        var messObjects = GameObject.FindGameObjectsWithTag("Mess");
        
        // If there are no more messes, skip this function
        if (messObjects.Length <= 0)
        {
            _currentlyCleaning = null;
            return;
        }
        
        // get the closest mess
        GameObject closestMess = messObjects
            .OrderBy(n => Vector2.Distance(transform.position, n.transform.position))
            .First();

        // test to see if it is in range
        var messDistance = Vector2.Distance(transform.position, closestMess.transform.position);
        if (messDistance > MessCleanProximity)
        {
            _currentlyCleaning = null;
            return;
        }
        
        _currentlyCleaning = closestMess.GetComponent<MessScript>();
    }

    /// <summary>
    /// Press Q and E rapidly to scrub away the messes.
    /// This function only runs if the player is cleaning.
    /// </summary>
    private void ScrubInput()
    {
        if (_currentlyCleaning == null)
            return;
        
        bool doneCleaning = false;

        // Scrub the mess
        if (Input.GetKeyDown(_scrubButton))
            doneCleaning = _currentlyCleaning.Scrub();
        
        if (!doneCleaning)
            return;
        
        // If the player is done cleaning the mess,
        // release the player from isCleaning and allow them to move again
        _currentlyCleaning = null;
        _globalScript.MessCleaned();
    }
}
