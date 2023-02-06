using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    static readonly World instance = new World();
    static WorldStates world;
    static Queue<GameObject> Tourists;
    static Queue<GameObject> AvailableDeer;
    static World()
    {
        world = new WorldStates();
    }
    public WorldStates GetWorld()
    {
        return world;
    }

    private World() {

    }
    public static World Instance
    {
        get
        {
            return instance;
        }
    }
}

