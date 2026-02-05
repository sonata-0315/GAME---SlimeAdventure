using UnityEngine;


public class Camera : MonoBehaviour
{

    //Simple Camera Lerp for gamfeel
    [Header("Settings")]
    public Transform target;
    [Range(0f, 10f)]
    public float smoothSpeed = 5f; 

    [Header("Offset")]
    public Vector3 offset = new Vector3(0, 0, -10); 

    void LateUpdate()
    {
     

        
        Vector3 offsetPosition = target.position + offset;

        
        
        Vector3 smoothPosition = Vector3.Lerp(transform.position, offsetPosition, smoothSpeed * Time.deltaTime);

        
        transform.position = smoothPosition;
    }
}
