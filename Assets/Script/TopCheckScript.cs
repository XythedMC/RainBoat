using UnityEngine;

public class TopCheckScript : MonoBehaviour
{
    public bool colliding = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        colliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        colliding = false;
    }
}
