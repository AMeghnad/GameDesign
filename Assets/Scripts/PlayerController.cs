using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public InteractType interactType;
    public bool doorOpen = false;


    private Vector3 direction = Vector3.zero;

    public CharacterController controller;

    // Start is called just before any of the Update methods is called the first time
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        doorOpen = false;
    }

    void OnDrawGizmosSelected()
    {
        Transform camTransform = Camera.main.transform;
        Vector3 camEuler = camTransform.eulerAngles;
        Quaternion rotation = Quaternion.AngleAxis(camEuler.y, Vector3.up);
        Ray camRay = new Ray(camTransform.position, rotation * Vector3.forward);
        Gizmos.DrawLine(camRay.origin, camRay.origin + camRay.direction * 1000f);
    }

    void CheckInteract()
    {
        Transform camTransform = Camera.main.transform;
        Vector3 camEuler = camTransform.eulerAngles;
        Quaternion rotation = Quaternion.AngleAxis(camEuler.y, Vector3.up);
        Ray camRay = new Ray(camTransform.position, rotation * Vector3.forward);
        RaycastHit hit;
        // Fire ray out from camera
        if (Physics.Raycast(camRay, out hit, 1000f, hitLayers))
        {
            switch (interactType)
            {
                case InteractType.Untagged:
                    Debug.LogWarning("Invalid");
                    break;
                case InteractType.BUTTON:
                    if (interactType == InteractType.BUTTON)
                    {
                        doorOpen = true;
                    }
                    break;
                case InteractType.PUSHABLE:
                    // Hit an object
                    Rigidbody rigid = hit.collider.GetComponent<Rigidbody>();
                    if (rigid)
                    {
                        // Add force to object
                        rigid.AddForceAtPosition(-hit.normal * pushForce, hit.point);
                    }
                    break;
                case InteractType.MINIGAME:
                    break;
                case InteractType.DOOR:
                    if (doorOpen)
                    {
                        print("Level Complete! Loading next level...");
                        SceneManager.LoadScene(2);
                    }
                    if (!doorOpen)
                    {
                        print("You need to find a way to open the door. Try the red button");
                    }
                    break;
                default:
                    break;
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
                // Check which object you're interacting with
                CheckInteract();
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
        if (other.gameObject.layer == 10)
        {
            interactType = other.gameObject.GetComponent<Interactable>().interactType;
            Debug.Log(interactType);
        }
    }

    void OnTriggerExit(Collider other)
    {
        isInteracting = false;
        interactType = InteractType.Untagged;
    }
}
public enum InteractType
{
    Untagged = -1,
    BUTTON = 0,
    PUSHABLE = 1,
    MINIGAME = 2,
    DOOR = 3
}