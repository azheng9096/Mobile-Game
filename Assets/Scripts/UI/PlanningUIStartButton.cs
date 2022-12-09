using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanningUIStartButton : MonoBehaviour
{
    Button button;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetInteractable(bool val) {
        if (button == null)
            return;
        if (val) {
            button.interactable = true;
            text.color = Color.white;
        } else {
            button.interactable = false;
            text.color = Color.grey;
        }
    }
}
