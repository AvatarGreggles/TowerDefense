using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element { Fire, Storm, Poison, Ice }
public abstract class Tower : MonoBehaviour
{
    [SerializeField] private string projectileType;
    [SerializeField] private float projectileSpeed;

    [SerializeField] private int damage;

    [SerializeField] private float debuffDuration;

    [SerializeField] private float proc;

    public float Proc { get => proc; set => proc = value; }

    public float DebuffDuration
    {
        get
        {
            return debuffDuration;
        }

        set
        {
            this.debuffDuration = value;
        }
    }

    public int Price { get; set; }

    public Element ElementType { get; protected set; }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public float ProjectileSpeed
    {
        get
        {
            return projectileSpeed;
        }
    }

    private SpriteRenderer spriteRenderer;
    private Monster target;

    public Monster Target
    {
        get
        {
            return target;
        }
    }

    private Queue<Monster> monsters = new Queue<Monster>();

    private bool canAttack = true;

    private float attackTimer;
    [SerializeField] private float attackCooldown = 1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    public void Select()
    {
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    private void Attack()
    {

        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }

        if (target == null && monsters.Count > 0)
        {
            target = monsters.Dequeue();
        }

        if (target != null && target.IsActive)
        {
            if (canAttack)
            {
                Shoot();
                canAttack = false;
            }
        }
        else if (monsters.Count > 0)
        {
            target = monsters.Dequeue();
        }

        if (target != null && !target.Alive)
        {
            target = null;
        }


    }

    private void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;

        projectile.Initialize(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            monsters.Enqueue(other.GetComponent<Monster>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Monster")
        {
            target = null;
        }
    }

    public abstract Debuff GetDebuff();
}
