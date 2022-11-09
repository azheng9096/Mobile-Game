using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    public List<Module> deck = new List<Module>();
    // List<Module> selected = new List<Module>();

    void Awake() {
        if (instance != null) {
            Debug.Log("An instance of DeckManager already exists");
            return;
        }

        instance = this;
    }

    public void AddModule(ModuleData moduleData) {
        deck.Add(new Module(moduleData));
    }

    public void RemoveModule(Module module) {
        deck.Remove(module);
    }

    void Update() {
    }
}
