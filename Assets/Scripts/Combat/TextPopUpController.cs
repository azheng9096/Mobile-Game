using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TextPopUpController : MonoBehaviour
{
    // Start is called before the first frame update

    public float fontSpeed = 2f;
    public float fadeSpeed = 20f;
    public float moveSpeed = 1f;
    TextMeshPro text;
    bool started = false;

    public void Init(string message, Color color) {
        text = GetComponent<TextMeshPro>();
        text.color = color;
        text.text = message;
        print(message);
        print("-------------------------");
        StartCoroutine(DelayDestroy());
        started = true;
    }

    public void Check() {
        print("Hihdoa;sdofj");
    }

    IEnumerator DelayDestroy() {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (started) {
            transform.position = transform.position + new Vector3(0, moveSpeed * Time.deltaTime, 0);
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Max(text.color.a - fadeSpeed * Time.deltaTime, 0));
            text.fontSize = Mathf.Max(text.fontSize - fontSpeed * Time.deltaTime, 0);
            print(text.fontSize + " " + text.color.a);
        }
    }

}
