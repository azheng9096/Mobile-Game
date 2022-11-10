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
    [SerializeField] GameObject disable;
    [SerializeField] TextMeshProUGUI disableText;

    [HideInInspector] public Module module;


    public event Action<PlanningUIModuleSlot> OnModuleClicked, OnModuleBeginDrag, OnModuleEndDrag;

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

