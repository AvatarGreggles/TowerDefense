using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    [SerializeField] private float speed = 1;

    private Stack<Node> path;

    [SerializeField] private Element elementType;

    public Point GridPosition { get; set; }

    public Vector3 destination;

    public bool IsActive { get; set; }

    [SerializeField] private Stat health;

    private List<Debuff> debuffs = new List<Debuff>();

    [SerializeField] int maxHealth;

    private SpriteRenderer spriteRenderer;

    private int invulnerability = 2;

    [SerializeField] private int currencyProvided = 1;

    public bool Alive
    {
        get
        {
            return health.CurrentValue > 0;
        }

    }

    public Element ElementType
    {
        get
        {
            return elementType;
        }
    }

    private Animator animator;

    private List<Debuff> debuffsToRemove = new List<Debuff>();
    private List<Debuff> newDebuffs = new List<Debuff>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health.Initialize();

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleDebuffs();
        Move();
    }

    public void Spawn()
    {
        transform.position = LevelManager.Instance.RedPortal.transform.position;
        this.health.Bar.Reset();


        if (GameManager.Instance.wave % 3 == 0)
        {
            health.CurrentValue = Mathf.FloorToInt(health.CurrentValue * (GameManager.Instance.wave * 1.5f));
            currencyProvided = Mathf.FloorToInt(currencyProvided);
            speed += 1f;
        }

        this.health.MaxVal = maxHealth;
        this.health.CurrentValue = this.health.MaxVal;

        LevelManager.Instance.RedPortal.Open();
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1), false));

        SetPath(LevelManager.Instance.Path);
    }

    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {

        float progress = 0;
        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime;
            yield return null;
        }

        transform.localScale = to;

        IsActive = true;

        if (remove)
        {
            Release();
        }
    }

    private void Move()
    {
        if (IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (transform.position == destination)
            {
                if (path != null && path.Count > 0)
                {
                    Animate(GridPosition, path.Peek().GridPosition);

                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        }

    }

    private void SetPath(Stack<Node> newPath)
    {
        if (newPath != null)
        {
            this.path = newPath;

            Animate(GridPosition, path.Peek().GridPosition);

            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }


    }

    // Need to actually implement animated monsters
    private void Animate(Point currentPos, Point newPos)
    {
        if (currentPos.y > newPos.y)
        {
            // We are moving down
            animator.SetFloat("MoveY", 1);
            animator.SetFloat("MoveX", 0);

        }
        else if (currentPos.y < newPos.y)
        {
            // We are moving up
            animator.SetFloat("MoveY", -1);
            animator.SetFloat("MoveX", 0);
        }

        if (currentPos.y == newPos.y)
        {
            if (currentPos.x > newPos.x)
            {
                // We are moving left
                animator.SetFloat("MoveX", -1);
                animator.SetFloat("MoveY", 0);
            }
            else if (currentPos.x < newPos.x)
            {
                // We are moving right
                animator.SetFloat("MoveX", 1);
                animator.SetFloat("MoveY", 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BluePortal")
        {
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));
            other.GetComponent<Portal>().Open();

            GameManager.Instance.Lives--;
        }

        if (other.tag == "Tile")
        {
            spriteRenderer.sortingOrder = other.GetComponent<TileScript>().GridPosition.y;

        }

    }

    public void Release()
    {
        IsActive = false;
        GridPosition = LevelManager.Instance.RedSpawn;
        GameManager.Instance.RemoveMonster(this);
        Debug.Log("Monster Released");

        GameManager.Instance.Pool.ReleaseObject(gameObject);


    }

    public void TakeDamage(float damage, Element dmgSource)
    {
        if (IsActive)
        {
            if (dmgSource == elementType)
            {
                damage = damage / invulnerability;
                invulnerability++;
            }
            health.CurrentValue -= damage;
        }

        if (health.CurrentValue <= 0)
        {
            GameManager.Instance.Currency += currencyProvided;
            animator.SetTrigger("Die");
            IsActive = false;

            GetComponent<SpriteRenderer>().sortingOrder--;
        }
    }

    public void AddDebuff(Debuff debuff)
    {
        if (!debuffs.Exists(x => x.GetType() == debuff.GetType()))
        {
            newDebuffs.Add(debuff);
        }
    }

    public void RemoveDebuff(Debuff debuff)
    {
        debuffsToRemove.Add(debuff);

        // debuffs.Remove(debuff);

    }

    private void HandleDebuffs()
    {
        if (newDebuffs.Count > 0)
        {
            debuffs.AddRange(newDebuffs);
            newDebuffs.Clear();
        }

        foreach (Debuff debuff in debuffsToRemove)
        {
            debuffs.Remove(debuff);
        }

        debuffsToRemove.Clear();

        foreach (Debuff debuff in debuffs)
        {
            debuff.Update();
        }
    }
}
