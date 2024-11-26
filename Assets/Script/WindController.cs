using UnityEngine;

public class WindController : MonoBehaviour
{
    [SerializeField] public GameObject Boat;
    [SerializeField] public GameObject wheel;
    [SerializeField, Range(0, 8)] public float WindSpeed;

    public WheelScript.Mode mode;

    private void Awake()
    {
        mode = wheel.GetComponent<WheelScript>().CurrentMode;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mode = wheel.GetComponent<WheelScript>().CurrentMode;
        if (mode == WheelScript.Mode.Wind)
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
}
