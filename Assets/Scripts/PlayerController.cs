using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cam;
    public GameObject interact;

    public float inputH, inputV;
    public float speed = 5f;
    public float jumpSpeed = 10f;
    public float gravity = 20f;
    public float pushForce = 10f;
    public bool isInteracting = false; // Sets to true when colliding with interactable
    public LayerMask hitLayers;

    private Vector3 direction = Vector3.zero;

    public CharacterController controller;

    // Start is called just before any of the Update methods is called the first time
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDrawGizmosSelected()
    {
        Transform camTransform = Camera.main.transform;
        Vector3 camEuler = camTransform.eulerAngles;
        Quaternion rotation = Quaternion.AngleAxis(camEuler.y, Vector3.up);
        Ray camRay = new Ray(camTransform.position, rotation * Vector3.forward);
        Gizmos.DrawLine(camRay.origin, camRay.origin + camRay.direction * 1000f);
    }

    void Interact()
    {
        Transform camTransform = Camera.main.transform;
        Vector3 camEuler = camTransform.eulerAngles;
        Quaternion rotation = Quaternion.AngleAxis(camEuler.y, Vector3.up);
        Ray camRay = new Ray(camTransform.position, rotation * Vector3.forward);
        RaycastHit hit;
        // Fire ray out from camera
        if (Physics.Raycast(camRay, out hit, 1000f, hitLayers))
        {
            // Hit an object
            Rigidbody rigid = hit.collider.GetComponent<Rigidbody>();
            if (rigid)
            {
                // Add force to object
                rigid.AddForceAtPosition(-hit.normal * pushForce, hit.point);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        if (isInteracting)
        {
            // If the interact button is pressed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Interact();
            }
        }

        if (controller.isGrounded)
        {

            // Rotate the player in the direction of the camera
            Vector3 euler = cam.transform.eulerAngles;
            transform.rotation = Quaternion.AngleAxis(euler.y, Vector3.up);

            direction = new Vector3(inputH, 0, inputV);
            direction = transform.TransformDirection(direction);
            direction *= speed;

            if (Input.GetButtonDown("Jump"))
            {
                direction.y = jumpSpeed;
            }
        }
        direction.y -= gravity * Time.deltaTime;
        controller.Move(direction * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        isInteracting = true;
    }

    void OnTriggerExit(Collider other)
    {
        isInteracting = false;
    }
}
