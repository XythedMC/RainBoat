using System;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject Timer;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject NextButton;
    [SerializeField] private GameObject ReplayButton;
    private GameObject GameManager;

    private void Awake()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameManager.GetComponent<Timer>().enabled = false;
        Timer.SetActive(false);
        winText.SetActive(true);
        winText.GetComponent<TextMeshProUGUI>().text = "You Win!";
        NextButton.SetActive(true);
        ReplayButton.SetActive(true);
    }
}
