using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ball : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;

    public TextMeshProUGUI scoreText;
    public GameObject winTextObject;
    public static int ballscore;


    void Start()
    {

        rb = GetComponent<Rigidbody>();
        SetCountText();
        winTextObject.SetActive(false);
        ballscore = 0;

    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag.

        if (other.gameObject.CompareTag("hole"))
        {

            rb.gameObject.SetActive(false);
            GameData.score++;
            ballscore += 1;
            SetCountText();

        }
    }
    void SetCountText()
    {
        scoreText.text = "Count: " + GameData.score.ToString();
        if (ballscore >= 9) {
            winTextObject.SetActive(true);
        }
    }

}
