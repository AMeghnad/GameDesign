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
    public bool doorOpen = false;
    [SerializeField]
    GameObject stairs;


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
        if (Physics.Raycast(camRay, out hit, 1000f, hitLayers, QueryTriggerInteraction.Collide))
        {
            isInteracting = true;
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable)
            {
                InteractType interactType = interactable.type;
                switch (interactType)
                {
                    case InteractType.BUTTON:
                        doorOpen = true;
                        break;
                    case InteractType.PUSHABLE:
                        interactable.Push(-hit.normal * pushForce, hit.point);
                        break;
                    case InteractType.MINIGAME:
                        break;
                    case InteractType.DOOR:
                        if (doorOpen)
                        {
                            Debug.Log("Level Complete! Loading next level...");
                            SceneManager.LoadScene("Level2");
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
            else
            {
                Debug.Log("There is no 'Interactable' script attached to the thing we hit");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        // If the interact button is pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Check which object you're interacting with
            CheckInteract();
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

        if (doorOpen)
            stairs.SetActive(true);
    }
}
public enum InteractType
{
    BUTTON = 0,
    PUSHABLE = 1,
    MINIGAME = 2,
    DOOR = 3
}