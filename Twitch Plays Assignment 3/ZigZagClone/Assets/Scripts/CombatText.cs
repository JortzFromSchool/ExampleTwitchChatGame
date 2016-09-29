using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatText : MonoBehaviour {

    /// <summary>
    /// The Combat text's movement speed
    /// </summary>
    private float speed;

    /// <summary>
    /// The directoin of the text
    /// </summary>
    private Vector3 direction = Vector3.zero;

    /// <summary>
    /// The time it will take for the text to fade out
    /// </summary>
    private float fadeTime;

    /// <summary>
    /// The crit animation clip
    /// </summary>
    public AnimationClip animation;

    /// <summary>
    /// Indicates if the text should move or not
    /// </summary>
    private bool stay = true;
   
    // Update is called once per frame
    void Update()
    {
        if (!stay) //If we are allowed to move
        {
            //Calculate the frame independent translation
            float translation = speed * Time.deltaTime;

            //Move the text in the desired direction
            transform.Translate(direction * translation);
        }
    }

    public void Start()
    {
        transform.LookAt(2 * transform.position - CombatTextManager.Instance.camTransform.position);
    }

    /// <summary>
    /// Initializes the combat text with the needed values
    /// </summary>
    /// <param name="speed">The movement speed</param>
    /// <param name="direction">The movement direction</param>
    /// <param name="fadeTime">The time it takes to fade out</param>
    /// <param name="critical">Indicates if this is a critical strike</param>
    public void Initialize(float speed, Vector3 direction, float fadeTime, bool critical)
    {
        //Sets the values
        this.speed = speed;
        this.direction = direction;
        this.fadeTime = fadeTime;
        stay = critical;

        if (critical) //If this is a critical strike
        {
            GetComponent<Animator>().SetTrigger("Critical"); //Play the crit animation
            StartCoroutine(Critical()); //Makes sure that the critical strike doesn't move until the animation is done
        }
        else //If it isn't a critical strike
        {
            StartCoroutine(FadeOut()); //Makes the text fade out
        }
    }

    /// <summary>
    /// Waits for the critical strike to finish animating
    /// </summary>
    private IEnumerator Critical()
    {   
        //Wait untill the animation is done
        yield return new WaitForSeconds(animation.length);

        //Makes the text move
        stay = false;

        //Fades out  the text
        StartCoroutine(FadeOut());
    }
    

    private IEnumerator FadeOut()
    {
        //Sets the values for fading
        float startAlpha = GetComponent<Text>().color.a;

        float rate = 1.0f / fadeTime; //Calculates the rate, so that we can fade over x amount of seconds

        float progress = 0.0f; //Progresses over the set time


        while (progress < 1.0) //Progresses over the set time
        {
            Color tmpColor = GetComponent<Text>().color;

            GetComponent<Text>().color = new Color(tmpColor.r,tmpColor.g,tmpColor.b,Mathf.Lerp(startAlpha, 0, progress));  //Lerps from the start alpha to 0 to make the inventory invisible

            progress += rate * Time.deltaTime; //Adds to the progress so that we will get close to out goal

            yield return null;
        }

        //Sets the end condition to make sure we are 100% invisible
        Destroy(gameObject);

    }
}
