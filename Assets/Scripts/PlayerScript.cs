using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // how close the player has to be to a mess before they are able to start cleaning it
    private const float MessCleanProximity = 1.5f;

    private GlobalScript _globalScript;
    
    private MessScript _currentlyCleaning;

    [SerializeField] private float movementSpeed = 4;

    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _particleSystem;

    private AudioSource _bubbleAudioSource;
    
    private readonly KeyCode _scrubButton = KeyCode.E;
    
    private Rigidbody2D _rb;


    private void Start()
    {
        _globalScript = GameObject.FindWithTag("Global").GetComponent<GlobalScript>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _particleSystem = GetComponent<ParticleSystem>();

        _bubbleAudioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Completely disables the player's script if the game hasn't started or the game has ended.
        if (!_globalScript.GameStarted || _globalScript.GameFinished)
            return;
        
        MovementInput();
        ProximityCheck();
        ScrubInput();
    }

    /// <summary>
    /// Use WASD to move around.
    /// Flip the player's sprite depending on which direction they are moving.
    /// </summary>
    private void MovementInput()
    {
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");

        var movementVector = new Vector3(horizontalMovement, verticalMovement, 0).normalized;

        _rb.velocity = movementVector * movementSpeed;
        
        // Flip the sprite left or right depending on the movement
        // This code assumes the original sprite is already facing right
        if (movementVector.x < 0) // going left
            _spriteRenderer.flipX = true;
        else if (movementVector.x > 0) // going right
            _spriteRenderer.flipX = false;

    }

    /// <summary>
    /// Finds the closest mess to the player.
    /// Sets the "currentlyCleaning" variable.
    /// </summary>
    private void ProximityCheck()
    {
        // Get all mess objects
        var messObjects = GameObject.FindGameObjectsWithTag("Mess");
        
        // Disable all the mess objects' texts
        foreach (var messObject in messObjects)
        {
            var messScript = messObject.GetComponent<MessScript>();
            messScript.DisableHealthText();
        }
        
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
        _currentlyCleaning.EnableHealthText();
    }

    /// <summary>
    /// Press Q and E rapidly to scrub away the messes.
    /// This function only runs if there is a mess in the player's proximity.
    /// </summary>
    private void ScrubInput()
    {
        if (_currentlyCleaning == null)
            return;
        
        bool doneCleaning = false;

        // Scrub the mess
        if (Input.GetKeyDown(_scrubButton))
        {
            doneCleaning = _currentlyCleaning.Scrub();
            
            // Do soap bubble particles
            ReleaseParticles();
            
            // Play bubble audio
            PlayBubbleAudio();
        }
        
        if (!doneCleaning)
            return;
        
        // If the player is done cleaning the mess,
        // release the player from isCleaning and allow them to move again
        _currentlyCleaning = null;
        _globalScript.GarbageCleaned();
    }

    /// <summary>
    /// Make the particle system release a wave of bubbles.
    /// </summary>
    private void ReleaseParticles()
    {
        _particleSystem.Emit(10);
    }

    private void PlayBubbleAudio()
    {
        if (_bubbleAudioSource.isPlaying)
            return;
        
        _bubbleAudioSource.Play();
    }

}
