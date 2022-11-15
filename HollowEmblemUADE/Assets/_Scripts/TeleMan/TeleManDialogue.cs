using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleManDialogue : MonoBehaviour
{
    public GameObject hiText;
    public GameObject dataText;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hiText.SetActive(true);
            dataText.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            hiText.SetActive(false);
            dataText.SetActive(true);
            animator.SetBool("isTalking", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hiText.SetActive(false);
            dataText.SetActive(false);
            animator.SetBool("isTalking", false);
        }
    }
}
