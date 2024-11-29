using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject Timer;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject NextButton;
    [SerializeField] private GameObject ReplayButton;
    [SerializeField] private GameObject BlurEffect;
    private GameObject GameManager;
    private Volume volume;
    private PlayerControlManager pControls;
    
    private void Awake()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        pControls = GameManager.GetComponent<PlayerControlManager>();
        volume = BlurEffect.GetComponent<Volume>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameManager.GetComponent<Timer>().enabled = false;
        Timer.SetActive(false);
        winText.SetActive(true);
        winText.GetComponent<TextMeshProUGUI>().text = "You Win!";
        NextButton.SetActive(true);
        ReplayButton.SetActive(true);
        volume.enabled = true;
        pControls.DisableInput();
    }
}
