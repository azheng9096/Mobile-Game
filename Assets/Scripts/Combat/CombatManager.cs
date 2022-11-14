using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CombatManager : MonoBehaviour
{
    [SerializeField] GameObject modIconPrefab;

    [SerializeField] Transform modIconParent;
    [SerializeField] List<GameObject> moduleIcons;

    public List<Module> modules;

    [SerializeField] GameObject HUDGroup;
    Button dashButton;
    Button useModuleButton;

    [SerializeField] EntityController player;
    [SerializeField] EntityController enemy;

    [SerializeField]int workingEntities = 0;
    void Awake() {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    void Destroy() {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    
    void Start() {
        dashButton = HUDGroup.transform.Find("DashButton").GetComponent<Button>();
        useModuleButton = HUDGroup.transform.Find("UseModuleButton").GetComponent<Button>();
        moduleIcons = new List<GameObject>();
        HUDGroup.SetActive(GameStateManager.Instance.CurrentGameState == GameState.Combat);


        print(Resources.Load<ModuleData>("Stab"));
        Module mod = new Module(Resources.Load<ModuleData>("Stab"));
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
    }

    IEnumerator DelayCombatStart() {
        yield return new WaitForSeconds(3f);
        print("Starting combat");
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
        if (GameStateManager.Instance.CurrentGameState == GameState.Combat) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                GameStateManager.Instance.SetState(GameState.Paused);
            }
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            EndCombat();
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

            // set module prefab
            modIconPrefab.GetComponent<Image>().sprite = module.moduleData.sprite;

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
                print("You win, they're dead");
            }
        }
        if (workingEntities == 0) {
            EndCombat();
        }
    }

    void EndCombat() {
        print("End combat");
        dashButton.interactable = false;
        useModuleButton.interactable = false;
        GameStateManager.Instance.SetState(GameState.Planning);
    }

    public void StartCombat() {
        Debug.Log("Start combat");
        dashButton.interactable = true;
        useModuleButton.interactable = true;
        GameStateManager.Instance.SetState(GameState.Combat);

        // have to list modules here as well as in OnGameStateChanged -- or else icon for last module does not load for some reason
        ListModules();
    }
}
