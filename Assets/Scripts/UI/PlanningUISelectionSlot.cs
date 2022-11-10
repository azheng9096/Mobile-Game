using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System;
using UnityEngine.EventSystems;

public class PlanningUISelectionSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;

    [HideInInspector] public Module module;

    [SerializeField] Sprite defaultSprite;

    // bool empty = true;
    public event Action<PlanningUISelectionSlot> OnModuleDrop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Module module) {
        this.module = module;

        icon.sprite = module.moduleData.sprite;
        text.text = ""; // placeholder for now
    }
    
    public void ResetSlot() {
        module = null;

        icon.sprite = defaultSprite;
        text.text = "";
    }

    public void OnDrop(PointerEventData eventData) {
        OnModuleDrop?.Invoke(this);
    }
}
