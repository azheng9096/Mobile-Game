using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    private EntityController thisController = null;
    private EntityController curTarget = null;
    private Module curMod;
    public Animator anim;

    public AudioClip laserShootSound;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(EntityController controller, EntityController target, Module mod)
    {
        thisController = controller;
        curTarget = target;
        curMod = mod;
        switch (mod.moduleData.mode)
        {
            case 0:
                anim.SetTrigger("A");
                PlaySound(laserShootSound);
                break;
            case 1:
                anim.SetTrigger("B");
                break;
            case 2:
                anim.SetTrigger("C");
                break;
            default:
                break;
        }
    }

    public void dealDamage()
    {
        EntityController target = curTarget;
        Module mod = curMod;

        float r = Random.Range(0f, 100f);
        if (r <= mod.moduleData.accuracy && thisController == thisController.combatManager.player)
        {
            target.TakeDamage(mod.moduleData.damage);
            //show target dodge animation
        }
        else if (target == thisController.combatManager.player)
        {
            target.TakeDamage(mod.moduleData.damage);
        }
        else
        {
            target.HandleMiss();
        }
    }

    private void PlaySound(AudioClip clip) {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}
