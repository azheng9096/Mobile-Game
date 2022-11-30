using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EntityStatus {
    Idle,
    Active,
    Empty,
    Dead
}
public delegate void Callback();

public class EntityController : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider planningHealthBar;
    public CombatManager combatManager;
    List<Module> modules;
    [SerializeField]
    public List<ModuleData> attackPattern;
    public EntityStatus status;

    public GameObject TextPopUpPrefab;
    public Transform TextPopUpTarget;
    bool dashing = false;
    bool blocking = false;
    bool hitInterrupt = false;
    [SerializeField] float animDelayMult = 1.0f; 
    [SerializeField] Animator animator;

    [SerializeField] LaserController laserController;
    [SerializeField] DroneController droneController;

    public EntityController curTarget = null;
    public Module curMod = null;

    public void Init(CombatManager combatManager, List<Module> modules, float maxHealth = 100f, float health = 100f) {
        if (animator != null) {
            animator.SetBool("Dead", false);
        }
        this.combatManager = combatManager;
        this.modules = modules;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        planningHealthBar.maxValue = maxHealth;
        planningHealthBar.value = health;
        if (modules.Count == 0) {
            combatManager.UpdateStatus(this, EntityStatus.Empty);
        }
    }

    public void nextModules(List<Module> modules)
    {
        this.modules = modules;
        if (modules.Count == 0)
        {
            print("next modules empty");
            combatManager.UpdateStatus(this, EntityStatus.Empty);
        } else
        {
            status = EntityStatus.Idle;
        }
    }
    void Awake()
    {
        status = EntityStatus.Idle;
    }
    
    public void Dash(Callback callback) {
        if (dashing) {
            return;
        }
        animator.SetTrigger("Dodge");
        StartCoroutine(Dash_Routine(callback));
    }
    IEnumerator Dash_Routine(Callback callback) {
        print("Dashing");
        dashing = true;
        yield return new WaitForSeconds(1f);
        dashing = false;
        print("Not dashing");
        yield return new WaitForSeconds(4f);
        callback();
    }
    public void UseModule(EntityController target) {
        if (modules.Count == 0) {
            UpdateEntityStatus();
            return;
        }
        print("Module used");
        print(modules.Count);
        Module mod = modules[modules.Count - 1];
        StartCoroutine(combatManager.cardCooldown(this, mod));
        modules.RemoveAt(modules.Count - 1);
        StartCoroutine(UseModule_Routine(mod, target));
        print(modules.Count);
    }

    public void dealDamage(){
        EntityController target = curTarget;
        Module mod = curMod;

        float r = Random.Range(0f, 100f);
                if (r <= mod.moduleData.accuracy && this == combatManager.player)
                {
                    target.TakeDamage(mod.moduleData.damage);
                    //show target dodge animation
                } else if(target == combatManager.player)
                {
                    target.TakeDamage(mod.moduleData.damage);
                } else {
                    target.HandleMiss();
                }
    }

    IEnumerator UseModule_Routine(Module mod, EntityController target) {
        
        if (mod.moduleData.type == ModuleType.Shoot || mod.moduleData.type == ModuleType.Melee) {
            hitInterrupt = false;
            if (animator != null) {
                animator.SetTrigger(mod.moduleData.GetModuleType());
                if (mod.moduleData.type == ModuleType.Shoot && laserController != null) {
                    laserController.Shoot();
                }
            }
            curTarget = target;
            curMod = mod;
        } else if (mod.moduleData.type == ModuleType.Support) {
            if (mod.moduleData.specialEffect == "block") {
                if (animator != null) {
                    animator.SetBool("Blocking", true);
                }
                blocking = true;
                yield return new WaitForSeconds(1.5f);
                blocking = false;
                if (animator != null) {
                    animator.SetBool("Blocking", false);
                }
            }
        } else if (mod.moduleData.type == ModuleType.Drone)
        {
            //do drone stuff
            droneController.init(this, target, mod);
        }
        if (modules.Count == 0 ) {
            if(this == combatManager.player)
                combatManager.UpdateStatus(this, EntityStatus.Empty);
            status = EntityStatus.Empty;
        }
    }
    public void HandleMiss() {
        print("Miss");
        CreateTextPopUp("Miss", Color.white);
    }

    void CreateTextPopUp(string text, Color color) {
        print("Creating popup " + text);
        GameObject textPopUp = Instantiate(TextPopUpPrefab, TextPopUpTarget.position, Quaternion.identity, transform);
        print(textPopUp);
        TextPopUpController textPopUpController = textPopUp.GetComponent<TextPopUpController>();
        textPopUpController.Init(text, color);
        textPopUpController.Check();
    }

    public void TakeDamage(float damage) {
        if (dashing) {
            print("dodged");
            CreateTextPopUp("Dodged", new Color(255, 130, 140, 255));
        } else if (blocking) {
            StartCoroutine(FindObjectOfType<CameraShaker>().Shake(.1f, .1f));
            print("blocked");
            CreateTextPopUp("Blocked", new Color(255, 130, 140, 255));
            /*
             * blocking = false;
            
            if (animator != null) {
                animator.SetBool("Blocking", false);
            }*/
        } else {
            StartCoroutine(FindObjectOfType<CameraShaker>().Shake(.1f, .3f));
            StartCoroutine(TakeDamage_Routine(damage));
        }
    }

    IEnumerator TakeDamage_Routine(float damage) {
        if (animator != null) {
            animator.SetTrigger("Hurt");
            if (laserController != null) {
                laserController.EndShoot();
            }
        }
        healthBar.value -= damage;
        planningHealthBar.value -= damage;
        if (healthBar.value <= 0) {
            StartCoroutine(Die());
        }
        yield return new WaitForSeconds(0.1f);
    }

    public void StartAutoAttack() { StartCoroutine(AutoAttack()); }
    IEnumerator AutoAttack() {
        status = EntityStatus.Active;
        yield return new WaitForSeconds(1f);
        while (status == EntityStatus.Active) {
            if (GameStateManager.Instance.CurrentGameState == GameState.Combat) {
                yield return new WaitForSeconds(0.5f);
                UseModule(combatManager.GetEnemy(this));
                
            }
            yield return new WaitForSeconds(2f);
        }
        UpdateEntityStatus();
    }
    IEnumerator Die() {
        if (animator != null) {
            animator.SetBool("Dead", true);
        }
        status = EntityStatus.Dead;
        yield return new WaitForSeconds(.3f);
        if (this == combatManager.player)
        {
            FindObjectOfType<DeathUI>().ShowDeath();
        }
        yield return new WaitForSeconds(1f);
        UpdateEntityStatus();
        Destroy(gameObject);
    }

    void UpdateEntityStatus() {
        if (healthBar.value <= 0) {
            status = EntityStatus.Dead;
        } else if (modules.Count == 0) {
            status = EntityStatus.Empty;
        } else {
            status = EntityStatus.Active;
        }
        combatManager.UpdateStatus(this, status);
    }
}
