using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class AnimateBackground : MonoBehaviour {

    public float Speed = 1.0f;
    private ScreenOverlay overlay;
    private float counter = 0.0f;
    private int currIndex = 0;

    public Texture2D[] images;

    // Use this for initialization
    void Start () {
        overlay = this.GetComponent<ScreenOverlay>();
	}
	
	// Update is called once per frame
	void Update () {

        counter += Speed * Time.deltaTime;
		
        if(counter >= 1.0f)
        {
            counter = 0.0f;
            currIndex++;
            if (currIndex >= images.Length) currIndex = 0;
            overlay.texture = images[currIndex];
        }
	}
}
