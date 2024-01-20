using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // how close the player has to be to a mess before they are able to start cleaning it
    private const float MessCleanProximity = 1.5f;
    
    private MessScript _currentlyCleaning;

    [SerializeField] private float movementSpeed = 4;
    
    private KeyCode _cleanButton = KeyCode.E;
    private KeyCode _scrubButton1 = KeyCode.Q;
    private KeyCode _scrubButton2 = KeyCode.E;
    private bool _scrubState; // if false, press scrub button 1, else, press scrub button 2

    private bool _isCleaning = false;

    // Update is called once per frame
    void Update()
    {
        MovementInput();
        ProximityCheck();
        StartCleaningInput();
        ScrubInput();
    }

    /// <summary>
    /// Use WASD to move around.
    /// Disables movement while cleaning.
    /// </summary>
    private void MovementInput()
    {
        if (_isCleaning)
            return;
        
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
        if (_isCleaning)
            return;
        
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
    /// Use E to start cleaning messes.
    /// This function puts the player into the isCleaning state.
    /// Skips the function if the player is already cleaning.
    /// </summary>
    private void StartCleaningInput()
    {
        if (_isCleaning || _currentlyCleaning == null)
            return;

        if (Input.GetKeyDown(_cleanButton))
        {
            _isCleaning = true;
        }
    }

    /// <summary>
    /// Press Q and E rapidly to scrub away the messes.
    /// This function only runs if the player is cleaning.
    /// </summary>
    private void ScrubInput()
    {
        if (!_isCleaning || _currentlyCleaning == null)
            return;
        
        bool doneCleaning = false;

        // Press Q when prompted
        bool qPressedProperly = !_scrubState && Input.GetKeyDown(_scrubButton1);
        // Press E when prompted
        bool ePressedProperly = _scrubState && Input.GetKeyDown(_scrubButton2);
        
        // Scrub the mess
        if (qPressedProperly || ePressedProperly)
        {
            doneCleaning = _currentlyCleaning.Scrub();
            _scrubState = !_scrubState;
        }   
        
        if (!doneCleaning)
            return;
        
        // If the player is done cleaning the mess,
        // release the player from isCleaning and allow them to move again
        _isCleaning = false;
        _currentlyCleaning = null;
    }
}
