using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    private List<GameObject> poolObjects = new List<GameObject>();

    [SerializeField] GameObject[] objectPrefabs;

    public GameObject GetObject(string type)
    {

        foreach (GameObject go in poolObjects)
        {
            if (!go.activeInHierarchy && go.name == type)
            {
                go.SetActive(true);
                return go;
            }

        }

        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].name == type)
            {
                GameObject newObject = Instantiate(objectPrefabs[i]);
                poolObjects.Add(newObject);
                newObject.name = type;
                return newObject;
            }
        }

        return null;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);

    }
}
