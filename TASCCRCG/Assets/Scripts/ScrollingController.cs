using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingController : MonoBehaviour
{
    [SerializeField] private Renderer red;
    [SerializeField] private float horizontal_speed;

    public bool IsScrolling { get; private set; } = true;

    // Update is called once per frame
    void Update()
    {
        // Blocking translation due to scrolling
        if (!IsScrolling)
        {
            return;
        }

        Vector2 offset = new Vector2(Time.time * horizontal_speed, 0f);
        red.material.mainTextureOffset = offset;
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
