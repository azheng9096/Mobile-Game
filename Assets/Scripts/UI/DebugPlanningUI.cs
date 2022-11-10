using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlanningUI : MonoBehaviour
{
    public ModuleData pencil;
    public ModuleData potion;


    public void AddPencil() {
        DeckManager.instance.AddModule(pencil);
        Debug.Log("AddedPencil");
    }

    public void AddPotion() {
        DeckManager.instance.AddModule(potion);
        Debug.Log("AddedPotion");
    }
}
