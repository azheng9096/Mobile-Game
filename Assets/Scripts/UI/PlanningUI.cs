using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningUI : MonoBehaviour
{
    [SerializeField] Transform ModuleSlots;
    [SerializeField] GameObject ModuleSlotPrefab;

    [SerializeField] PlanningUIDragFollower dragFollower;

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
            slot.Initialize(module);

            slot.OnModuleClicked += HandleModuleClick;
            slot.OnModuleBeginDrag += HandleModuleSelection;
            slot.OnModuleEndDrag += HandleEndDrag;
        }
    }

    void HandleModuleClick(PlanningUIModuleSlot moduleSlot) {
        InfoDisplay.DisplayModule(moduleSlot.module);
    }

    void HandleModuleSelection(PlanningUIModuleSlot moduleSlot) {
        dragFollower.Toggle(true);
        dragFollower.Set(moduleSlot.module);
    }

    void HandleEndDrag(PlanningUIModuleSlot moduleSlot) {
        dragFollower.Toggle(false);
    }
}