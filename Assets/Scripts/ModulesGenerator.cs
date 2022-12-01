using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulesGenerator : MonoBehaviour
{
    public static ModulesGenerator instance;

    public ModuleData[] unlockedModules;

    [System.Serializable]
    public struct ModuleAmount
    {
        public ModuleData data;
        public int amount;
    }

    // A fixed set of modules (set in editor) that will be returned when generateFixedSet() is called
    [SerializeField] ModuleAmount[] fixedGenModuleSet;

    [SerializeField] bool generateDeckOnStart; // if true, will generate a deck of modules to be added to deck
    [SerializeField] bool generateRandomDeckOnStart; // used only if generateDeckOnStart is true, will generate random modules from unlocked modules
    [SerializeField] int generateRandomModuleAmountOnStart; // used only if generateRandomDeckOnStart is true, specifies # of modules to be generated
    [SerializeField] ModuleAmount[] fixedOnStartSet; // used only if generateRandomDeckOnStart is false, specifies the fixed deck to be generated

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (instance != null) {
            Debug.Log("An instance of ModulesGenerator already exists");
            Destroy(this.gameObject);
        }

        instance = this;
        yield return new WaitForSecondsRealtime(1f);

        if (generateDeckOnStart) {
            if (generateRandomDeckOnStart) {
                DeckManager.instance.ExtendDeck(generateRandomModules(generateRandomModuleAmountOnStart));
            } else {
                foreach(ModuleAmount m in fixedOnStartSet) {
                    for (int i = 0; i < m.amount; i++) {
                        DeckManager.instance.AddModule(m.data);
                    }
                }
            }
        }
    }

    public List<Module> generateRandomModules(int num) {
        List<Module> ret = new List<Module>();

        for (int i = 0; i < num; i++) {
            ModuleData generatedModData = unlockedModules[Random.Range(0, unlockedModules.Length)];
            ret.Add(new Module(generatedModData));
        }

        return ret;
    }

    public List<Module> generateFixedSet() {
        List<Module> ret = new List<Module>();

        foreach(ModuleAmount m in fixedGenModuleSet) {
            for (int i = 0; i < m.amount; i++) {
                ret.Add(new Module(m.data));
            }
        }

        return ret;
    }
}
