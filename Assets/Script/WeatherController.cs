using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Serialization;

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
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Click");
            _rb2dBoat.AddForceX(windSpeed, ForceMode2D.Impulse);
        }
        else if (Input.GetMouseButton(1))
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
                if (Input.GetMouseButton(0))
                {
                    if (water.transform.position.y < -1f)
                        water.transform.position += new Vector3(0, waterUpSpeed / 100);
                }

                break;
            }
            case Mode.Sun:
            {
                if (Input.GetMouseButton(0))
                    if (water.transform.position.y > -10f)
                        water.transform.position -= new Vector3(0, waterDownSpeed / 100);
                break;
            }
            case Mode.Wind:
                return;
        }
    }
}
