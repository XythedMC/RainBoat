using System;
using System.Collections;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] public GameObject timer;
    [SerializeField] public float seconds;

    private TextMeshProUGUI textMesh;
    private RectTransform rectTransform;

    private void Awake()
    {
        textMesh = timer.GetComponent<TextMeshProUGUI>();
        rectTransform = timer.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (seconds > 0)
        {
            seconds -= Time.deltaTime;
            textMesh.text = "Seconds Left: " + Mathf.FloorToInt(seconds % 60).ToString();
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.zero;
            textMesh.text = "Time Over!";
        }
    }

    
}
