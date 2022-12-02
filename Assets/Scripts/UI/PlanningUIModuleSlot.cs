using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using UnityEngine.EventSystems;

public class PlanningUIModuleSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject disable;
    [SerializeField] TextMeshProUGUI disableText;

    [HideInInspector] public Module module;


    public event Action<PlanningUIModuleSlot> OnModuleClicked, OnModulePointerExit, OnModulePointerDown, OnModulePointerUp;
    public event Action<PlanningUIModuleSlot, PointerEventData> OnModuleBeginDrag, OnModuleEndDrag, OnModuleOnDrag;

    Image raycastImage;
    bool draggable = true;

    void Start() {
        raycastImage = gameObject.GetComponent<Image>();
    }

    public void Set(Module newModule) {
        module = newModule;

        icon.sprite = module.moduleData.sprite;
        text.text = ""; // placeholder for now

        if (!module.planningAvailable) {
            draggable = false;
            Disable("Unavailable");
        }
    }

    public void Disable(string message = "Disabled") {
        draggable = false;
        disable.SetActive(true);
        disableText.text = message;
    }

    public void Enable() {
        draggable = true;
        disable.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        // 0 = touch, -1 = left click mouse
        if (eventData.pointerId == 0 || eventData.pointerId == -1) {
            OnModuleClicked?.Invoke(this);
        }
    }


    public void OnBeginDrag(PointerEventData eventData) {
        if (!draggable) {
            return;
        }
        OnModuleBeginDrag?.Invoke(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {
        OnModuleEndDrag?.Invoke(this, eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        // needed for OnBeginDrag() and OnEndDrag() to work
        // no implementation needed - unless may want to use it for scroll priority issues

        OnModuleOnDrag?.Invoke(this, eventData);
    }


    // These methods below mainly for dealing with scroll priority (does not need to be implemented)
    public void OnPointerExit(PointerEventData eventData) {
        // 0 = touch, -1 = left click mouse
        if (eventData.pointerId == 0 || eventData.pointerId == -1) {
            OnModulePointerExit?.Invoke(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        // 0 = touch, -1 = left click mouse
        if (eventData.pointerId == 0 || eventData.pointerId == -1) {
            OnModulePointerUp?.Invoke(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        // 0 = touch, -1 = left click mouse
        if (eventData.pointerId == 0 || eventData.pointerId == -1) {
            OnModulePointerDown?.Invoke(this);
        }
    }
}

