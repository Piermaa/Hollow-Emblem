using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public LayerMask enemyLayer;
    Vector2 attackDirection;

    [Header("Classes")]
    PlayerInventory inventory;
    [SerializeField] PlayerSounds sounds;
    Rigidbody2D rb;
    Animator animator;
    CharacterController2D controller;

    [Header("Transforms")]
    public Transform playerCenter; //saves the position of the player center, this to attack from there when liquid
    public Transform attackPosition; // saves the transform of the normal attack in case of changing to liquid
    public Transform shootStart;
    public Transform attackPoint; // the transform used when attacking
    public Transform[] attackDirections; //0 forward 1up 2down
    public Transform[] shootDirections;

    [Header("Bools")]
    public bool canAttack;
    public bool aiming;
    public bool reloading;
    public bool showingInventory;
    public bool canShoot;

    [Header("Floats")]
    public float attackRange = .8f;

    [Header("Int")]
    public int damage;
    public int shootDamage;
    public int maxAmmo = 10;
    public int currentAmmo;
    public List<GameObject> ammo;



    public enum DirectionsToAttack
    {
        Up,Down,Front
    }
    public DirectionsToAttack directionsToAttack;

    private void Start()
    {
        inventory = PlayerInventory.Instance;
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentAmmo = maxAmmo;
    }
    public void Update()
    {
        if (canShoot)
        {
            SetAttackDirection();
            Aim();

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

        }
        
        if (Input.GetButtonDown("Attack") && canAttack)
        {
            Attack();
        }

        if (reloading)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        //Debug.DrawRay(shootStart.position, Vector2.right*100,Color.red);
        Debug.DrawRay(shootStart.position, shootStart.TransformDirection(Vector2.left) * 100, Color.red);
    }

    void SetAttackDirection()
    {
        float y = Input.GetAxis("Vertical");

        if(y==0)
        {
            directionsToAttack = DirectionsToAttack.Front;
            attackPoint.position = attackDirections[0].position;
            shootStart.position = shootDirections[0].position;
            shootStart.rotation = shootDirections[0].rotation;
            attackDirection = new Vector2((transform.position.x) - (attackPoint.position.x), 0)*6;
        }
        if(y>0)
        {
            directionsToAttack = DirectionsToAttack.Up;
            attackPoint.position = attackDirections[1].position;
            shootStart.position = shootDirections[1].position;
            shootStart.rotation = shootDirections[1].rotation;
            attackDirection = new Vector2(0,0);
        }
    
        if(y<0)
        {
            directionsToAttack = DirectionsToAttack.Down;
            attackPoint.position = attackDirections[2].position;
            shootStart.position = shootDirections[2].position;
            shootStart.rotation = shootDirections[2].rotation;
            attackDirection = Vector2.up*6 /*new Vector2(0, (transform.position.y) - (attackPoint.position.y))*/;
        }
    }

    /// <summary>
    /// Attacks using overlapcricleall, triggers the correct animation and inflict damage on enemies
    /// </summary>
    void Attack()
    {
        sounds.PlaySound(sounds.attack);
        animator.SetBool("Jump", false);
        switch (directionsToAttack)
        {
            case DirectionsToAttack.Front:
                animator.SetTrigger("AttackFront");
                break;
            case DirectionsToAttack.Down:
                animator.SetTrigger("AttackDown");
                break;
            case DirectionsToAttack.Up:
                animator.SetTrigger("AttackUp");
                break;
        }
      

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange,enemyLayer);

        if(hitEnemies.Length >0)
        {
            if(attackDirection.y>0)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            rb.AddForce(attackDirection,ForceMode2D.Impulse);
            sounds.PlaySound(sounds.inflictdDmg);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            HealthController health;
            enemy.TryGetComponent<HealthController>(out health);

            if (health!=null)
            {
                health.TakeDamage(damage);
            }
   
        }
    }

    void Aim()
    {
        if (Input.GetMouseButton(1) && controller.CheckGround())
        {
            
            canAttack = false;
            aiming = true;

            switch (directionsToAttack)
            {

                case DirectionsToAttack.Front:
                    animator.SetTrigger("AimFront");
                    break;
                case DirectionsToAttack.Down:
                    animator.SetTrigger("AimDown");
                    break;
                case DirectionsToAttack.Up:
                    animator.SetTrigger("AimUp");
                    break;
            }

            Shoot();
        }
        else if (!reloading)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            aiming = false;
        }
        animator.SetBool("Aiming", aiming);
    }
    void Shoot()
    {
        RaycastHit2D hit;

        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            hit = Physics2D.Raycast(shootStart.position, shootStart.TransformDirection(Vector2.left), 50, enemyLayer); // the bug was the direction, as the shootstart is rotated... (1/2)
            //depending of the aimDirection transform we have to use TransformDirection(new Vector(direction)), also enemyLayer as it works as int it worked as distance
            currentAmmo--;

            UpdateUI();

            if (hit)
            {
                Debug.Log(hit.collider.name);   
                hit.collider.TryGetComponent < HealthController >(out var health);
                if(health!=null)
                {
                    health.TakeDamage(shootDamage);
                }
                
            }
        }
    }

    public void Reload()
    {
        if ( controller.CheckGround() && maxAmmo>currentAmmo)
        {
            StartCoroutine(Reloading());
            animator.SetTrigger("Reload");
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < ammo.Count; i++)
        {
            if (i < currentAmmo)
            {
                ammo[i].SetActive(true);
            }
            else
            {
                ammo[i].SetActive(false);
            }
        }
    }

    IEnumerator Reloading()
    {
        reloading = true;

        yield return new WaitForSeconds(1);
        inventory.GetAmmoFromInventory();
        UpdateUI();
        reloading = false;
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, shootStart.TransformDirection(Vector3.forward)*100);
    }
}
