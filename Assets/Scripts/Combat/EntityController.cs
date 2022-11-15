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
    CombatManager combatManager;
    List<Module> modules;
    [SerializeField]
    public List<ModuleData> attackPattern;
    public EntityStatus status;
    bool dashing = false;
    public void Init(CombatManager combatManager, List<Module> modules, float maxHealth = 100f, float health = 100f) {
        this.combatManager = combatManager;
        this.modules = modules;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        healthBar.gameObject.SetActive(true);
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
        healthBar.gameObject.SetActive(false);
    }
    
    public void Dash(Callback callback) {
        if (dashing) {
            return;
        }
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
        print("Module used");
        print(modules.Count);
        Module mod = modules[modules.Count - 1];
        StartCoroutine(combatManager.cardCooldown(this, mod));
        modules.RemoveAt(modules.Count - 1);
        if (mod.moduleData.type == ModuleType.Attack) {
            float r = Random.Range(0f, 100f);
            if (r <= mod.moduleData.accuracy && this == combatManager.player)
            {
                target.TakeDamage(mod.moduleData.damage);
                //show target dodge animation
            } else if(target == combatManager.player)
            {
                target.TakeDamage(mod.moduleData.damage);
            }
        }
        print(modules.Count);
        if (modules.Count == 0 ) {
            if(this == combatManager.player)
                combatManager.UpdateStatus(this, EntityStatus.Empty);
            status = EntityStatus.Empty;
        }
        
    }

    void TakeDamage(float damage) {
        if (dashing) {
            print("dodged");
        } else {
            healthBar.value -= damage;
            if (healthBar.value <= 0) {
                StartCoroutine(Die());
            }
        }
    }

    public void StartAutoAttack() { StartCoroutine(AutoAttack()); }
    IEnumerator AutoAttack() {
        status = EntityStatus.Active;
        yield return new WaitForSeconds(1f);
        while (status == EntityStatus.Active) {
            if (GameStateManager.Instance.CurrentGameState == GameState.Combat) {
                print("Attacking now");
                yield return new WaitForSeconds(0.5f);
                UseModule(combatManager.GetEnemy(this));
            }
            yield return new WaitForSeconds(2f);
        }
        UpdateEntityStatus();
    }
    IEnumerator Die() {
        print("Oh no I'm dying");
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
