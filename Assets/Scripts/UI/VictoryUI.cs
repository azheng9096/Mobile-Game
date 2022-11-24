using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] GameObject victoryUI;

    [SerializeField] Transform ModuleSlots;
    [SerializeField] GameObject ModuleSlotPrefab;

    [SerializeField] PlanningUIInfoDisplay InfoDisplay;

    List<Module> salvageModules = new List<Module>();
    public delegate void OnSalvageModulesChanged();
    public OnSalvageModulesChanged SalvageModulesChangedCallback;

    void Awake() {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDestroy() {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        DeckManager.instance.DeckChangedCallback -= ListModules;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize Listener
        SalvageModulesChangedCallback += ListModules;
    }

    public void AddModule(Module module) {
        salvageModules.Add(module);

        SalvageModulesChangedCallback.Invoke();
    }

    public void RemoveModule(Module module) {
        salvageModules.Remove(module);

        SalvageModulesChangedCallback.Invoke();
    }

    public void SetModules(List<Module> salvageModules) {
        this.salvageModules = salvageModules;

        SalvageModulesChangedCallback.Invoke();
    }

    public void ClearModules() {
        salvageModules.Clear();
    }

    public void ListModules() {
        foreach (Transform child in ModuleSlots) {
            Destroy(child.gameObject);
        }

        foreach (Module module in salvageModules) {
            GameObject obj = Instantiate(ModuleSlotPrefab, ModuleSlots);
            
            PlanningUIModuleSlot slot = obj.GetComponent<PlanningUIModuleSlot>();
            slot.Set(module);

            slot.OnModuleClicked += HandleModuleClick;
        }
    }

    // --- MODULE SLOT ---
    // Handle On Click
    void HandleModuleClick(PlanningUIModuleSlot moduleSlot) {
        InfoDisplay.DisplayModule(moduleSlot.module);
    }

    // --- NEXT STAGE BUTTON ---
    // Handle On Click
    public void OnButtonClick() {
        print("going to next stage");
    }


    // --- SWITCHING TO VICTORY PHASE ---
    void OnGameStateChanged(GameState newState) {
        if (newState == GameState.Victory) {
            print("now displaying victory ui");
            victoryUI.SetActive(true);
            ListModules();
        }
    }
}
