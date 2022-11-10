using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningUI : MonoBehaviour
{
    [SerializeField] Transform ModuleSlots;
    [SerializeField] GameObject ModuleSlotPrefab;

    [SerializeField] Transform SelectionSlots;

    [SerializeField] PlanningUIDragFollower dragFollower;
    Module currentlyDraggedModule = null;

    [SerializeField] PlanningUIInfoDisplay InfoDisplay;


    Dictionary<Module, PlanningUIModuleSlot> moduleSlotsCurrentState = new Dictionary<Module, PlanningUIModuleSlot>();
    List<Module> selection = new List<Module>();

    void Start() {
        dragFollower.Toggle(false);

        // Initialize Selection Slots
        foreach (Transform child in SelectionSlots) {
            PlanningUISelectionSlot slot = child.gameObject.GetComponent<PlanningUISelectionSlot>();
            slot.ResetSlot();

            slot.OnModuleClicked += HandleModuleClick;
            slot.OnModuleBeginDrag += HandleModuleSelection;
            slot.OnModuleEndDrag += HandleEndDrag;
            slot.OnModuleDrop += HandleModuleDrop;
        }
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

        UpdateSelections();
    }

    // Update module slots (enable/disable) when selections changes
    void UpdateSelections() {
        selection.Clear();
        foreach (Transform child in SelectionSlots) {
            PlanningUISelectionSlot slot = child.gameObject.GetComponent<PlanningUISelectionSlot>();
            if (slot.module != null) {
                selection.Add(slot.module);
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

            if (selection.Contains(module)) {
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
    }

    // Handle End Drag
    void HandleEndDrag(PlanningUISelectionSlot selectionSlot) {
        dragFollower.Toggle(false);
        currentlyDraggedModule = null;
    }

    // Handle On Drop
    void HandleModuleDrop(PlanningUISelectionSlot selectionSlot) {
        if (currentlyDraggedModule == null) {
            return;
        }

        if (selectionSlot.IsEmpty()) {
            selectionSlot.Set(currentlyDraggedModule);
            UpdateSelections();
        }

        // TODO: need to handle swapping if needed

        dragFollower.Toggle(false);
        currentlyDraggedModule = null;
    }
}