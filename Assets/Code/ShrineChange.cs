using UnityEngine;

public class ShrineChange : MonoBehaviour
{
    public PlayerColour colour;
    public GameObject player;
    PlayerController script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        script = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            script.ChangeColour(colour);
        }
    }
}
