using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Serialization;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Collections;

public class WeatherController : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] public GameObject boat;
    [SerializeField] public GameObject wheel;
    [SerializeField] public GameObject water;
    [SerializeField] public List<GameObject> objectsToBlur;
    [SerializeField] private Material rainMat;
    [SerializeField] private Material dryMat;
    [SerializeField] private Material windMat;
    [SerializeField] public Controller controller;
    public ParticleSystem ps;

    [Header("Values")]
    [SerializeField, Range(0, 1)] public float windSpeed;
    [SerializeField, Range(1f, 10f)] public float waterUpSpeed; // NEED TO DIVIDE VALUE BY 1000
    [SerializeField, Range(1f, 10f)] public float waterDownSpeed;

    public int[] values = new int[3] {0,0,1};
    public int[] prevValues = new int[3];
    public float updateTimer = 0f;
    
    [Header("Locked Modes")]
    [SerializeField] public bool rain;
    [SerializeField] public bool sun;
    [SerializeField] public bool wind;
    
    private Mode _mode;
    private WheelScript _wheelScript;
    private Rigidbody2D _rb2dBoat;

    private void Start()
    {
        Array.Copy(values, prevValues, 3);
        _wheelScript = wheel.GetComponent<WheelScript>();
        _mode = _wheelScript.CurrentMode;
        _rb2dBoat = boat.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Debug.Log(string.Join("", prevValues) + " " + string.Join("", values));
        updateTimer += Time.fixedDeltaTime;
        if (updateTimer > 4.0f)
        {
            Array.Copy(values, prevValues, 3);
            updateTimer = 0f;
        }
        
        if (controller.lastReceivedMessage != "")
        {
            try
            {
                values[0] = controller.lastReceivedMessage[2] - '0';
                values[1] = controller.lastReceivedMessage[6] - '0';
                values[2] = controller.lastReceivedMessage[10] - '0';
            }
            catch (Exception e)
            {
                values[0] = 0;
                values[1] = 0;
                values[2] = 1;
                if (e.GetType() != typeof(NullReferenceException))
                    Debug.LogError(e.Message);
            }
        }
        
        if (values != prevValues)
        {
            if (values[0] != prevValues[0])
                _wheelScript.CurrentMode = Mode.Rain;
            else if (values[1] != prevValues[1])
                _wheelScript.CurrentMode = Mode.Sun;
            else if (values[2] != prevValues[2])
                _wheelScript.CurrentMode = Mode.Wind;
        }
            
        _mode = _wheelScript.CurrentMode;

        Weather(_mode);
    }

    private void Weather(Mode curMode)
    {
        switch (curMode)
        {
            case Mode.Rain:
            {
                if (Input.GetMouseButton(0)  || Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Alpha5) 
                    || values[0] == 1 || values[0] == 2 || values[0] == 3 || values[0] == 4 || values[0] == 5)
                {
                    //Debug.Log(WindValue);
                    if (water.transform.position.y < -1f && CanGoHigher())
                        water.transform.position += new Vector3(0, FindWaterSpeed(values[0], waterUpSpeed, ps));
                }
                break;
            }
            case Mode.Sun:
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Alpha5)
                    || values[1] == 1 || values[1] == 2 || values[1] == 3 || values[1] == 4 || values[1] == 5)
                {
                    if (water.transform.position.y > -10f)
                        water.transform.position -= new Vector3(0, FindWaterSpeed(values[1], waterDownSpeed, ps));
                }
                break;
            }
            case Mode.Wind:
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.RightArrow) || values[2] == 2)
                    _rb2dBoat.AddForceX(windSpeed, ForceMode2D.Impulse);
                else if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftArrow) || values[2] == 0)
                    _rb2dBoat.AddForceX(-windSpeed, ForceMode2D.Impulse);
                ManageParticles(_mode, 0f, ps, 0f);
                return;
        }
    }
    
    private bool CanGoHigher()
    {
        Vector2 rayOrigin = new Vector2(boat.transform.position.x, boat.transform.position.y + boat.transform.localScale.y / 2 + 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, 0.8f, LayerMask.GetMask("Ground"));
        try
        {
            if (hit)
                return false;
        }
        catch (Exception e)
        {
            if (e.GetType() != typeof(NullReferenceException))
                Debug.LogError(e.Message);
        }
        return true;
    }

    private float FindWaterSpeed(int message, float speed, ParticleSystem particleSystem)
    {
        if (Input.GetKey(KeyCode.Alpha1) || message == 1)
        {
            ManageParticles(_mode, speed, particleSystem, 0.2f);
            return speed / 100 * 0.2f;
        }
        if (Input.GetKey(KeyCode.Alpha2) || message == 2)
        {
            ManageParticles(_mode, speed, particleSystem, 0.4f);
            return speed / 100 * 0.4f;
        }
        if (Input.GetKey(KeyCode.Alpha3) || message == 3)
        {
            ManageParticles(_mode, speed, particleSystem, 0.6f);
            return speed / 100 * 0.6f;
        }
        if (Input.GetKey(KeyCode.Alpha4) || message == 4)
        {
            ManageParticles(_mode, speed, particleSystem, 0.8f);
            return speed / 100 * 0.8f;
        }
        if (Input.GetKey(KeyCode.Alpha5) || message == 5)
        {
            ManageParticles(_mode, speed, particleSystem, 1f);
            return speed / 100;
        }

        return 0;
    }

    public void ManageParticles(Mode curMode, float speed, ParticleSystem ps, float mult)
    {
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        if (!renderer.enabled) renderer.enabled = true;
        var emission = ps.emission;

        switch (curMode)
        {
            case Mode.Wind:
                emission.rateOverTime = 0;
                renderer.material = windMat;
                return;

            case Mode.Sun:
                emission.rateOverTime = 0;
                renderer.material = dryMat;
                return;

            case Mode.Rain:
                emission.rateOverTime = Mathf.Round(speed / 2 * mult) * 10;
                renderer.material = rainMat;
                return;
        }
        emission.enabled = false;
    }


    public void manageBlur(List<GameObject> objectsToBlur, string blurLayer)
    {
        foreach (GameObject obj in objectsToBlur)
        {
            obj.layer = LayerMask.NameToLayer(blurLayer);
        }
    }
}
