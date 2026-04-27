using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public enum UpgradeType { Speed, Health, Damage, Special, AttackSpeed }

[System.Serializable]
public class UpgradeOption
{
    public string name;
    public string description;
    public UpgradeType type;
    public float multiplier;
}
[System.Serializable]
public class BossUpgradeOption
{
    public string name;
    public string description;
    public UpgradeType type;
    public float multiplier;
}

public class StateManager : MonoBehaviour
{
    public string currentState;
    public List<UpgradeOption> possibleUpgrades;
    public List<BossUpgradeOption> possibleBossUpgrades;

    public Canvas CountdownCanvas;
    public TextMeshProUGUI countdownText;
    public Camera startingCam;
    private float timer = 0;

    public Canvas FightCanvas;
    public TextMeshProUGUI LoopNumberText;
    private float loopNumber = 0;
    public Transform playerSpawn;
    public Transform bossSpawn;
    public Slider playerHealthSlider;
    public Slider bossHealthSlider;
    public Slider dashSlider;
    public TextMeshProUGUI playerHealthText;

    private Health currentBossHealth;
    private Health currentPlayerHealth;

    public Canvas WinCanvas;
    public Button Card1;
    public Button Card2;
    public Button Card3;

    public Canvas LoseCanvas;
    public TextMeshProUGUI loseText;

    public Canvas PauseCanvas;

    public GameObject playerPrefab;
    public GameObject bossPrefab;

    public GameObject victoryParticles;

    public Canvas pauseCanvas;
    public bool isPaused;

    public bool isEnding = false;

    public GameObject tempObjects;

    [SerializeField] public List<string> unknownSpecials = new List<string> {"roar", "above"};
    private HashSet<string> knownSpecials = new HashSet<string> { "ground" };

    private float playerHealthMult = 1f;
    private float playerDamageMult = 1f;
    private float playerSpeedMult = 1f;
    private float playerCDDivider = 1f;

