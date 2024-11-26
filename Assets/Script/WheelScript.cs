using System;
using UnityEngine;

public class WheelScript : MonoBehaviour
{
    public enum Mode
    {
        Rain,
        Wind,
        Sun
    }
    public Mode CurrentMode;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RotationManager(CurrentMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if ((int)CurrentMode < 2)
            {
                CurrentMode = (Mode)((int)CurrentMode + 1);
            }
            else if ((int)CurrentMode == 2)
            {
                CurrentMode = (Mode)((int) CurrentMode - 2);
            }
            Console.WriteLine(CurrentMode);
        } 
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if ((int)CurrentMode > 0) { CurrentMode = (Mode)((int)CurrentMode - 1); }
            else
            {
                CurrentMode = Mode.Sun;
            }
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
