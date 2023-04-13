using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform transform;
    [SerializeField] private float speed = 2.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void move(bool direction)
    {
        transform.localScale = new Vector3(direction ? 1 : -1, 1, 1);
        transform.Translate(Time.deltaTime * speed * (direction ? -1 : 1), 0, 0);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        animator.Play("MoveAnim");
    }

    private void fly(bool direction)
    {
        transform.localScale = new Vector3(direction ? 1 : -1, 1, 1);
        transform.Translate(Time.deltaTime * speed * (direction ? -1 : 1), 0, 0);
        transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
        animator.Play("FlyAnim");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.W))
            {
                fly(true);
            }
            else
            {
                move(true);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.W))
            {
                fly(false);
            }
            else
            {
                move(false);
            }
        }
        else
        {
            animator.Play("IdleAnim");
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}
