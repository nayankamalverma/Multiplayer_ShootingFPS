using UnityEngine;

public class FaceObjectToCamera : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
