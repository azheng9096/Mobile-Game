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

    void Start() {
        dragFollower.Toggle(false);
    }

    public void ListModules() {
        foreach (Transform child in ModuleSlots) {
            Destroy(child.gameObject);
        }

        foreach (Module module in DeckManager.instance.deck) {
            GameObject obj = Instantiate(ModuleSlotPrefab, ModuleSlots);
            
            PlanningUIModuleSlot slot = obj.GetComponent<PlanningUIModuleSlot>();
            slot.Set(module);

            slot.OnModuleClicked += HandleModuleClick;
            slot.OnModuleBeginDrag += HandleModuleSelection;
            slot.OnModuleEndDrag += HandleEndDrag;
        }

        // Reset Selection Slots
        foreach (Transform child in SelectionSlots) {
            PlanningUISelectionSlot slot = child.gameObject.GetComponent<PlanningUISelectionSlot>();
            slot.ResetSlot();

            slot.OnModuleDrop += HandleModuleDrop;
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
    // Handle On Drop
    void HandleModuleDrop(PlanningUISelectionSlot selectionSlot) {
        if (currentlyDraggedModule == null) {
            return;
        }

        // need to handle swapping if needed
        selectionSlot.Set(currentlyDraggedModule);

        dragFollower.Toggle(false);
        currentlyDraggedModule = null;
    }
}