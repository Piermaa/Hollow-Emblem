using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int damage;

    [SerializeField]PlayerSounds sounds;

    Rigidbody2D rb;
    Animator animator;

    public Transform playerCenter; //saves the position of the player center, this to attack from there when liquid
    public Transform attackPosition; // saves the transform of the normal attack in case of changing to liquid

    public Transform attackPoint; // the transform used when attacking
    public Transform[] attackDirections; //0 forward 1up 2down

    Vector2 attackDirection;
    public float attackRange = .8f;
    public LayerMask enemyLayer;

    public bool canAttack;

    public enum DirectionsToAttack
    {
        Up,Down,Front
    }
    public DirectionsToAttack directionsToAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        SetAttackDirection();
        if(Input.GetButtonDown("Attack") && canAttack)
        {
            Attack();
        }
    }


    void SetAttackDirection()
    {
        float y = Input.GetAxis("Vertical");


        if(y==0)
        {
            directionsToAttack = DirectionsToAttack.Front;
            attackPoint.position = attackDirections[0].position;
            attackDirection = new Vector2((transform.position.x) - (attackPoint.position.x), 0)*6;
        }
        if(y>0)
        {
            directionsToAttack = DirectionsToAttack.Up;
            attackPoint.position = attackDirections[1].position;
            attackDirection = new Vector2(0,0);
        }
    
        if(y<0)
        {
            directionsToAttack = DirectionsToAttack.Down;
            attackPoint.position = attackDirections[2].position;
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
                Debug.Log(enemy.name + "hitted");
            }
   
        }
    }
}
