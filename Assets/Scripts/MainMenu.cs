using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    bool fired = false;
    public GameObject ninja;
    float timer = 0.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (fired == false) return;

        ninja.GetComponent<Animator>().SetFloat("velocityX", Mathf.Abs(3.0f));
        ninja.transform.Translate(3.0f * Time.deltaTime, 0, 0);

        if (ninja.transform.position.x >= 40.0f)
        {
            //done
        }
    }

    public void FireOnClick()
    {
        fired = true;
    }
}
