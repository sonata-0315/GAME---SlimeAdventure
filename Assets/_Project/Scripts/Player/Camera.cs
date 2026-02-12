using UnityEngine;
using System.Collections;


public class Camera : MonoBehaviour
{

    //Simple Camera Lerp for gamfeel
    [Header("Settings")]
    public Transform target;
    [Range(0f, 10f)]
    public float smoothSpeed = 5f; 

    [Header("Offset")]
    public Vector3 offset = new Vector3(0, 0, -10);


    void Start()
    {
        // Start the ramp-up routine immediately
        StartCoroutine(RampCameraSpeed());
    }

    IEnumerator RampCameraSpeed()
    {
        
        float targetSpeed = smoothSpeed;

        
        smoothSpeed = 0.02f;

        
        float duration = 6f;
        float elapsed = 0f;

       
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
           
            smoothSpeed = Mathf.Lerp(0.1f, targetSpeed, elapsed / duration);

            yield return null;
        }

        
        smoothSpeed = targetSpeed;
    }

    void LateUpdate()
    {
     

        
        Vector3 offsetPosition = target.position + offset;

        
        
        Vector3 smoothPosition = Vector3.Lerp(transform.position, offsetPosition, smoothSpeed * Time.deltaTime);

        
        transform.position = smoothPosition;
    }
}
