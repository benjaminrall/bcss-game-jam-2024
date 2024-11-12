using UnityEngine;

public class HideBlock : MonoBehaviour
{
    public GameObject player;
    public PlayerColour blockColour;
    PlayerController script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        script = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ensure we are correctly accessing the PlayerColour component from the player GameObject
        PlayerController test = player.GetComponent<PlayerController>();
        if (script.playerColour == blockColour)
        {
            // Hide the object
            Renderer objectRenderer = GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                objectRenderer.enabled = false;
            }

            // Disable the collider
            Collider2D objectCollider = GetComponent<Collider2D>();
            if (objectCollider != null)
            {
                objectCollider.enabled = false;
            }
        }
        else
        {
            // Show the object
            Renderer objectRenderer = GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                objectRenderer.enabled = true;
            }

            // Enable the collider
            Collider2D objectCollider = GetComponent<Collider2D>();
            if (objectCollider != null)
            {
                objectCollider.enabled = true;
            }
        }
    }
}
