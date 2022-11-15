using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningUI : MonoBehaviour
{
    [SerializeField] GameObject planningUI;

    [SerializeField] Transform ModuleSlots;
    [SerializeField] GameObject ModuleSlotPrefab;

    [SerializeField] Transform SelectionSlots;

    [SerializeField] PlanningUIClearSelection ClearSelection;

    [SerializeField] PlanningUIDragFollower dragFollower;
    Module currentlyDraggedModule = null;


    [SerializeField] PlanningUIInfoDisplay InfoDisplay;


    [SerializeField] CombatManager combatManager;


    Dictionary<Module, PlanningUIModuleSlot> moduleSlotsCurrentState = new Dictionary<Module, PlanningUIModuleSlot>();
    Dictionary<Module, PlanningUISelectionSlot> selectionCurrentState = new Dictionary<Module, PlanningUISelectionSlot>();

    void Awake() {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void Destroy() {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        DeckManager.instance.DeckChangedCallback -= ListModules;
    }

    void Start() {
        // Initialize Listener
        DeckManager.instance.DeckChangedCallback += ListModules;

        // Initialize Selection Slots
        foreach (Transform child in SelectionSlots) {
            PlanningUISelectionSlot slot = child.gameObject.GetComponent<PlanningUISelectionSlot>();
            slot.ResetSlot();

            slot.OnModuleClicked += HandleModuleClick;
            slot.OnModuleBeginDrag += HandleModuleSelection;
            slot.OnModuleEndDrag += HandleEndDrag;
            slot.OnModuleDrop += HandleModuleDrop;
        }

        // Initialize Clear Selection
        ClearSelection.OnModuleDrop += HandleModuleDrop;

        // Initialize Active State
        planningUI.SetActive(GameStateManager.Instance.CurrentGameState == GameState.Planning);
    }

    public void ListModules() {
        foreach (Transform child in ModuleSlots) {
            Destroy(child.gameObject);
        }

        moduleSlotsCurrentState.Clear();

        foreach (Module module in DeckManager.instance.deck) {
            GameObject obj = Instantiate(ModuleSlotPrefab, ModuleSlots);
            
            PlanningUIModuleSlot slot = obj.GetComponent<PlanningUIModuleSlot>();
            slot.Set(module);

            slot.OnModuleClicked += HandleModuleClick;
            slot.OnModuleBeginDrag += HandleModuleSelection;
            slot.OnModuleEndDrag += HandleEndDrag;

            moduleSlotsCurrentState.Add(module, slot);
        }

        UpdateModuleSlots();
    }

    // Update module slots (enable/disable) when selections changes
    void UpdateModuleSlots() {
        selectionCurrentState.Clear();
        foreach (Transform child in SelectionSlots) {
            PlanningUISelectionSlot slot = child.gameObject.GetComponent<PlanningUISelectionSlot>();
            if (slot.module != null) {
                selectionCurrentState.Add(slot.module, slot);
            }
        }

        /*
        foreach (Transform child in ModuleSlots) {
            PlanningUIModuleSlot slot = child.gameObject.GetComponent<PlanningUIModuleSlot>();
            if (!slot.module.planningAvailable) {
                slot.Disable("Unavailable");
                continue;
            }

            if (selection.Contains(slot.module)) {
                slot.Disable("Selected");
                continue;
            }

            slot.Enable();
        }
        */

        foreach (Module module in moduleSlotsCurrentState.Keys) {
            PlanningUIModuleSlot slot = moduleSlotsCurrentState[module];
            if (!module.planningAvailable) {
                slot.Disable("Unavailable");
                continue;
            }

            if (selectionCurrentState.ContainsKey(module)) {
                slot.Disable("Selected");
                continue;
            }

            slot.Enable();
        }
    }


    // --- MODULE SLOT ---
    // Handle On Click
    void HandleModuleClick(PlanningUIModuleSlot moduleSlot) {
        InfoDisplay.DisplayModule(moduleSlot.module);
    }

    // Handle Begin Drag
    void HandleModuleSelection(PlanningUIModuleSlot moduleSlot) {
        currentlyDraggedModule = moduleSlot.module;

        InfoDisplay.DisplayModule(moduleSlot.module);
        dragFollower.Toggle(true);
        dragFollower.Set(moduleSlot.module);
    }

    // Handle End Drag
    void HandleEndDrag(PlanningUIModuleSlot moduleSlot) {
        dragFollower.Toggle(false);
        currentlyDraggedModule = null;
    }


    // --- SELECTION SLOT ---
    // Handle On Click
    void HandleModuleClick(PlanningUISelectionSlot selectionSlot) {
        // Displays for now -- can be used to interact differently (ex. click to remove)
        InfoDisplay.DisplayModule(selectionSlot.module);

    }

    // Handle Begin Drag
    void HandleModuleSelection(PlanningUISelectionSlot selectionSlot) {
        currentlyDraggedModule = selectionSlot.module;

        InfoDisplay.DisplayModule(selectionSlot.module);
        dragFollower.Toggle(true);
        dragFollower.Set(selectionSlot.module);

        ClearSelection.Toggle(true);
    }

    // Handle End Drag
    void HandleEndDrag(PlanningUISelectionSlot selectionSlot) {
        dragFollower.Toggle(false);
        currentlyDraggedModule = null;

        ClearSelection.Toggle(false);
    }

    // Handle On Drop
    void HandleModuleDrop(PlanningUISelectionSlot selectionSlot) {
        if (currentlyDraggedModule == null) {
            return;
        }

        // TODO: need to handle swapping if needed
        // Handle: 
            // Modules -> Empty Selection
            // Modules -> Existing Selection
            // Selection -> Empty Selection
            // Selection -> Existing Selection

        if (selectionCurrentState.ContainsKey(currentlyDraggedModule)) {
            // Selection -> Empty Selection
            if (selectionSlot.IsEmpty()) {
                selectionCurrentState[currentlyDraggedModule].ResetSlot(); // empty slot the module was previously in
            }

            // Selection -> Existing Selection
            else {
                selectionCurrentState[currentlyDraggedModule].Set(selectionSlot.module); 
            }

        }

        selectionSlot.Set(currentlyDraggedModule);
        UpdateModuleSlots();

        // Below handled by HandleEndDrag()
        // dragFollower.Toggle(false);
        // currentlyDraggedModule = null;
    }


    // --- CLEAR SELECTION ---
    // Handle Module Drop
    void HandleModuleDrop(PlanningUIClearSelection clearSelection) {
        if (selectionCurrentState[currentlyDraggedModule] == null) {
            Debug.LogError("Module not in selection slot");
            return;
        }

        selectionCurrentState[currentlyDraggedModule].ResetSlot();
        UpdateModuleSlots();
    }


    // --- START BUTTON ---
    // Handle On Click
    public void OnStartClick() {
        // UpdateModuleSlots just in case
        UpdateModuleSlots();

        // Set selection modules
        SetSelection();

        // Start combat 
        combatManager.StartCombat();
        Debug.Log("Starting Battle");

        // Disable planning UI
        planningUI.SetActive(false);
    }

    void SetSelection() {
        // Set modules in CombatManager
        combatManager.modules.Clear();
        combatManager.modules = new List<Module>(selectionCurrentState.Keys);
        combatManager.modules.Reverse();
        Debug.Log(combatManager.modules.Count);
        for (int i = 0; i < combatManager.modules.Count; i++) {
            Debug.Log(combatManager.modules[i]);
        }

        // Disable selected modules for future rounds of planning
        foreach(Module module in combatManager.modules) {
            DeckManager.instance.ToggleModuleAvailability(module, false);
        }

        // Clear selections
        ClearAllSelectionSlots();

        // Update slots
        ListModules();
    }

    void ClearAllSelectionSlots() {
        foreach (Transform child in SelectionSlots) {
            PlanningUISelectionSlot slot = child.gameObject.GetComponent<PlanningUISelectionSlot>();
            slot.ResetSlot();
        }

        UpdateModuleSlots();
    }


    // --- SWITCHING TO PLANNING PHASE ---
    void OnGameStateChanged(GameState newState) {
        if (newState == GameState.Planning
            /*&& GameStateManager.Instance.PreviousGameState == GameState.Combat*/
            )
        {
            planningUI.SetActive(true);
            ListModules();
        }
    }


    // --- OTHERS ---
}