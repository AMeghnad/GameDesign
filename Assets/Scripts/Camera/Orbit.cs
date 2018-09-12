using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public bool hideCursor = false;
    public Transform target;
    public Vector3 offset = new Vector3(0, 1f, 0);
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    [Header("Collision")]
    public bool cameraCollision = false;
    public float camRadius = 0.3f;
    public float rayDistance = 1000f;
    public LayerMask ignoreLayers;

    private Vector3 originalOffset;
    private float x = 0.0f;
    private float y = 0.0f;

    // Use this for initialisation
    void Start()
    {
        // Calculate offset of camera at start
        originalOffset = transform.position - target.position;
        // Ray distance is as long as the magnitude of offset
        rayDistance = originalOffset.magnitude;

        // Convert camera angles to vectors
        Vector3 angles = transform.eulerAngles;
        // Determine the x and y components of each camera vector
        x = angles.y;
        y = angles.x;

        // Unparent the camera from the player
        transform.SetParent(null);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, camRadius);
    }

    public void Look(float mouseX, float mouseY)
    {
        // The camera vector's components are determined by mouse position
        x += mouseX * xSpeed * Time.deltaTime;
        y += mouseY * ySpeed * Time.deltaTime;

        // Clamp the rotation 
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        // Get the values of the camera rotation as a Vector3
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        // Player rotation is the same as camera rotation
        transform.rotation = rotation;
    }

    void FixedUpdate()
    {
        // If the target is the focus
        if (target)
        {
            // If camera collision is on
            if (cameraCollision)
            {
                // Create a new ray from the target's position out of the screen
                Ray camRay = new Ray(target.position, -transform.forward);
                RaycastHit hit;
                if (Physics.SphereCast(camRay, camRadius, out hit, rayDistance, ~ignoreLayers, QueryTriggerInteraction.Ignore))
                {
                    distance = hit.distance;
                    //return
                    return;
                }
            }

            // Reset distance if not hitting
            distance = originalOffset.magnitude;
        }

    }

    void LateUpdate()
    {
        if (target)
        {
            transform.position = target.position + -transform.forward * distance;
        }

        // If the cursor is supposed to be hidden
        if (hideCursor)
        {
            // Lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            // Hide the cursor from the player
            Cursor.visible = false;
        }
        else
        {
            // Keep the cursor within the window
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    // Define the angles which limit the camera rotation
    public static float ClampAngle(float angle, float min, float max)
    {
        // Define the angular limits
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}