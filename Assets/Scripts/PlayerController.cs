using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject chopper;
    public GameObject smoke;
    public float moveSpeed;
    
    void FixedUpdate()
    {
        Move();
        if (FindObjectOfType<GameController>().isDead)
        {            
            FindObjectOfType<GameController>().isDead = false;
            Destroy(gameObject);
        }
    }    
    private void Move()
    {
        if (moveSpeed == 0)
            return;

        if (FindObjectOfType<GameController>().isStart)
        {
            chopper.transform.Translate(0, 0, 0);
            
        }

        if (Input.GetMouseButton(0))
        {
            FindObjectOfType<GameController>().isStart = false;
            chopper.transform.Translate(0, moveSpeed * Time.deltaTime, 0);
            GetComponent<Animator>().SetBool("Fly",true);
        }
        else if(!FindObjectOfType<GameController>().isStart)
        {
            chopper.transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
            GetComponent<Animator>().SetBool("Fly", false);
            GetComponent<Animator>().SetBool("FlyTop", false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("barrier") || col.CompareTag("building"))
        {
            FindObjectOfType<GameController>().isDead = true;
        }        
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Respawn"))
        {
            GetComponent<Animator>().SetBool("FlyTop", true);
        }
    }
}
