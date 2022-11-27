using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    public float scrollSpeed;
    Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void LateUpdate()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, 20);
        transform.position = startPosition + Vector2.right * newPos;
    }
}
