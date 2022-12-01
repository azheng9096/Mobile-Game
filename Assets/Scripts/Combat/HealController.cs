using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealController : MonoBehaviour
{
    private EntityController thisController = null;
    private Module curMod;
    public ParticleSystem[] healParticles;
    public Transform correctSpotForParticles;
    public GameObject armorPrefab;
    private List<GameObject> armaments;
    public bool isArmored = false;
    public float damageBlocking = 0f; 
    public float armoredTimeLeft = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isArmored){
            armoredTimeLeft -= Time.deltaTime;
            if(armoredTimeLeft<=0){
                isArmored = false;
                damageBlocking = 0;
                //delete all objects in the armaments list and clear the list
            }
        }
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
            case 2: //armament
                //spawn a new armor prefab and add it to the list
                //put the armor particles in the right spot
                isArmored = true;
                if(armoredTimeLeft<=0){
                    armoredTimeLeft = float.Parse(mod.ModuleData.animName);
                } else { //allow the armaments to stack
                    armoredTimeLeft += float.Parse(mod.ModuleData.animName);
                }
                damageBlocking += mod.moduleData.damage;
                break;
            default:
                break;
        }
    }



}