    private float bossHealthMult = 1f;
    private float bossDamageMult = 1f;
    private float bossSpeedMult = 1f;
    private float bossWalkDivider = 1f;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && currentState == "Fight" && isEnding == false)
        {
            TogglePause();
        }

        if (!isPaused)
        {
            if (currentState == null || currentState == "")
            {
                ChangeState("Prep");
            }
            else if (currentState == "Prep")
            {
                timer -= Time.deltaTime;
                countdownText.text = Mathf.CeilToInt(timer).ToString();

                startingCam.transform.position = new Vector3(0f, (200f * timer / 3f) + 10f, 0f);

                if (timer <= 0)
                {
                    ChangeState("Fight");
                }
                return;
            }
            else if (currentState == "Fight")
            {
                if (currentBossHealth != null && currentBossHealth.currentHealth <= 0 && isEnding == false)
                {
                    isEnding = true;
                    StartCoroutine(VictoryWait());
                }

                if (currentPlayerHealth != null && currentPlayerHealth.currentHealth <= 0)
                {
                    ChangeState("Lose");
                }
            }
            else if (currentState == "Win")
            {
                
            }
            else if (currentState == "Lose")
            {

            }
        }
    }

    IEnumerator VictoryWait()
    {
        currentPlayerHealth.currentHealth = 99999;
        if (victoryParticles != null)
        {
            victoryParticles.SetActive(true);
        }
        yield return new WaitForSeconds(3f);
        ChangeState("Win");
        isEnding = false;
    }

    void SetupCards(Button btn, UpgradeOption data, BossUpgradeOption bossData)
    {
        btn.transform.Find("You").GetComponent<TextMeshProUGUI>().text = "You Get:\n" + data.name + "\n" + data.description;
        btn.transform.Find("Boss").GetComponent<TextMeshProUGUI>().text = "Boss Gets:\n" + bossData.name + "\n" + bossData.description;
    }

    void ShuffleUpgrades(List<UpgradeOption> list, List<BossUpgradeOption> bossList)
    {
        if (unknownSpecials.Count <= 0)
        {
            for (int i = 0; i < bossList.Count; i++)
            {
                if (bossList[i].type == UpgradeType.Special)
                {
                    bossList.RemoveAt(i);
                    break;
                }
            }
        }
        
        for (int i = 0; i < list.Count; i++) {
            UpgradeOption temp = list[i];
            int bruh = Random.Range(i, list.Count);
            list[i] = list[bruh];
            list[bruh] = temp;
        }

        for (int i = 0; i < bossList.Count; i++)
        {
            BossUpgradeOption temp2 = bossList[i];
            int bruh = Random.Range(i, bossList.Count);
            bossList[i] = bossList[bruh];
            bossList[bruh] = temp2;
        }

        SetupCards(Card1, list[0], bossList[0]);
        SetupCards(Card2, list[1], bossList[1]);
        SetupCards(Card3, list[2], bossList[2]);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (pauseCanvas != null)
            {
                pauseCanvas.gameObject.SetActive(true);
            }
        } else
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (pauseCanvas != null)
            {
                pauseCanvas.gameObject.SetActive(false);
            }
        }
    }

    public void ChooseUpgrade(int num)
    {
        //Do the upgrade for the player first
        UpgradeOption hold = possibleUpgrades[num];
        if (hold.type == UpgradeType.Speed)
        {
            playerSpeedMult += hold.multiplier;
        }
        else if (hold.type == UpgradeType.Health)
        {
            playerHealthMult += hold.multiplier;
        }
        else if (hold.type == UpgradeType.Damage)
        {
            playerDamageMult += hold.multiplier;
        } else if (hold.type == UpgradeType.AttackSpeed)
        {
            playerCDDivider += hold.multiplier;
        }

            // Do the upgrade for the boss
            BossUpgradeOption asdf = possibleBossUpgrades[num];
        if (asdf.type == UpgradeType.Speed)
        {
            bossSpeedMult += asdf.multiplier;
        }
        else if (asdf.type == UpgradeType.Health)
        {
            bossHealthMult  += asdf.multiplier;
        }
        else if (asdf.type == UpgradeType.Damage)
        {
            bossDamageMult += asdf.multiplier;
        }
        else if (asdf.type == UpgradeType.AttackSpeed)
        {
            bossWalkDivider += asdf.multiplier;
        } else if (asdf.type == UpgradeType.Special)
        {
            int index = Random.Range(0, unknownSpecials.Count);
            string chosen = unknownSpecials[index];
            unknownSpecials.RemoveAt(index);
            knownSpecials.Add(chosen);
        }
    }

    public void ChangeState(string newState)
    {
        currentState = newState;

        if (currentState == "Prep")
        {
            Cleanup();
            startingCam.gameObject.SetActive(true);
            startingCam.transform.position = new Vector3(0f, 200f, 0f);
            timer = 3;
            DisableAllCanvas();
            CountdownCanvas.gameObject.SetActive(true);
            return;
        }
        else if (currentState == "Fight")
        {
            SpawnAll();

            startingCam.gameObject.SetActive(false);
            DisableAllCanvas();
            FightCanvas.gameObject.SetActive(true);
            return;
        }
        else if (currentState == "Win")
        {
            loopNumber += 1;
            LoopNumberText.text = loopNumber.ToString();
            Cleanup();
            startingCam.gameObject.SetActive(true);
            DisableAllCanvas();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ShuffleUpgrades(possibleUpgrades, possibleBossUpgrades);
            WinCanvas.gameObject.SetActive(true);

            return;
        }
        else if (currentState == "Lose")
        {
            startingCam.gameObject.SetActive(true);
            Cleanup();
            DisableAllCanvas();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            LoseCanvas.gameObject.SetActive(true);
            loseText.text = "You have survived " + loopNumber +" loops";
            return;
        }
    }

    private void DisableAllCanvas()
    {
        CountdownCanvas.gameObject.SetActive(false);
        FightCanvas.gameObject.SetActive(false);
        WinCanvas.gameObject.SetActive(false);
        LoseCanvas.gameObject.SetActive(false);
        PauseCanvas.gameObject.SetActive(false);
    }

    private void SpawnAll()
    {
        GameObject playerObject = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
        GameObject bossObject = Instantiate(bossPrefab, bossSpawn.position, bossSpawn.rotation);

        bossObject.transform.parent = tempObjects.transform;
        playerObject.transform.parent = tempObjects.transform;

        currentBossHealth = bossObject.GetComponent<Health>();
        currentBossHealth.healthBar = bossHealthSlider;
        currentBossHealth.maxHealth *= (loopNumber * 0.5f + 1f);
        currentBossHealth.maxHealth *= bossHealthMult;
        currentBossHealth.currentHealth = currentBossHealth.maxHealth;

        BossAI bossAIScript = bossObject.GetComponent<BossAI>();
        bossAIScript.stateManager = this;
        bossAIScript.damageMultiplier = bossDamageMult * (loopNumber * 0.1f + 1f);
        bossAIScript.knownSpecials = knownSpecials;
        bossAIScript.walkDuration /= bossWalkDivider;
        bossAIScript.tempObjects = tempObjects;

        NavMeshAgent bossAgent = bossObject.GetComponent<NavMeshAgent>();
        bossAgent.speed *= bossSpeedMult;

        Transform deathParticles = bossObject.transform.Find("DeathParticles");
        if (deathParticles != null)
        {
            victoryParticles = deathParticles.gameObject;
        }

        currentPlayerHealth = playerObject.GetComponent<Health>();
        currentPlayerHealth.healthBar = playerHealthSlider;
        currentPlayerHealth.healthText = playerHealthText;
        currentPlayerHealth.maxHealth *= playerHealthMult;
        currentPlayerHealth.currentHealth = currentPlayerHealth.maxHealth;

        PlayerMovement currentPlayerDash = playerObject.GetComponent<PlayerMovement>();
        currentPlayerDash.dashSlider = dashSlider;
        currentPlayerDash.speed *= playerSpeedMult;

        MouseLook mouseLookScript = playerObject.GetComponentInChildren<MouseLook>();
        mouseLookScript.stateManager = this;

        PlayerCombat playerCombatScript = playerObject.GetComponentInChildren<PlayerCombat>();
        playerCombatScript.stateManager = this;
        playerCombatScript.tempObjects = tempObjects;
        playerCombatScript.damageMultiplier = playerDamageMult;
        playerCombatScript.fireCooldown /= playerCDDivider;
    }

    public void Cleanup()
    {
        for (int i = tempObjects.transform.childCount - 1; i >=0; i--)
        {
            GameObject.Destroy(tempObjects.transform.GetChild(i).gameObject);
        }
    }
}
