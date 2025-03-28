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
                        water.transform.position += new Vector3(0, FindWaterSpeed(waterUpSpeed));
                }
                break;
            }
            case Mode.Sun:
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Alpha5))
                {
                    if (water.transform.position.y > -10f)
                        water.transform.position -= new Vector3(0, FindWaterSpeed(waterDownSpeed));
                }
                break;
            }
            case Mode.Wind:
                return;
        }
    }
    
    /*
    private void OnDrawGizmos()
    {
        Vector2 rayOrigin = new Vector2(boat.transform.position.x, boat.transform.position.y + boat.transform.localScale.y / 2 + 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, 0.8f, LayerMask.GetMask("Ground"));
        Debug.Log(hit.transform.name);
        LayerMask mask = LayerMask.GetMask("Ground");
        Gizmos.DrawLine(boat.transform.position, new Vector3(boat.transform.position.x, boat.transform.localScale.y / 2 + 20f, 0));
        Gizmos.DrawLine(new Vector2(boat.transform.position.x, boat.transform.position.y + boat.transform.localScale.y / 2), new Vector3(boat.transform.position.x, boat.transform.position.y + boat.transform.localScale.y / 2 + 0.8f, 0));
        try
        {
            Gizmos.DrawSphere(hit.transform.position, 0.2f);
        }
        catch (Exception e)
        {
            if (e.GetType() != typeof(NullReferenceException))
                Debug.LogError(e.Message);
        }
        Gizmos.DrawSphere(rayOrigin, 0.2f);
    }
    */

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

    private float FindWaterSpeed(float speed)
    {
        if (Input.GetKey(KeyCode.Alpha1)) return speed / 100 * 0.2f;
        if (Input.GetKey(KeyCode.Alpha2)) return speed / 100 * 0.4f;
        if (Input.GetKey(KeyCode.Alpha3)) return speed / 100 * 0.6f;
        if (Input.GetKey(KeyCode.Alpha4)) return speed / 100 * 0.8f;
        if (Input.GetKey(KeyCode.Alpha5)) return speed / 100;
        return speed / 100;
    }

    public void manageBlur(List<GameObject> objectsToBlur, string blurLayer)
    {
        foreach (GameObject obj in objectsToBlur)
        {
            obj.layer = LayerMask.NameToLayer(blurLayer);
        }
    }
}
