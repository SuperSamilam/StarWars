using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using MixedReality.Toolkit;
using MixedReality.Toolkit.Subsystems;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    [Header("General")]
    public LevelSystem level;

    [SerializeField] GameObject yodaWin;
    [SerializeField] GameObject yodaLoss;
    [SerializeField] GameObject vaderWin;
    [SerializeField] GameObject vaderLoss;

    [SerializeField] AudioSource voiceLines;
    [SerializeField] AudioClip yodaWinAudio;
    [SerializeField] AudioClip yodaLossAudio;
    [SerializeField] AudioClip vaderWinAudio;
    [SerializeField] AudioClip vaderLossAudio;

    List<GameObject> playerPlanets = new List<GameObject>();
    List<GameObject> enemyPlanets = new List<GameObject>();
    List<GameObject> noOwnerPlanets = new List<GameObject>();

    bool completed = false;

    //Go back variels
    float time = 0f;
    bool pinch = false;

    void Start()
    {
        level = FindObjectOfType<LevelKepper>().level;
        LoadLevel();
        Ship.SetGamemanager();
    }

    void LoadLevel()
    {
        bool playerHasPlanet = false;
        bool enemyHasPlanet = false;
        if (level == null)
        {
            Debug.Log("No Level enetred");
            return;
        }
        if (level.planets.Count < 2)
        {
            Debug.Log("Not enough planets");
            return;
        }

        for (int i = 0; i < level.planets.Count; i++)
        {
            if (level.planets[i].owner == Owner.Player)
            {
                playerHasPlanet = true;
            }
            if (level.planets[i].owner == Owner.Enemy)
            {
                enemyHasPlanet = true;
            }
        }

        if (!playerHasPlanet)
        {
            Debug.Log("Player does not have any planets to start with");
            return;
        }
        if (!enemyHasPlanet)
        {
            Debug.Log("Enemy does not have any planets to start with");
            return;
        }

        for (int i = 0; i < level.planets.Count; i++)
        {
            GameObject planet = Instantiate(level.planets[i].planet, level.planets[i].position, Quaternion.identity);
            Planet planetScript = planet.GetComponent<Planet>();
            planetScript.timeBetweenShipSpawn = level.shipSpawnTime;
            if (level.planets[i].owner != Owner.None)
            {
                if (level.planets[i].owner == Owner.Player && level.playingAsJedi)
                {
                    planetScript.owner = Owner.Player;
                    planetScript.shipPrefab = level.jediShip;
                    playerPlanets.Add(planet);
                }
                else if (level.planets[i].owner == Owner.Player && !level.playingAsJedi)
                {
                    planetScript.owner = Owner.Player;
                    planetScript.shipPrefab = level.sithShip;
                    playerPlanets.Add(planet);
                }
                else if (level.playingAsJedi)
                {
                    planetScript.owner = Owner.Enemy;
                    planetScript.shipPrefab = level.sithShip;
                    enemyPlanets.Add(planet);
                }
                else
                {
                    planetScript.owner = Owner.Enemy;
                    planetScript.shipPrefab = level.jediShip;
                    enemyPlanets.Add(planet);
                }
                for (int j = 0; j < level.startShipAmount - 1; j++)
                {
                    planetScript.SpawnShip();
                }
            }
            else
            {
                noOwnerPlanets.Add(planet);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckWinSituation();
        CheckIfPlayerWantsToGoBack();

        for (int i = 0; i < enemyPlanets.Count; i++)
        {
            if (enemyPlanets[i].GetComponent<Planet>().haveAttacked)
            {
                continue;
            }

            if (noOwnerPlanets.Count >= 1)
            {
                enemyPlanets[i].GetComponent<Planet>().haveAttacked = true;
                Attack(enemyPlanets[i], noOwnerPlanets[0]);
                continue;
            }

            for (int j = 0; j < playerPlanets.Count; j++)
            {
                if (enemyPlanets[i].transform.childCount > playerPlanets[j].transform.childCount * 1.5f)
                {
                    enemyPlanets[i].GetComponent<Planet>().haveAttacked = true;
                    Attack(enemyPlanets[i], playerPlanets[j]);
                }
            }
        }
    }

    void CheckWinSituation()
    {
        //Current - wil change to a better end game
        if (enemyPlanets.Count == 0 && !completed)
        {
            Debug.Log("WOn");
            completed = true;
            if (level.playingAsJedi)
            {
                yodaWin.SetActive(true);
                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    voiceLines.clip = yodaWinAudio;
                    voiceLines.Play();
                }
            }
            else
            {
                vaderWin.SetActive(true);
                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    voiceLines.clip = vaderWinAudio;
                    voiceLines.Play();
                }
            }
        }
        else if (playerPlanets.Count == 0 && !completed)
        {
            completed = true;
            if (level.playingAsJedi)
            {
                yodaLoss.SetActive(true);
                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    voiceLines.clip = yodaLossAudio;
                    voiceLines.Play();
                }
            }
            else
            {
                vaderLoss.SetActive(true);
                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    voiceLines.clip = vaderLossAudio;
                    voiceLines.Play();
                }
            }
        }
    }

    void CheckIfPlayerWantsToGoBack()
    {
        if (pinch)
        {
            time += Time.deltaTime;
            if (time >= 3f)
            {
                SceneManager.LoadScene(sceneBuildIndex: 0);
            }
        }
        else
        {
            time = 0;
        }

        pinch = IsPinching(XRNode.LeftHand);
    }

    public static void Attack(GameObject planet, GameObject victim)
    {

        List<GameObject> ships = new List<GameObject>();
        int i = 0;
        foreach (Transform child in planet.transform)
        {
            if (i % 2 == 0)
            {
                ships.Add(child.gameObject);
                child.parent = null;
                Ship ship = child.gameObject.GetComponent<Ship>();
                ship.orbiting = false;
                ship.target = victim.transform;
            }
            i++;
        }
    }

    public static bool IsPinching(XRNode hand)
    {
        var aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        aggregator.TryGetJoint(TrackedHandJoint.IndexTip, hand, out HandJointPose pose);

        aggregator.TryGetPinchProgress(hand, out bool isReadyToPinch, out bool isPinching, out float pinchAmount);
        return isPinching;
    }

    public static bool GetPointerPos(out GameObject hitObj, LayerMask mask)
    {
        hitObj = null;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward * 1000f);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 1f, out hit, 1000000, mask))
        {
            hitObj = hit.transform.gameObject;
            return true;
        }

        return false;
    }

    public void ModifyPlanetOwnership(GameObject planet, Owner owner)
    {
        playerPlanets.Remove(planet);
        noOwnerPlanets.Remove(planet);
        enemyPlanets.Remove(planet);

        if (owner == Owner.Enemy)
        {
            enemyPlanets.Add(planet);
        }
        else if (owner == Owner.Player)
        {
            playerPlanets.Add(planet);
        }
    }

    public bool WhoOwnsPlanet(Owner owner, GameObject planet)
    {
        if (owner == Owner.Player && playerPlanets.Contains(planet))
        {
            return true;
        }

        if (owner == Owner.Enemy && enemyPlanets.Contains(planet))
        {
            return true;
        }

        if (owner == Owner.None && noOwnerPlanets.Contains(planet))
        {
            return true;
        }

        return false;
    }
}
