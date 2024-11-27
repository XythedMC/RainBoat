using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class WeatherController : MonoBehaviour
{
    [SerializeField] public GameObject Boat;
    [SerializeField] public GameObject wheel;
    [SerializeField, Range(0, 8)] public float WindSpeed;
    [SerializeField, Range(1f, 10f)] public float WaterUpSpeed; // NEED TO DIVIDE VALUE BY 1000
    [SerializeField, Range(1f, 10f)] public float WaterDownSpeed;

    [Header("Locked Modes")]
    [SerializeField] public bool Rain;
    [SerializeField] public bool Sun;
    [SerializeField] public bool Wind;

    
    private Mode mode;


    private void Awake()
    {
        mode = wheel.GetComponent<WheelScript>().CurrentMode;
    }

    private void Update()
    {
        mode = wheel.GetComponent<WheelScript>().CurrentMode;
        WaterControl(mode);
        WindControl(mode);
    }

    public void WindControl(Mode curMode)
    {
        if (curMode == Mode.Wind)
        {
            if (Input.GetMouseButton(0))
            {
                Boat.GetComponent<Rigidbody2D>().AddForceX(WindSpeed);
            }
            else if (Input.GetMouseButton(1))
            {
                Boat.GetComponent<Rigidbody2D>().AddForceX(-WindSpeed);
            }
        }
    }

    public void WaterControl(Mode curMode)
    {
        if (curMode == Mode.Rain)
        {
            if (Input.GetMouseButton(0))
            {
                gameObject.transform.position += new Vector3(0, WaterUpSpeed / 1000);
            }
        }
        else if (curMode == Mode.Sun)
        {
            if (Input.GetMouseButton(0))
                gameObject.transform.position -= new Vector3(0, WaterDownSpeed / 1000);
        }
    }
}
