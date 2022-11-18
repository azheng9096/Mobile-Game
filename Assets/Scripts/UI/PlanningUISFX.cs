using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningUISFX : MonoBehaviour
{
    public AudioSource aud;
    public AudioClip enter;
    public AudioClip exit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterSFX()
    {
        aud.PlayOneShot(enter);
    }
    public void ExitSFX()
    {
        aud.PlayOneShot(exit);
    }
}
