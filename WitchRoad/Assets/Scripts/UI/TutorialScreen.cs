using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    public GameObject tutorialScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Close()
    {
        if (tutorialScreen != null)
        {
            bool isActive = tutorialScreen.activeSelf;
            tutorialScreen.SetActive(!isActive);
        }
    }
}
