using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class BossAI : MonoBehaviour
{
    public enum BossState {Walking, Shooting, Special}
    public BossState currentState = BossState.Walking;

    public HashSet<string> knownSpecials = new HashSet<string> {};

    private NavMeshAgent agent;
    private Health health;
    private Transform player;

    public StateManager stateManager;

    public int shotsFired = 0;
    public int shotsBeforeSpecial = 4;
 
    public float walkDuration = 2f;
    public float shootDuration = 1f;
    public float specialDuration = 3f;

    public float damageMultiplier = 1f;


    public AudioClip shootClip;
    public AudioSource SFXPlayer;

    public Animator myAnimator;

    public GameObject roarParticles;
    public AudioClip roarVoiceline;
    public AudioSource voicelinePlayer;

    public AudioClip slamVoiceline;
    public GameObject flatRocketPrefab;

    public AudioClip aboveVoiceline;

    private float stateTimer;

    public Transform firePoint;
    public Transform roarPoint;

    public GameObject rocketPrefab;


    public GameObject tempObjects;

    void Start()
    {
        stateTimer = walkDuration;
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        if (SettingsBridge.Instance != null)
        {
            SFXPlayer.volume = SettingsBridge.Instance.sfxVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentState);
        if (stateManager.isPaused || health.currentHealth <= 0)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;

        stateTimer -= Time.deltaTime;

        switch (currentState)
        {
            case BossState.Walking:
                agent.SetDestination(player.position);

                if (stateTimer <= 0)
                {
                    if (shotsFired >= shotsBeforeSpecial)
                    {
                        if (knownSpecials.Count == 0)
                        {
                            agent.isStopped = false;
                            stateTimer = walkDuration;
                            currentState = BossState.Walking;
                        }
                        else
                        {
                            string randomMove = knownSpecials.ElementAt(Random.Range(0, knownSpecials.Count));

                            if ( randomMove == "roar")
                            {
                                shotsFired = 0;
                                currentState = BossState.Special;
                                StartCoroutine(RoarRoutine());
                                voicelinePlayer.clip = roarVoiceline;
                                voicelinePlayer.Play();
                                stateTimer = specialDuration;
                            } else if (randomMove == "ground")
                            {
                                shotsFired = 0;
                                currentState = BossState.Special;
                                StartCoroutine(JumpropeRoutine());
                                voicelinePlayer.clip = slamVoiceline;
                                voicelinePlayer.Play();
                                stateTimer = specialDuration + 1;
                            } else if (randomMove == "above")
                            {
                                shotsFired = 0;
                                currentState = BossState.Special;
                                StartCoroutine(aboveRoutine());
                                voicelinePlayer.clip = aboveVoiceline;
                                voicelinePlayer.Play();
                                stateTimer = specialDuration;
                            }

                        }
                    }
                    else
                    {
                        shotsFired++;
                        currentState = BossState.Shooting;
                        StartCoroutine(ShootRoutine());
                        stateTimer = shootDuration;
                    }
                }
                break;
            case BossState.Shooting:
                agent.isStopped = true;
                agent.velocity = Vector3.zero;

                Vector3 direction = (player.position - transform.position).normalized;
                direction.y = 0;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
                }

                if (stateTimer <= 0)
                {
                    agent.isStopped = false;
                    stateTimer = walkDuration;
                    currentState = BossState.Walking;
                }
                break;
            case BossState.Special:
                agent.isStopped = true;
                agent.velocity = Vector3.zero;

                Vector3 direction2 = (player.position - transform.position).normalized;
                direction2.y = 0;
                if (direction2 != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction2), Time.deltaTime * 5f);
                }

                if (stateTimer <= 0)
                {
                    agent.isStopped = false;
                    stateTimer = walkDuration;
                    currentState = BossState.Walking;
                }
                break;
        }
    }

    IEnumerator ShootRoutine()
    {
        myAnimator.SetTrigger("doShoot");

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 3; i++)
        {
            if (rocketPrefab != null)
            {
                SFXPlayer.clip = shootClip;
                SFXPlayer.Play();
                firePoint.LookAt(player.position + Vector3.up);
                GameObject rocketObj = Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
                Rocket rocketScript = rocketObj.GetComponent<Rocket>();
                rocketScript.damage *= damageMultiplier;
                rocketObj.transform.parent = tempObjects.transform;
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.4f);
    }

    IEnumerator RoarRoutine()
    {
        myAnimator.SetTrigger("doShout");
        roarParticles.SetActive(true);

        int totalShots = 300;

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < totalShots; i++)
        {
            SFXPlayer.clip = shootClip;
            SFXPlayer.Play();
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            GameObject rocketObj = Instantiate(rocketPrefab, roarPoint.position, randomRotation);
            Rocket rocketScript = rocketObj.GetComponent<Rocket>();
            rocketScript.damage *= damageMultiplier;
            rocketObj.transform.parent = tempObjects.transform;
            yield return new WaitForSeconds(0.001f);
        }

        roarParticles.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator JumpropeRoutine()
    {
        myAnimator.SetTrigger("doGround");

        int totalShots = 90;
        float waves = 3;

        yield return new WaitForSeconds(1f);

        for (int j = 0; j < waves; j++)
        {
            for (int i = 0; i < totalShots; i++)
            {
                SFXPlayer.clip = shootClip;
                SFXPlayer.Play();
                Quaternion chosenRotation = Quaternion.Euler(0, i * 4, 0);
                GameObject flatRocketObj = Instantiate(flatRocketPrefab, roarPoint.position, chosenRotation);
                Rocket rocketScript = flatRocketObj.GetComponent<Rocket>();
                rocketScript.damage *= damageMultiplier;
                flatRocketObj.transform.parent = tempObjects.transform;
            }
            yield return new WaitForSeconds(2.5f / waves);
        }

        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator aboveRoutine()
    {
        myAnimator.SetTrigger("doShout");
        roarParticles.SetActive(true);

        int totalShots = 300;
        float spawnHeight = 100f;
        float spawnRadius = 100f;

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < totalShots; i++)
        {
            SFXPlayer.clip = shootClip;
            SFXPlayer.Play();

            float randomX = Random.Range(-spawnRadius, spawnRadius);
            float randomZ = Random.Range(-spawnRadius, spawnRadius);

            Quaternion dropRotation = Quaternion.Euler(90, 0, 0);

            GameObject rocketObj = Instantiate(rocketPrefab, gameObject.transform.position + new Vector3(randomX, spawnHeight, randomZ), dropRotation);

            Rocket rocketScript = rocketObj.GetComponent<Rocket>();
            rocketScript.damage *= damageMultiplier;
            rocketScript.speed = 30f;
            rocketObj.transform.parent = tempObjects.transform;
            yield return new WaitForSeconds(0.001f);
        }

        roarParticles.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }
}
