using UnityEngine;

public class MainMenu : MonoBehaviour
{
    /** The ninja prefab */
    public GameObject ninja;

    /** has the game been fired */
    private bool fired = false;
    /** a timer */
    private float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (fired == false) return;

        ninja.GetComponent<Animator>().SetFloat("velocityX", Mathf.Abs(3.0f));
        ninja.transform.Translate(3.0f * Time.deltaTime, 0, 0);

        if (ninja.transform.position.x >= 40.0f)
        {
            //done
            GameObject.Find("Canvas").GetComponent<UIManager>().OnLevelSelector();
        }
    }

    public void FireOnClick()
    {
        fired = true;
    }
}
