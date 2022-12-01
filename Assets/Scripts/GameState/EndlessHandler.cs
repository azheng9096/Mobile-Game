using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndlessHandler : MonoBehaviour
{

    /*
        Endless mode should keep track of waves, player health, and modules in deck

        waves should have different enemies.
        wave 1 should be the first enemy type, then it gets randomized. 
        backgrounds should also get randomized for each wave.

        player
    */

    public int oldWave = 0;
    public int wave = 1;
    public int maxWaveCount;
    public TextMeshProUGUI waveText;

    public GameObject[] enemyPrefabs;
    public GameObject[] backgrounds;


    // Start is called before the first frame update
    void Start()
    {

    }

    void initializeWave(){
        if(wave == 1){
            //do stuff for wave 1

        } else {
            //random stuff
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(oldWave < wave){
            oldWave = wave;
            initializeWave();
        }
        if(oldWave > wave){
            oldWave = 1;
            wave = 1;
            initializeWave();
        }
    }
}
