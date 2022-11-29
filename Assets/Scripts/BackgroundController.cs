using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour
{
    public List<Sprite> backgrounds;
    public Sprite startBackground;
    public GameObject backgroundContainerPrefab;
    List<GameObject> backgroundContainers;
    GameObject startContainer;
    public float backgroundHeight;
    public float moveSpeed;
    public float delayStart = 1.0f;
    bool moving = false;
    void Start()
    {
        backgroundContainers = new List<GameObject>();
        Vector3 pos = transform.position;
        startContainer = Instantiate(backgroundContainerPrefab, transform);
        startContainer.transform.position = pos;
        startContainer.GetComponent<SpriteRenderer>().sprite = startBackground;

        for(int i = 0; i < backgrounds.Count; i++)
        {
            pos.y += backgroundHeight;
            GameObject backgroundContainer = Instantiate(backgroundContainerPrefab, transform);
            backgroundContainer.transform.position = pos;
            backgroundContainer.GetComponent<SpriteRenderer>().sprite = backgrounds[i];
            backgroundContainers.Add(backgroundContainer);
        }
        StartCoroutine(DelayStart());
    }
    IEnumerator DelayStart() {
        yield return new WaitForSeconds(delayStart);
        moving = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!moving) {
            return;
        }
        if (startContainer != null) {
            Vector3 pos = startContainer.transform.position;
            pos.y -= moveSpeed * Time.deltaTime;
            startContainer.transform.position = pos;
            if (pos.y < transform.position.y-backgroundHeight) {
                Destroy(startContainer);
            }
        }
        foreach(GameObject backgroundContainer in backgroundContainers)
        {
            Vector3 pos = backgroundContainer.transform.position;
            pos.y -= moveSpeed * Time.deltaTime;
            if (pos.y < transform.position.y-backgroundHeight)
            {
                pos.y += backgroundHeight * backgrounds.Count;
            }
            backgroundContainer.transform.position = pos;
        }
    }
}
