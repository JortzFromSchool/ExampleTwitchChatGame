using UnityEngine;
using System.Collections;

public class ParticleScript : MonoBehaviour {

    /// <summary>
    /// The particle system
    /// </summary>
    private ParticleSystem ps;

	// Use this for initialization
	void Start () {

        //Creates a reference to the system
        ps = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () 
    {   
        //Removes the particle system from the Game, when its done playing
        if (!ps.isPlaying)
        {
            Destroy(gameObject);
        }
	}
}
