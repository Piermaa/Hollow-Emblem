using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
public class HealthController : MonoBehaviour
{
    [Header("Classes")]
    [SerializeField] SpriteRenderer sprite;
    public AudioSource takeDamageSound;
    public Material takingDamageMaterial;
    Material baseMaterial;
    private UIHealth uiHealth;
    
    [Header("Bool")]
    public bool inmune = false;
    public bool white;

    bool takingDamage;
    [Header("Int")]
    public int healthPoints = 100;
    public int maxHealth;
 
    public float playerInmunity = 1;

    [Header("Events")]
    public UnityEvent OnHealthAdd;
    public UnityEvent DieEvent;


    private void Awake()
    {
        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        uiHealth = FindObjectOfType<UIHealth>();
        
    }

    private void Start()
    {
        baseMaterial = sprite.material;
        if (maxHealth != 0) FullHeal();

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

        if (healthPoints <= 0)
        {
            Death();
        }
    }
    IEnumerator DamageTaken()
    {
        takingDamage = true;
        yield return new WaitForSeconds(0.1f*playerInmunity);
        sprite.material = baseMaterial;
        takingDamage = false;
    }
    /// <summary>
    /// Reduces healthpoints and starts a coroutine that changes the target's sprite color
    /// </summary>
    /// <param name="damage">Amount of damage</param>
    public void TakeDamage(int damage)
    {
        takeDamageSound.Play();
        if (!inmune && healthPoints>0 &&!takingDamage)
        {   
            healthPoints -= damage;
            uiHealth.hasTakeDamage = true;

            sprite.material = takingDamageMaterial;

            //Color col = sprite.color;
            //var red = new Color(255, 0, 0);
            
            //sprite.color = red;

            StartCoroutine(DamageTaken());
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
        OnHealthAdd.Invoke();
    }

    public void Death()
    {
        if (DieEvent!=null)
        {
            DieEvent.Invoke();
        }
        else       
        {
            this.gameObject.SetActive(false);
        } 
    }
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
