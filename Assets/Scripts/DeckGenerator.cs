using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckGenerator : MonoBehaviour
{
    //------------------------------------------------------------------------
    //TODO: generate procedurally based on an unlock list of modules
    //------------------------------------------------------------------------

    [System.Serializable]
    public struct ModuleAmount
    {
        public ModuleData data;
        public int amount;
    }
    
    public ModuleAmount[] moduleAmounts;
    public static DeckGenerator instance;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (instance != null)
        {
            Debug.Log("An instance of DeckGenerator already exists");
            Destroy(this.gameObject);
        }
        instance = this;
        yield return new WaitForSecondsRealtime(1f);
        GenerateDeck();
    }

    public void GenerateDeck()
    {
        foreach (ModuleAmount m in moduleAmounts)
        {
            for (int i = 0; i < m.amount; i++)
            {
                print(i);
                DeckManager.instance.AddModule(m.data);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
