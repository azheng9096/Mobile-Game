using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealController : MonoBehaviour
{
    private EntityController thisController = null;
    private Module curMod;
    public ParticleSystem[] healParticles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(EntityController controller, Module mod)
    {
        thisController = controller;
        curMod = mod;
        switch (mod.moduleData.mode)
        {
            //heal for damage value
            case 0: // basic heal
                healParticles[0].Stop();
                healParticles[0].Play();
                thisController.HealLifePoints(mod.moduleData.damage);
                break;
            case 1: //aggressive heal
                healParticles[1].Stop();
                healParticles[1].Play();
                thisController.HealLifePoints(mod.moduleData.damage);
                break;
            case 2:
                break;
            default:
                break;
        }
    }



}
