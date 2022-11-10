using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningUIDragFollower : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Camera mainCam;

    [SerializeField] PlanningUIModuleSlot moduleSlot;

    void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        mainCam = Camera.main;

        moduleSlot = GetComponentInChildren<PlanningUIModuleSlot>();
    }


    public void Set(Module module) {
        moduleSlot.Set(module);
    }


    void Update()
    {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.GetTouch(0).position,
            canvas.worldCamera,
            out position
        );

        transform.position = canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool val) {
        gameObject.SetActive(val);
    }
}
