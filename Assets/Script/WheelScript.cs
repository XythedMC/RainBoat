using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class WheelScript : MonoBehaviour
{
    [NonSerialized] public Mode CurrentMode;
    private int _currentModeIndex;
    [FormerlySerializedAs("GameManager")] [SerializeField] public GameObject gameManager;
    private readonly List<Mode> _lockModes = new();
    private WeatherController _wc;

    private void Awake()
    {
        _wc = gameManager.GetComponent<WeatherController>();
        if (_wc.rain)
            _lockModes.Add(Mode.Rain);
        if(_wc.sun)
            _lockModes.Add(Mode.Sun);
        if (_wc.wind)
            _lockModes.Add(Mode.Wind);
        if (!_lockModes.Any())
        {
            Debug.LogError("NO MODE IS ENABLED! ENABLE AT LEAST ONE!");
        }
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        CurrentMode = _lockModes[0];
        RotationManager(CurrentMode);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _currentModeIndex++;
            if (_currentModeIndex > _lockModes.Count - 1)
            {
                _currentModeIndex = 0;
            }
            CurrentMode = _lockModes[_currentModeIndex];
        } 
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _currentModeIndex--;
            if (_currentModeIndex == -1)
            {
                _currentModeIndex = _lockModes.Count - 1;
            }
            CurrentMode = _lockModes[_currentModeIndex];
        }
        RotationManager(CurrentMode);
    }

    private void RotationManager(Mode mode)
    {
        switch (mode)
        {
            case Mode.Rain:
                gameObject.transform.eulerAngles = new Vector3(0,0,240);
                break;
            case Mode.Sun:
                gameObject.transform.eulerAngles = new Vector3(0, 0, 120);
                break;
            default:
                gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
        }
    }
}
[Serializable]
public enum Mode
{
    Rain = 0,
    Sun = 1,
    Wind = 2
}
