using System;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class WaterCollisionTracker : MonoBehaviour
{
    [SerializeField] public GameObject wheel;
    [SerializeField] public GameObject boat;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hello");
        if (collision.gameObject == boat && wheel.GetComponent<WheelScript>().CurrentMode == Mode.Rain)
        {
            boat.transform.parent = gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == boat && wheel.GetComponent<WheelScript>().CurrentMode == Mode.Sun)
        {
            boat.transform.parent = null;
        }
    }
}
