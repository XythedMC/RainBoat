using System;
using TMPro;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject Timer;
    [SerializeField] private GameObject NextButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Timer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Timer.GetComponent<Timer>().enabled = false;
        Timer.GetComponent<TextMeshProUGUI>().text = "You Win!";
        NextButton.SetActive(true);
    }
}
