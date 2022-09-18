using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class HealthController : MonoBehaviour
{
    public bool inmune = false;
    public int healthPoints = 100;
    public int maxHealth; 
    [SerializeField]SpriteRenderer sprite;
    StaminaController _staminaController;

    public bool white;
    public UnityEvent DieEvent;
    public AudioSource takeDamageSound;

    bool takingDamage;
    public float playerInmunity=1;
    private void Start()
    {
        if (sprite==null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        if (maxHealth != 0) FullHeal();

        TryGetComponent<StaminaController>(out _staminaController);
    }

    private void Update()
    {
        if (maxHealth!=0)
        {
            if(healthPoints>maxHealth)
            {
                healthPoints = maxHealth;
            }
        }
    }
    IEnumerator DamageTaken(Color color)
    {
        takingDamage = true;
        yield return new WaitForSeconds(0.1f*playerInmunity);
        sprite.color = color;
        takingDamage = false;
    }

    public void TakeDamage(int damage)
    {
        takeDamageSound.Play();
        if (!inmune && healthPoints>0 &&!takingDamage)
        {
            if (_staminaController != null)
            {
                if (!_staminaController.CheckStamina())
                {
                    healthPoints -= 1000;
                }
                {
                    healthPoints -= damage;
                }
            }
            else
            {
                healthPoints -= damage;
            }
            Color col;
            if (white)
            {
                col = Color.white;
            }
            else
            {
                col = sprite.color;
            }
          
            var red = new Color(255, 0, 0);
            
            sprite.color = red;

            StartCoroutine(DamageTaken(col));
        }

        if(healthPoints<=0)
        {
            Death();
        }
    }
    public void Heal(int hpAdded)
    {
        healthPoints += hpAdded;
    }

    public void FullHeal()
    {
        healthPoints = maxHealth;
    }

    public void AddMaxHealth(int healthAdded)
    {
        maxHealth+=healthAdded;
        FullHeal();
    }

    public void Death()
    {
       
        if (DieEvent!=null)
        {
            PlayerMovement playerMovement;
            TryGetComponent<PlayerMovement>(out playerMovement);
            if (playerMovement==null)
            {
               
                this.gameObject.SetActive(false);
            }
            DieEvent.Invoke();
        }
        

        if (DieEvent==null)
        {
            this.gameObject.SetActive(false);
        }
       
    }
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
