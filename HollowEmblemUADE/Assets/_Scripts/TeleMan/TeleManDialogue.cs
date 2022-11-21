using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleManDialogue : MonoBehaviour
{
    public Text telemanText;

    public GameObject dataText;
    public GameObject secondDataText;

    public bool canInteract;
    public bool hasTalked;

    public GameObject target;
    public GameObject newTarget;

    public Rigidbody2D rb;

    public float cooldown = 0.5f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        cooldown -= Time.deltaTime;

        if ( cooldown < 0)
        {
            cooldown = 0;
        }

        ChangeText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            telemanText.text = "Hi There!";
        }
    }

    private void OnTriggerStay2D(Collider2D collision)  //Can´t detect inputs
    {
        canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            telemanText.text = "";
            canInteract = false;
            hasTalked = false;
            animator.SetBool("isTalking", false);
            target.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    void ChangeText()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract && !hasTalked && cooldown <= 0)
        {
            //StartCoroutine(TimeToInteract());
            telemanText.text = "You´re not the first one on getting here.";
            //target.transform.position = newTarget.transform.position;
            //rb.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetBool("isTalking", true);
            cooldown = 0.5f;
            hasTalked = true;
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && canInteract && hasTalked && cooldown <= 0)
        {
            //StartCoroutine(TimeToInteract());
            telemanText.text = "I think that there´s a gun for you nearby!";
            cooldown = 0.5f;
            //target.transform.position = newTarget.transform.position;
            //rb.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
    }

    //IEnumerator TimeToInteract()
    //{
    //    canInteract = false;
    //    yield return new WaitForSeconds(0.5f);
    //    canInteract = true;
    //}
}
