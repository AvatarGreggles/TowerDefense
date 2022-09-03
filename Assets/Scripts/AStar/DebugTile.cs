using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugTile : MonoBehaviour
{

    [SerializeField] private TMP_Text g;
    [SerializeField] private TMP_Text h;
    [SerializeField] private TMP_Text f;

    public TMP_Text G
    {
        get
        {
            g.gameObject.SetActive(true);
            return g;
        }

        set
        {
            this.g = value;
        }
    }

    public TMP_Text H
    {
        get
        {
            h.gameObject.SetActive(true);
            return h;
        }

        set
        {
            this.h = value;
        }
    }

    public TMP_Text F
    {
        get
        {
            f.gameObject.SetActive(true);
            return f;
        }

        set
        {
            this.f = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
