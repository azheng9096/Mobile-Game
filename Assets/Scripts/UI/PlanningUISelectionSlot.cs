using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using UnityEngine.EventSystems;

public class PlanningUISelectionSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;

    [HideInInspector] public Module module;

    [SerializeField] Sprite defaultSprite;

    bool empty = true;
    public event Action<PlanningUISelectionSlot> OnModuleClicked, OnModuleBeginDrag, OnModuleEndDrag, OnModuleDrop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Module module) {
        if (module == null) {
            ResetSlot();
            return;
        }

        this.module = module;

        icon.sprite = module.moduleData.sprite;
        text.text = ""; // placeholder for now
        empty = false;
    }
    
    public void ResetSlot() {
        module = null;

        icon.sprite = defaultSprite;
        text.text = "";
        empty = true;
    }

    public bool IsEmpty() {
        return empty;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (empty) {
            return;
        }

        if (eventData.pointerId == 0) {
            OnModuleClicked?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (empty) {
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

    public void OnDrop(PointerEventData eventData) {
        OnModuleDrop?.Invoke(this);
    }
}
