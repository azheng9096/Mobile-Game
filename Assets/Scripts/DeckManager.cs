using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager instance;

    public List<Module> deck = new List<Module>();

    public delegate void OnDeckChanged();
    public OnDeckChanged DeckChangedCallback;

    void Awake() {
        if (instance != null) {
            Debug.Log("An instance of DeckManager already exists");
            return;
        }

        instance = this;
    }

    public void AddModule(ModuleData moduleData) {
        deck.Add(new Module(moduleData));

        DeckChangedCallback.Invoke();
    }

    public void RemoveModule(Module module) {
        deck.Remove(module);

        DeckChangedCallback.Invoke();
    }

    public void ToggleModuleAvailability(Module module, bool val) {
        if (deck.IndexOf(module) == -1) {
            Debug.Log("Cannot toggle availability of a module not in deck");
            return;
        }

        module.planningAvailable = val;
        DeckChangedCallback.Invoke();
    }

    void Update() {
    }
}