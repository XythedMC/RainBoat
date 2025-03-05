using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Serialization;
using System;

public class WeatherController : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] public GameObject boat;
    [SerializeField] public GameObject wheel;
    [SerializeField] public GameObject water;
    
    [Header("Values")]
    [SerializeField, Range(0, 1)] public float windSpeed;
    [SerializeField, Range(1f, 10f)] public float waterUpSpeed; // NEED TO DIVIDE VALUE BY 1000
    [SerializeField, Range(1f, 10f)] public float waterDownSpeed;

    [Header("Locked Modes")]
    [SerializeField] public bool rain;
    [SerializeField] public bool sun;
    [SerializeField] public bool wind;

    
    private Mode _mode;
    private WheelScript _wheelScript;
    private Rigidbody2D _rb2dBoat;

    private void Start()
    {
        
        _wheelScript = wheel.GetComponent<WheelScript>();
        _mode = _wheelScript.CurrentMode;
        _rb2dBoat = boat.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _mode = _wheelScript.CurrentMode;
        WaterControl(_mode);
        WindControl(_mode);
    }

    private void WindControl(Mode curMode)
    {
        if (curMode != Mode.Wind) return;
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("Click");
            _rb2dBoat.AddForceX(windSpeed, ForceMode2D.Impulse);
        }
        else if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("Click");
            _rb2dBoat.AddForceX(-windSpeed, ForceMode2D.Impulse);
        }
    }

    private void WaterControl(Mode curMode)
    {
        switch (curMode)
        {
            case Mode.Rain:
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Alpha5))
                {
                    if (water.transform.position.y < -1f && CanGoHigher(boat))
                        water.transform.position += new Vector3(0, FindWaterUpSpeed());
                }
                break;
            }
            case Mode.Sun:
            {
                    if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Alpha6) || Input.GetKey(KeyCode.Alpha7) || Input.GetKey(KeyCode.Alpha8) || Input.GetKey(KeyCode.Alpha9) || Input.GetKey(KeyCode.Alpha0))
                        if (water.transform.position.y > -10f)
                        water.transform.position -= new Vector3(0, FindWaterDownSpeed());
                break;
            }
            case Mode.Wind:
                return;
        }
    }

    private bool CanGoHigher(GameObject boat)
    {
        RaycastHit2D hit = Physics2D.Raycast(boat.transform.position, Vector2.up);
        if (hit)
        {
            Debug.Log(true);
            return true; 
        }
        return false;
    }

    private float FindWaterUpSpeed()
    {
        if (Input.GetKey(KeyCode.Alpha1)) { return waterUpSpeed / 100 * 0.2f; }
        else if (Input.GetKey(KeyCode.Alpha2)) { return waterUpSpeed / 100 * 0.4f; }
        else if (Input.GetKey(KeyCode.Alpha3)) { return waterUpSpeed / 100 * 0.6f; }
        else if (Input.GetKey(KeyCode.Alpha4)) { return waterUpSpeed / 100 * 0.8f; }
        else if (Input.GetKey(KeyCode.Alpha5)) { return waterUpSpeed / 100; }
        return waterUpSpeed / 100;
    }
    
    private float FindWaterDownSpeed()
    {
        if (Input.GetKey(KeyCode.Alpha6)) { return waterDownSpeed / 100 * 0.2f; }
        else if (Input.GetKey(KeyCode.Alpha7)) { return waterDownSpeed / 100 * 0.4f; }
        else if (Input.GetKey(KeyCode.Alpha8)) { return waterDownSpeed / 100 * 0.6f; }
        else if (Input.GetKey(KeyCode.Alpha9)) { return waterDownSpeed / 100 * 0.8f; }
        else if (Input.GetKey(KeyCode.Alpha0)) { return waterDownSpeed / 100; }
        return waterDownSpeed / 100;
    }
}
