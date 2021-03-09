using UnityEngine;

public class ColumnsMovingUpDown : MonoBehaviour
{
    public float speed;
    public float switchTime;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
        InvokeRepeating("Switch", 0, switchTime);
    }

    void Switch()
    {
        GetComponent<Rigidbody2D>().velocity *= -1;
    }
}
