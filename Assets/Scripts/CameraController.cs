using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform bird;

    void Update()
    {
        transform.position = new Vector3(bird.position.x,
                                         transform.position.y,
                                         transform.position.z);
    }

    private void LateUpdate()
    {
        if (bird == null)
            return;

        transform.position = new Vector3(bird.position.x,
                                        transform.position.y,
                                        transform.position.z);
    }
}