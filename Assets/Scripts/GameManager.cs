using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    public TowerButton ClickedButton { get; set; }

    [SerializeField] private int currency;
    [SerializeField] public int wave = 0;
    [SerializeField] private int lives = 0;

    private bool gameOver = false;

    [SerializeField] private TMP_Text waveText;

    [SerializeField] private TMP_Text currencyText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text sellText;

    [SerializeField] private GameObject waveButton;
    [SerializeField] private GameObject fireTowerButton;
    [SerializeField] private GameObject iceTowerButton;
    [SerializeField] private GameObject stormTowerButton;
    [SerializeField] private GameObject poisonTowerButton;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject upgradePanel;

    private Tower selectedTower;

    private List<Monster> activeMonsters = new List<Monster>();

    public ObjectPool Pool { get; set; }

    public bool WaveActive
    {
        get
        {
            return activeMonsters.Count > 0;
        }
    }

    public int Currency
    {
        get
        {
            return currency;
        }
        set
        {
            this.currency = value;
            this.currencyText.text = "$" + value.ToString();
        }
    }

    public int Lives
    {
        get
        {
            return lives;
        }

        set
        {
            this.lives = value;

            if (lives <= 0)
            {
                this.lives = 0;
                GameOver();
            }

            this.livesText.text = lives.ToString();
        }
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }


    // Start is called before the first frame update
    void Start()
    {
        this.currencyText.text = "$" + currency.ToString();
        this.livesText.text = lives.ToString();
        waveText.text = wave.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();

    }

    public void PickTower(TowerButton towerButton)
    {
        // if (Currency >= towerButton.Price && !WaveActive)
        if (Currency >= towerButton.Price)
        {
            this.ClickedButton = towerButton;
            Hover.Instance.Activate(towerButton.Sprite);
        }
    }

    public void BuyTower()
    {
        if (Currency >= ClickedButton.Price)
        {
            Currency -= ClickedButton.Price;
            Hover.Instance.Deactivate();
        }
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }

    public void StartWave()
    {
        wave++;
        waveText.text = wave.ToString();
        StartCoroutine(SpawnWave());

        DisableButtons();
    }

    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath();

        if (wave % 10 == 0)
        {

            string type = "BossMonster";

            Monster monster = Pool.GetObject(type).GetComponent<Monster>();
            monster.Spawn();

            activeMonsters.Add(monster);

            yield return new WaitForSeconds(2.5f);

        }
        else
        {
            for (int i = 0; i < wave; i++)
            {
                int monsterIndex = Random.Range(0, 4);
                string type = string.Empty;

                switch (monsterIndex)
                {
                    case 0:
                        type = "BlueMonster";
                        break;
                    case 1:
                        type = "RedMonster";
                        break;
                    case 2:
                        type = "GreenMonster";
                        break;
                    case 3:
                        type = "PurpleMonster";
                        break;
                    case 4:
                        type = "BossMonster";
                        break;
                }

                Monster monster = Pool.GetObject(type).GetComponent<Monster>();
                monster.Spawn();

                activeMonsters.Add(monster);

                yield return new WaitForSeconds(2.5f);
            }
        }
    }

    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);

        if (!WaveActive && !gameOver)
        {
            EnableButtons();
        }

    }

    public void DisableButtons()
    {
        waveButton.SetActive(false);
        // fireTowerButton.SetActive(false);
        // iceTowerButton.SetActive(false);
        // stormTowerButton.SetActive(false);
        // poisonTowerButton.SetActive(false);
    }

    public void EnableButtons()
    {
        waveButton.SetActive(true);
        // fireTowerButton.SetActive(true);
        // iceTowerButton.SetActive(true);
        // stormTowerButton.SetActive(true);
        // poisonTowerButton.SetActive(true);
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);

        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectTower(Tower tower)
    {

        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = tower;
        selectedTower.Select();

        sellText.text = "+ $" + (selectedTower.Price / 2).ToString();

        upgradePanel.SetActive(true);
    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        upgradePanel.SetActive(false);

        selectedTower = null;
    }

    public void SellTower()
    {
        if (selectedTower != null)
        {
            Currency += selectedTower.Price / 2;
            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;

            Destroy(selectedTower.transform.parent.gameObject);

            DeselectTower();
        }
    }


}
