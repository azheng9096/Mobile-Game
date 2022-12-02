using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    [SerializeField] GameObject modIconPrefab;

    [SerializeField] Transform modIconParent;
    [SerializeField] List<GameObject> moduleIcons;

    public List<Module> modules;

    [SerializeField] GameObject HUDGroup;
    Button dashButton;
    Button useModuleButton;

    [SerializeField] public EntityController player;
    [SerializeField] public EntityController enemy;
    [SerializeField] float enemyHealth = 50f;

    [SerializeField]int workingEntities = 2;
    bool useOnCooldown = false;
    void Awake() {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    void OnDestroy() {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    
    void Start() {
        if (SceneManager.GetActiveScene().name != "Endless_Scene")
        {
            dashButton = HUDGroup.transform.Find("DashButton").GetComponent<Button>();
            useModuleButton = HUDGroup.transform.Find("UseModuleButton").GetComponent<Button>();
            moduleIcons = new List<GameObject>();
            HUDGroup.SetActive(GameStateManager.Instance.CurrentGameState == GameState.Combat);

            startBattle();

            StartCoroutine(StartCutscene());
        }
        /*
        modules = new List<Module>();
        modules.Add(mod);
        modules.Add(mod);
        modules.Add(mod);
        modules.Add(mod);
        modules.Add(mod);

        List<Module> modEnemy = new List<Module>();
        modEnemy.Add(mod);
        modEnemy.Add(mod);
        modEnemy.Add(mod);
        modEnemy.Add(mod);
        modEnemy.Add(mod);
        */

        /*
        
        print(player);
        if (player != null) {
            player.Init(this, modules, 100f, 100f);
            workingEntities += 1;
        }
        if (enemy != null) {
            enemy.Init(this, modEnemy, 100f, 50f);
            workingEntities += 1;
        }
        if (GameStateManager.Instance.PreviousGameState == GameState.Combat) {
            ListModules();
            StartCoroutine(DelayCombatStart());   
        }
        */

    }

    public void AlternateStart()
    {
        dashButton = HUDGroup.transform.Find("DashButton").GetComponent<Button>();
        useModuleButton = HUDGroup.transform.Find("UseModuleButton").GetComponent<Button>();
        moduleIcons = new List<GameObject>();
        HUDGroup.SetActive(GameStateManager.Instance.CurrentGameState == GameState.Combat);

        startBattle();
        StartCoroutine(StartCutscene());
    }
    
    //function for initialization tbh
    public void startBattle()
    {
        List<Module> modEnemy = new List<Module>();
        foreach(ModuleData m in enemy.attackPattern)
        {
            modEnemy.Add(new Module(m));
        }
        modules = new List<Module>();
        print(player);
        if (player != null)
        {
            workingEntities += 1;
            //player.Init(this, modules, 50f, 50f);
            player.Init(this, modules, PlayerSavedData.maxHP, PlayerSavedData.HP);
            print("DEBUGDEBUGDEBUG player here");
        }
        if (enemy != null)
        {
            workingEntities += 1;
            enemy.Init(this, modEnemy, enemyHealth, enemyHealth);
        }
    }

    public void nextBattlePhase()
    {
        useOnCooldown = false;
        workingEntities = 0;
        if (player != null)
        {
            workingEntities += 1;
            player.nextModules(modules);
        }
        if (enemy != null)
        {
            workingEntities += 1;
            List<Module> modEnemy = new List<Module>();
            foreach (ModuleData m in enemy.attackPattern)
            {
                modEnemy.Add(new Module(m));
            }
            enemy.nextModules(modEnemy);
            StartCoroutine(DelayCombatStart(1f));
        }
    }

    IEnumerator DelayCombatStart(float time) {
        yield return new WaitForSeconds(time);
        print("Starting aa");
        enemy.StartAutoAttack();
    } 

    void Update() {
        // if (Input.touchCount > 0) {
        //     Touch touch = Input.GetTouch(0);
        //     if (touch.phase == TouchPhase.Began) {
        //         if (combatModules.Count > 0) {
        //             combatModules[combatModules.Count - 1].UseModule();
        //             combatModules.RemoveAt(combatModules.Count - 1);
        //         }
        //     }
        // }

        /*
        if (GameStateManager.Instance.CurrentGameState == GameState.Combat) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                GameStateManager.Instance.SetState(GameState.Paused);
            }
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            EndCombat();
        }
        */
        if(player.status == EntityStatus.Empty || GameStateManager.Instance.CurrentGameState != GameState.Combat || useOnCooldown == true)
        {
            //print("status: " + player.status + " state: " + GameStateManager.Instance.CurrentGameState + " cooldown?: " +useOnCooldown);
            useModuleButton.interactable = false;
        } else
        {
            useModuleButton.interactable = true;
        }
    }

    void OnGameStateChanged(GameState newState) {
        if (newState == GameState.Combat && GameStateManager.Instance.PreviousGameState == GameState.Planning) {
            HUDGroup.SetActive(true);
            ListModules();
        }
    }

    void ListModules() {
        foreach (GameObject icon in moduleIcons) {
            Destroy(icon);
        }
        
        foreach (Module module in modules) {
            GameObject obj = Instantiate(modIconPrefab, modIconParent);
            obj.GetComponent<Image>().sprite = module.moduleData.sprite;
            moduleIcons.Add(obj);
        }
        
    }
    public void DashCallback() {
        dashButton.interactable = true;
    }
    public void Dash() {
        dashButton.interactable = false;
        player.Dash(DashCallback);
    }
    public void UseCard() {
        player.UseModule(enemy);
        Destroy(moduleIcons[moduleIcons.Count - 1]);
        moduleIcons.RemoveAt(moduleIcons.Count - 1);
    }

    public IEnumerator cardCooldown(EntityController controller, Module mod)
    {
        if(controller == player)
        {

            useOnCooldown = true;
            yield return new WaitForSeconds(mod.moduleData.cooldown);
            useOnCooldown = false;
        } else
        {
            yield return null;
        }
        
    }

    public EntityController GetEnemy(EntityController enemy) {
        if (this.enemy == enemy) {
            return player;
        } else {
            return this.enemy;
        }
    }
    public void UpdateStatus(EntityController controller, EntityStatus status) {
        if (controller == player) {
            workingEntities -= 1;
            if (status == EntityStatus.Dead) {
                print("You died");
            }
            useModuleButton.interactable = false;
        } else if (controller == enemy) {
            workingEntities -= 1;
            if (status == EntityStatus.Dead) {
                enemy.status = EntityStatus.Dead;
                StartCoroutine(EndCombat());
            }
        }
        if (workingEntities == 0) {
            StartCoroutine(EndCombat());
        }
    }

    public IEnumerator StartCutscene()
    {
        GameStateManager.Instance.SetState(GameState.Cutscene);
        yield return new WaitForSeconds(.1f);
        GameStateManager.Instance.SetState(GameState.Planning);
    }
    public IEnumerator EndCutscene()
    {
        GameStateManager.Instance.SetState(GameState.Cutscene);
        yield return new WaitForSeconds(2f);

        if(player.status!=EntityStatus.Dead)
            GameStateManager.Instance.SetState(GameState.Victory);
    }

    public IEnumerator EndCombat() {
        yield return new WaitForSeconds(.2f);
        if (enemy != null && player != null && enemy.status != EntityStatus.Dead && player.status != EntityStatus.Dead)
        {
            print("End combat");
            //hide the two buttons
            dashButton.interactable = false;
            useModuleButton.interactable = false;
            GameStateManager.Instance.SetState(GameState.Planning);
            print("SPECIALENDCOMBAT");
        } else
        {
            print("Endscene!");
            //hide the two buttons
            dashButton.interactable = false;
            useModuleButton.interactable = false;
            StartCoroutine(EndCutscene());
        }
        
    }

    public void StartCombat() {
        Debug.Log("Start combat");
        //show the two buttons
        dashButton.interactable = true;
        useModuleButton.interactable = true;
        GameStateManager.Instance.SetState(GameState.Combat);
        nextBattlePhase();
        // have to list modules here as well as in OnGameStateChanged -- or else icon for last module does not load for some reason
        ListModules();
    }
}
