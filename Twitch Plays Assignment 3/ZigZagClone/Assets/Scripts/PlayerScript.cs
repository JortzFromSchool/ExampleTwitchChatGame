using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    /// <summary>
    /// The player's movement speed
    /// </summary>
    public float speed;

    /// <summary>
    /// The direction the player is traveling ing
    /// </summary>
    private Vector3 dir;

    /// <summary>
    /// A reference to the player's particle system
    /// </summary>
    public GameObject ps;

    /// <summary>
    /// Indicates if the player is dead
    /// </summary>
    private bool isDead;

    /// <summary>
    /// A reference to the reset button
    /// </summary>
    public GameObject resetBtn;

    /// <summary>
    /// The player's current score
    /// </summary>
    private int score = 0;
    
    /// <summary>
    /// The score text in the top left corner
    /// </summary>
    public Text scoreText;

    /// <summary>
    /// The menu's aniamtor
    /// </summary>
    public Animator gameOverAnim;

    /// <summary>
    /// The text, that tells the player that a new highscore has been earned
    /// </summary>
    public Text newHighScore;

    /// <summary>
    /// The menu background
    /// </summary>
    public Image background;

    /// <summary>
    /// This array contains all the text in the menu
    /// </summary>
    public Text[] scoreTexts;

    public Transform contactPoint;

    public LayerMask whatIsGround;

    public bool playing = false;

	// Use this for initialization
	void Start () 
    {   
        //Makes sure that the player is alive when the game starts
        isDead = false;

        //Sets the player's direction to 0, so that the player doesn't move whent he game start
        dir = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (!IsGrounded() && playing)
        {

            isDead = true; //Kills the player

            GameOver();

            resetBtn.SetActive(true); //Shows the reset button

            if (transform.childCount > 0) //Stops the camera from following the player
            {
                transform.GetChild(0).transform.parent = null;
            }
        }

        //If we click on the screen or the first mouse button
        if (Input.GetMouseButtonDown(0) && !isDead)
        {
            playing = true;
            score++;
            scoreText.text = score.ToString();

            //Switches the players direction every time we click ont he screen or mouse
            if (dir == Vector3.forward)
            {
                dir = Vector3.left;
            }
            else
            {
                dir = Vector3.forward;
            }
        }

        //Calculates the player's movement
        float amoutToMove = speed * Time.deltaTime;

        //Makes the player move
        transform.Translate(dir * amoutToMove);
	}

    /// <summary>
    /// When the player enters a trigger
    /// </summary>
    /// <param name="other">the trigger it collides with</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickup") //If its a pick up able objects
        {
            other.gameObject.SetActive(false); //Hides the pickup able object
            Instantiate(ps, transform.position, Quaternion.identity); //Instantiates the particle system
            score+= 3;
            scoreText.text = score.ToString();
            CombatTextManager.Instance.CreateText(other.transform.position, "+3", new Color32(255, 4, 238, 255), false);
        }
    }

    /// <summary>
    /// When the player exits a trigger
    /// </summary>
    /// <param name="other">The trigger it exits</param>
    void OnTriggerExit(Collider other)
    {
        //if (other.tag == "Tile") //If we exit a tile
        //{   
        //    //Do a raycast, to check if we need to die
        //    RaycastHit hit;

        //    Ray downRay = new Ray(transform.position, -Vector3.up);

        //    if (!Physics.Raycast(downRay, out hit)) //We didnt hit anything, and we need to die
        //    {
                
        //        isDead = true; //Kills the player

        //        GameOver();

        //        resetBtn.SetActive(true); //Shows the reset button

        //        if (transform.childCount > 0) //Stops the camera from following the player
        //        {
        //            transform.GetChild(0).transform.parent = null;
        //        }
                
        //    }
        //}
   }

    /// <summary>
    /// Handels the GameOver menu
    /// </summary>
    private void GameOver()
    {
        //Triggers the menu
        gameOverAnim.SetTrigger("GameOver");

        //Writes the current score
        scoreTexts[1].text = score.ToString();

        //Loads the best score
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        //If the current score is better than the last highscore
        if (score > bestScore)
        {
            //Set the current score as the new highscore
            PlayerPrefs.SetInt("BestScore", score);

            //Enables the "NEW HIGHSCORE!" text
            newHighScore.gameObject.SetActive(true);

            //Changes the background to pink
            background.color = new Color32(255, 118, 246, 255);

            //Runs through all the text and changes the color to white
            foreach (Text txt in scoreTexts)
            {
                txt.color = Color.white;
            }

        }

        //Writes the highscore
        scoreTexts[3].text = PlayerPrefs.GetInt("BestScore",0).ToString();
        
    }

    private bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(contactPoint.position,.5f, whatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }


        return false;
    }
}
