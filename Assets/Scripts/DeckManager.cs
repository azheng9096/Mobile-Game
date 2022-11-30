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

    void Start() {
    }

    public IEnumerator checkForGeneration()
    {
        yield return new WaitForSeconds(1f);
        int countAvailable = 0;
        foreach(Module m in deck)
        {
            if(m.planningAvailable == true)
            {
                countAvailable++;
            }
        } 
        if(countAvailable == 0)
        {
            // POSSIBLE SHOW LOSE SCREEN
            
            // regeneration
            deck.Clear();
            DeckChangedCallback.Invoke();
            DeckGenerator.instance.GenerateDeck();
        }
    }

    public void AddModule(ModuleData moduleData) {
        deck.Add(new Module(moduleData));

        SortByDefault();
        DeckChangedCallback.Invoke();
    }

    public void RemoveModule(Module module) {
        deck.Remove(module);

        SortByDefault();
        DeckChangedCallback.Invoke();
    }

    public void ToggleModuleAvailability(Module module, bool val) {
        if (deck.IndexOf(module) == -1) {
            Debug.Log("Cannot toggle availability of a module not in deck");
            return;
        }

        module.planningAvailable = val;

        SortByDefault();
        DeckChangedCallback.Invoke();
    }


    public void SetDeck(List<Module> deck) {
        this.deck = deck;

        SortByDefault();
        DeckChangedCallback.Invoke();
    }


    public void ExtendDeck(List<Module> deck) {
        foreach (Module module in deck) {
            this.deck.Add(module);
        }

        SortByDefault();
        DeckChangedCallback.Invoke();
    }


    public void SortByDefault() {
        print("Sorting");
        
        deck.Sort((x, y) => {
            int ret = y.planningAvailable.CompareTo(x.planningAvailable);
            return ret != 0 ? ret : x.moduleData.modName.CompareTo(y.moduleData.modName);
        });
    }

    public void SortByName() {
        deck.Sort((x, y) => x.moduleData.modName.CompareTo(y.moduleData.modName));
    }

    public void SortByAvailability() {
        deck.Sort((x, y) => y.planningAvailable.CompareTo(x.planningAvailable));
    }


    void Update() {
    }
}