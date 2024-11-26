using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class WaterController : MonoBehaviour
{
    [SerializeField] public GameObject wheel;
    [SerializeField] public GameObject boat;
    [SerializeField, Range(0.001f, 0.01f)] public float UpSpeed;
    [SerializeField, Range(0.001f, 0.01f)] public float DownSpeed;
    
    private WheelScript.Mode wheelMode;

    private void Awake()
    {
        wheelMode = wheel.GetComponent<WheelScript>().CurrentMode;
    }

    private void Update()
    {
        wheelMode = wheel.GetComponent<WheelScript>().CurrentMode;
        if (wheelMode == WheelScript.Mode.Rain)
        {
            if (Input.GetMouseButton(0))
            {
                gameObject.transform.position += new Vector3(0, UpSpeed);
            }
        }
        else if(wheelMode == WheelScript.Mode.Sun)
        {
            if (Input.GetMouseButton(0))
                gameObject.transform.position -= new Vector3(0, DownSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hello");
        if (collision.gameObject == boat && wheelMode == WheelScript.Mode.Rain)
        {
            boat.transform.parent = gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == boat || wheelMode == WheelScript.Mode.Sun)
        {
            boat.transform.parent = null;
        }
    }
}
