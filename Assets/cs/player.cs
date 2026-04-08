using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb;
    public TextMeshProUGUI scoreText;

    public float speed2;
    public float tilt;
    //public Boundary boundary;

    public GameObject shot;
    public Transform shotspawn;
    private Rigidbody rb2;
    public float firerate;

    private float nextfire;

    public GameObject audio1;
    public GameObject audio2;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    // Speed at which the player moves.
    public float speed = 0;
    

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
        
  
    }
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextfire)
        {
            nextfire = Time.time + firerate;
            GameObject clone =
            Instantiate(shot, shotspawn.position, shotspawn.rotation);
            clone.SetActive(true);
            rb2 = clone.GetComponent<Rigidbody>();
            rb2.velocity = transform.forward * speed2;
        }
    }

    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate()
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);
    }
    void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag.

        if (other.gameObject.CompareTag("hole"))
        {

            //rb.gameObject.SetActive(false);
            GameData.score -= 1;
            scoreText.text = "Count: " + GameData.score.ToString();
            transform.position = new Vector3(0, 0.5f, 0);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PickUp")
        {
            GameObject clone1 = Instantiate(audio1, rb.position, rb.rotation);
            clone1.SetActive(true);
        }
        if (collision.gameObject.tag == "wall")
        {
            GameObject clone2 = Instantiate(audio2, rb.position, rb.rotation);
            clone2.SetActive(true);
        }
    }
}
