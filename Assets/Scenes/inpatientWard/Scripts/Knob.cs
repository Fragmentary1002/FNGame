using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knod : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject videoPlayer;
    public VideoPlayerScript playerScript;
    void Start()
    {
        playerScript = videoPlayer.GetComponent<VideoPlayerScript>();
    }

    void OnMouseDown()
    {
        playerScript.KnobOnPressDown();
    }

    void OnMouseUp()
    {
        playerScript.KnobOnRelease();
    }

    void OnMouseDrag()
    {
        playerScript.KnobOnDrag();  
    }
}
