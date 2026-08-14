using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingController : MonoBehaviour
{
    [SerializeField] private Renderer red;
    [SerializeField] private float horizontal_speed;
    private float scrollOffset;

    public bool IsScrolling { get; private set; } = true;

    // Update is called once per frame
    void Update()
    {
        // Blocking translation due to scrolling
        if (!IsScrolling)
        {
            return;
        }

        scrollOffset += horizontal_speed * Time.deltaTime;
        red.material.mainTextureOffset = new Vector2(scrollOffset, 0f);

    }

    public void StopScrolling()
    {
        IsScrolling = false;
    }

    public void StartScrolling()
    {
        IsScrolling = true;
    }
}
