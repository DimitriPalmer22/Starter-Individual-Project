using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessScript : MonoBehaviour
{

    private int _health = 8;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
        
        if (_health > 0)
            return false;
        
        Destroy(gameObject);

        return true;
    }
    
}
