using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using UnityEngine.EventSystems;

public class PlanningUIModuleSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;

    [HideInInspector] public Module module;


    public event Action<PlanningUIModuleSlot> OnModuleClicked, OnModuleBeginDrag, OnModuleEndDrag;

    bool draggable = true;

    public void Set(Module newModule) {
        module = newModule;

        icon.sprite = module.moduleData.sprite;
        text.text = ""; // placeholder for now
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.pointerId == 0) {
            OnModuleClicked?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!draggable) {
            return;
        }
        OnModuleBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData) {
        OnModuleEndDrag?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData) {
        // needed for OnBeginDrag() and OnEndDrag() to work
        // no implementation needed
    }
}

