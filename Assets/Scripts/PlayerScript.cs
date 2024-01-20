using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // how close the player has to be to a mess before they are able to start cleaning it
    private const float MESS_CLEAN_PROXIMITY = 5;
    
    private MessScript _currentlyCleaning;


    [SerializeField] private float movementSpeed = 4;
    
    private KeyCode _cleanButton = KeyCode.E;

    private bool IsCleaning => _currentlyCleaning != null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
    }

    private void MovementInput()
    {
        if (IsCleaning)
            return;
        
        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");

        var movementVector = new Vector3(horizontalMovement, verticalMovement, 0).normalized;

        transform.position += movementVector * (movementSpeed * Time.deltaTime);
    }
    
}
