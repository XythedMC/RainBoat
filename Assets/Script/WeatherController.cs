using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Serialization;
using System;
using UnityEngine.UIElements;

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
    public ParticleSystem ps;
    
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
            _rb2dBoat.AddForceX(windSpeed, ForceMode2D.Impulse);
        else if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftArrow))
            _rb2dBoat.AddForceX(-windSpeed, ForceMode2D.Impulse);
    }

    private void WaterControl(Mode curMode)
    {
        switch (curMode)
        {
            case Mode.Rain:
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Alpha5))
                {
                    if (water.transform.position.y < -1f && CanGoHigher())
                        water.transform.position += new Vector3(0, FindWaterSpeed(waterUpSpeed, ps));
                }
                break;
            }
            case Mode.Sun:
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Alpha5))
                {
                    if (water.transform.position.y > -10f)
                        water.transform.position -= new Vector3(0, FindWaterSpeed(waterDownSpeed, ps));
                }
                break;
            }
            case Mode.Wind:
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

    private float FindWaterSpeed(float speed, ParticleSystem ps)
    {
        var emission = ps.emission;
        if (Input.GetKey(KeyCode.Alpha1))
        {
            ManageParticles(_mode, speed, ps, 0.2f);
            return speed / 100 * 0.2f;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            ManageParticles(_mode, speed, ps, 0.4f);
            emission.rateOverTime = 20;
            return speed / 100 * 0.4f;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            ManageParticles(_mode, speed, ps, 0.6f);
            return speed / 100 * 0.6f;
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            ManageParticles(_mode, speed, ps, 0.8f);
            return speed / 100 * 0.8f;
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            ManageParticles(_mode, speed, ps, 1f);
            return speed / 100;
        }
        ManageParticles(_mode, speed, ps, 1f);
        return speed / 100;
    }

    public void ManageParticles(Mode curMode, float speed, ParticleSystem ps, float mult)
    {
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
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
