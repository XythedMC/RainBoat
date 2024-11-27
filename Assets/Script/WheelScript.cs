using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WheelScript : MonoBehaviour
{
    [NonSerialized] public Mode CurrentMode;
    private int CurrentModeIndex;
    [SerializeField] public GameObject GameManager;
    private List<Mode> lockModes = new List<Mode>();
    private WeatherController wc;

    private void Awake()
    {
        wc = GameManager.GetComponent<WeatherController>();
        if (wc.Rain)
            lockModes.Add(Mode.Rain);
        if(wc.Sun)
            lockModes.Add(Mode.Sun);
        if (wc.Wind)
            lockModes.Add(Mode.Wind);
        if (!lockModes.Any())
        {
            Debug.LogError("NO MODE IS ENABLED! ENABLE AT LEAST ONE!");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RotationManager(CurrentMode);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(CurrentModeIndex);
        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentModeIndex++;
            if (CurrentModeIndex > lockModes.Count - 1)
            {
                CurrentModeIndex = 0;
            }
            CurrentMode = lockModes[CurrentModeIndex];
            Debug.Log(CurrentMode);
        } 
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.DownArrow))
        {
            CurrentModeIndex--;
            if (CurrentModeIndex == -1)
            {
                CurrentModeIndex = lockModes.Count - 1;
            }
            CurrentMode = lockModes[CurrentModeIndex];
            Debug.Log(CurrentMode);
        }
        RotationManager(CurrentMode);
    }

    public void RotationManager(Mode mode)
    {
        if (mode == Mode.Rain)
            gameObject.transform.eulerAngles = new Vector3(0,0,250);
        else if (mode == Mode.Sun)
            gameObject.transform.eulerAngles = new Vector3(0, 0, 114);
        else
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
[Serializable]
public enum Mode : int
{
    Rain = 0,
    Sun = 1,
    Wind = 2
}
