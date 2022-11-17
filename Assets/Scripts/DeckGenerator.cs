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
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1f);
        foreach(ModuleAmount m in moduleAmounts)
        {
            for(int i = 0; i < m.amount; i++)
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
