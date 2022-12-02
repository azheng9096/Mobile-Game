using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndlessHandler : MonoBehaviour
{

    /*
        Endless mode should keep track of waves, player health, and modules in deck

        waves should have different enemies.
        wave 1 should be the first enemy type, then it gets randomized. 
        backgrounds should also get randomized for each wave.

        player
    */
    public int wave = 1;
    public int maxWaveCount = 1;

    public int modsPerWave = 3;
    public int waveGroupCount = 3;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI maxWaveText;
    public TextMeshProUGUI deathText;

    [SerializeField] public List<ModuleData> currentlyUnlocked;
    //the amount in this case will be the wave necessary to add it to currently unlocked
    [SerializeField] ModulesGenerator.ModuleAmount[] unlockOrder;

    [SerializeField] Slider planningHealthBar;

    public CombatManager cm;
    public GameObject[] enemyPrefabs;
    public GameObject currentEnemy;

    public SpriteRenderer background;
    public Color[] backgroundColors;

    // Start is called before the first frame update
    void Start()
    {
        LoadScore();
        initializeWave();
        int amountOfModulesToGive = modsPerWave*((wave-1) / waveGroupCount) + modsPerWave;
        int bgColorIndex = ((wave-1) / waveGroupCount) % backgroundColors.Length;
        background.color = backgroundColors[bgColorIndex];
        VictoryUI v = FindObjectOfType<VictoryUI>();
        v.minRandSalvagedModules = amountOfModulesToGive;
        v.maxRandSalvagedModules = amountOfModulesToGive;
        for(int i = 0; i < unlockOrder.Length; i++)
        {
            if(unlockOrder[i].amount <= wave)
            {
                currentlyUnlocked.Add(unlockOrder[i].data);
            } else
            {
                break;
            }
        }
    }
    public void incrementScore()
    {
        wave++;
        PlayerPrefs.SetInt("wave", wave);
        if (wave > maxWaveCount)
        {
            maxWaveCount = wave;
            PlayerPrefs.SetInt("maxWaveCount", maxWaveCount);
        }
        PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        wave = 1;
        PlayerPrefs.SetInt("wave", wave);
        PlayerPrefs.Save();
        PlayerSavedData.ResetSavedData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScore()
    {
        if (!PlayerPrefs.HasKey("maxWaveCount"))
        {
            PlayerPrefs.SetInt("wave", wave);
            PlayerPrefs.SetInt("maxWaveCount", maxWaveCount);
        }
        wave = PlayerPrefs.GetInt("wave");
        maxWaveCount = PlayerPrefs.GetInt("maxWaveCount");
    }

    void initializeWave(){
        waveText.text = "Floor: " + wave;
        deathText.text = "More floors\nawait you. Don't\nstop at floor <b>" + wave;
        maxWaveText.text = "Highest Floor Reached: " + maxWaveCount;
        int enemyToSpawn = 0;
        //first few waves are standard, then gets randomized
        switch (wave)
        {
            case 1:
                StartCoroutine(ModulesGenerator.instance.generateWholeDeck());
                break;
            case 2:
                enemyToSpawn = 1;
                break;
            case 3:
                enemyToSpawn = 2;
                break;
            default:
                enemyToSpawn = Random.Range(0, enemyPrefabs.Length);
                break;
        }
        //then start combat
        EntityController newEnemy = Instantiate(enemyPrefabs[enemyToSpawn]).transform.GetChild(0).GetComponent<EntityController>();
        newEnemy.planningHealthBar = planningHealthBar;
        newEnemy.combatManager = cm;
        cm.enemy = newEnemy;

        cm.AlternateStart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetLevel()
    {
        wave = 1;
        PlayerPrefs.SetInt("wave", wave);
        PlayerPrefs.Save();
        PlayerSavedData.ResetSavedData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetAndMenu()
    {
        wave = 1;
        PlayerPrefs.SetInt("wave", wave);
        PlayerPrefs.Save();
        PlayerSavedData.ResetSavedData();
        SceneManager.LoadScene("Start");
    }
}
