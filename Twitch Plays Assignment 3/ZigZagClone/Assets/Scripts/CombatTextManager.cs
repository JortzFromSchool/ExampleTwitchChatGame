using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CombatTextManager : MonoBehaviour {

    /// <summary>
    /// The singleton instance
    /// </summary>
    private static CombatTextManager instance;

    /// <summary>
    /// Sets the speed of the text
    /// </summary>
    public float speed;

    /// <summary>
    /// Sets the direction of the text
    /// </summary>
    public Vector3 direction;

    /// <summary>
    /// Prefab used to instantiate the text
    /// </summary>
    public GameObject textPrefab;

    /// <summary>
    /// A reference to the canvasTransform
    /// </summary>
    public RectTransform canvasTransform;

    /// <summary>
    /// Sets the fade time of the text
    /// </summary>
    public float fadeTime;

    /// <summary>
    /// The camera's transform
    /// </summary>
    public Transform camTransform;

    /// <summary>
    /// Property for accessing the singleton instance
    /// </summary>
    public static CombatTextManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CombatTextManager>();
            }
            return CombatTextManager.instance; 
        }
    }


    /// <summary>
    /// Creates a scrolling combat text
    /// </summary>
    /// <param name="position">The text's spawn position</param>
    /// <param name="text">The text's content</param>
    /// <param name="color">The color of the text</param>
    /// <param name="critical">Indicates if this is a critical strike</param>
    public void CreateText(Vector3 position, string text, Color color, bool critical)
    {
        //Instantiates the scrolling combat text
        GameObject sct = (GameObject)Instantiate(textPrefab, position, Quaternion.identity);

        //Sets the canvas as the parent
        sct.transform.SetParent(canvasTransform);

        //Sets the correct scale
        sct.GetComponent<RectTransform>().localScale = new Vector3(.1f, .1f, .1f);

        //Initializes the text
        sct.GetComponent<CombatText>().Initialize(speed, direction,fadeTime, critical);

        //Sets the text
        sct.GetComponent<Text>().text = text;

        //Sets the color
        sct.GetComponent<Text>().color = color;
    }
}
