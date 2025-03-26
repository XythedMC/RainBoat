using UnityEngine;

public class WindController : MonoBehaviour
{
    [SerializeField] public GameObject Boat;
    [SerializeField] public GameObject wheel;
    [SerializeField, Range(0, 8)] public float WindSpeed;

    public Mode mode;

    private void Awake()
    {
        mode = wheel.GetComponent<WheelScript>().CurrentMode;
    }
   

    // Update is called once per frame
    void Update()
    {
        mode = wheel.GetComponent<WheelScript>().CurrentMode;
    }
}
