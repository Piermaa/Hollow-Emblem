using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public string[] ammo;
    bool shooting;

    float rangedAttackTimer;
    float changePosTimer;
    float lowAttackTimer;

    public float rangedAttackCooldown;
    public float changePosCooldown;
    public float lowAttackCooldown;

    int rangedAttackCounter;
    public int maxRangedAttacks;

    int changeSideUpCounter;
    public int maxChangeSideUp;

    Animator animator;
    ObjectPooler objectPooler;
    [SerializeField] Animator spikesAnimator;
    [SerializeField] GameObject spikesOnBody;
    [SerializeField] Transform shootTransform;
    [SerializeField] GameObject tutorial;
    public GameObject abUnlocker;

    [SerializeField] AudioSource shot;
    [SerializeField] AudioSource intoFlor;
    [SerializeField] AudioSource intoSlime;


    public bool bossInRight;
    public enum BossState
    {
        Solid, Liquid
    }

    public BossState bossState;

    private void Start()
    {
        rangedAttackTimer = rangedAttackCooldown;
        animator = GetComponent<Animator>();
        objectPooler = ObjectPooler.Instance;
    }

    private void Update()
    {

        if(bossState==BossState.Liquid)
            rangedAttackTimer = rangedAttackCooldown;

        if (bossState != BossState.Liquid) 
        {
            rangedAttackTimer -= Time.deltaTime;
        }

        if(rangedAttackTimer<0)
        {
            Shoot();
        }

        if (rangedAttackCounter >= maxRangedAttacks && bossState != BossState.Liquid &&!shooting)
        {
            if (changeSideUpCounter < maxChangeSideUp)
            {
                ChangeSide();
            }
            else
            {
                LowAttack();
            }
        }
    }
    void LowAttack()
    {
        if (bossInRight)
        {
            animator.SetTrigger("LowLeft");
            spikesAnimator.SetTrigger("Left");
            print("moving left");

        }
        else
        {
            animator.SetTrigger("LowRight");
            spikesAnimator.SetTrigger("Right");
            print("moving right");
        }
        StartCoroutine(PlaySoundDelay(intoFlor));
        rangedAttackCounter = 0;
        changeSideUpCounter= 0;

        
    }

    void ChangeSide()
    {
        StartCoroutine(PlaySoundDelay(intoSlime));
        if (bossInRight)
        {
            animator.SetTrigger("Left");
            print("moving left");
         
        }
        else
        {
            animator.SetTrigger("Right");
            print("moving right");
        }
        changeSideUpCounter++;
        rangedAttackCounter = 0;
    }

    
    void Shoot()
    {
        // how i set the rotation is very possibly incorrect because I do not know wich is left or right
        StartCoroutine(PlaySoundDelay(shot));
        Quaternion rotation =Quaternion.Euler( new Vector3(0,0,0));
        Quaternion invertedRotation = Quaternion.Euler(new Vector3(0, 0, -1));
        if (bossState != BossState.Liquid)
        {
      
            Quaternion rot;
            if (bossInRight)
            {
                rot = rotation;
            }
            else
            {
                rot = invertedRotation;
            }

            rangedAttackTimer =rangedAttackCooldown;


            StartCoroutine(Shooting(rot));
            print("spawning bullet");

            rangedAttackCounter++;
        }


    }

    IEnumerator PlaySoundDelay(AudioSource sound)
    {
        yield return new WaitForSeconds(0.2f);
        sound.Play();
    }

    IEnumerator Shooting(Quaternion rotation)
    {
        shooting = true;
        animator.SetTrigger("Shot");
        yield return new WaitForSeconds(0.45f);
        foreach (string ammunition in ammo)
        {
            objectPooler.SpawnFromPool(ammunition, shootTransform.position, rotation);
        }
        shooting = false;
    }
    IEnumerator ShowSpikes()
    {
        spikesOnBody.SetActive(true);
        yield return new WaitForSeconds(1);
        spikesOnBody.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ShowSpikes());

            Rigidbody2D rb;
            HealthController health;
            collision.gameObject.TryGetComponent<HealthController>(out health);
            collision.gameObject.TryGetComponent<Rigidbody2D>(out rb);


            Vector3 dir = collision.transform.position- transform.position;
            dir.Normalize();
            dir = new Vector3(dir.x*50,2);
            rb.AddForce(dir, ForceMode2D.Impulse);

            health.TakeDamage(1);
            
            print("PlayerAttacked");
        }
    }

    public void Death()
    {
        var drop = Instantiate(abUnlocker, transform.position,transform.rotation);
        drop.TryGetComponent<AbilityUnlocker>(out var ability);
        ability.unlockedAb = "Slime";

        tutorial.SetActive(true);
    }
}
