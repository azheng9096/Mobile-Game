using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVictoryUI : MonoBehaviour
{
    [SerializeField] VictoryUI victoryUI;
    public ModuleData pencil;
    public ModuleData potion;
    public ModuleData shield;


    public void AddPencil() {
        victoryUI.AddModule(new Module(pencil));
        Debug.Log("AddedPencil");
    }

    public void AddPotion() {
        victoryUI.AddModule(new Module(potion));
        Debug.Log("AddedPotion");
    }

    public void AddShield() {
        victoryUI.AddModule(new Module(shield));
        Debug.Log("AddedShield");
    }
}
