using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public PlayerController player;

    public Image ammoFill;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ammoFill.fillAmount = player.AmmoPercentage;

    }
}
