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

    private Vector3 direction = Vector3.zero;

    public CharacterController controller;

    // Update is called once per frame
    void Update()
    {
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        // If interacting
        if (Input.GetMouseButtonDown(0))
        {
            interact.SetActive(true);
            Debug.Log("Interacting");
        }
        else
        {
            interact.SetActive(false);
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
}
