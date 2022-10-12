using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    public Transform playerTransform;
    public PlayerMovement playerMovement;
    public Rigidbody2D rb;

    Vector3 startPos;

    Transform newTarget;
    bool mustAim;
    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        startPos = playerTransform.position;
        vcam.Follow = playerTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (mustAim)
        {
            StartCoroutine(Awaiting());
        }
        if(!mustAim && playerTransform.localPosition != Vector3.zero)
        {
            playerTransform.localPosition = Vector3.zero;
        }
    }

    public void ChangeTarget(Transform targetToAim)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        playerMovement.enabled = false;
        newTarget = targetToAim;
        mustAim=true;
    }

    private IEnumerator Awaiting()
    {
    
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            playerTransform.position = Vector2.MoveTowards(playerTransform.position, newTarget.position, i*1/20);
            yield return null;
        }
        Debug.Log("Esperando");

        yield return new WaitForSeconds(3);
        mustAim = false;
        playerTransform.localPosition = Vector3.zero;
        playerMovement.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
