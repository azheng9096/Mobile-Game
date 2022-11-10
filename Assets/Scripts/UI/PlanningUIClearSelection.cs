using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PlanningUIClearSelection : MonoBehaviour, IDropHandler
{
    public event Action<PlanningUIClearSelection> OnModuleDrop;

    public void Toggle(bool val) {
        gameObject.SetActive(val);
    }

    public void OnDrop(PointerEventData eventData) {
        OnModuleDrop?.Invoke(this);
    }
}
