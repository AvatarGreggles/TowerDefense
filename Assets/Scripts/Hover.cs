using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer;

    private SpriteRenderer rangeRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rangeRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();

    }

    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }
    }

    public void Activate(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;
        rangeRenderer.enabled = true;
    }

    public void Deactivate()
    {
        spriteRenderer.enabled = false;
        spriteRenderer.sprite = null;
        rangeRenderer.enabled = false;
        GameManager.Instance.ClickedButton = null;
    }
}
