using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "Create new Level", order = 1)]
public class LevelSystem : ScriptableObject
{
    public List<PlanetInfo> planets = new List<PlanetInfo>();

    [Range(2,10)]
    public int startShipAmount = 3;

    [Range(10,30)]
    public int shipSpawnTime = 20;

    public bool playingAsJedi = true;
    public GameObject sithShip;
    public GameObject jediShip;

}

[System.Serializable]
public struct PlanetInfo
{
    public GameObject planet;
    public Vector3 position;
    public Owner owner;

    public PlanetInfo(GameObject planet, Vector3 position, Owner owner = Owner.None)
    {
        this.planet = planet;
        this.position = position;
        this.owner = owner;
    }

}

public enum Owner { Player, Enemy, None }
