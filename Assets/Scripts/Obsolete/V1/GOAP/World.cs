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
        Tourists = new Queue<GameObject>();
        AvailableDeer = new Queue<GameObject>();
       // World.Instance.FindAllDeer();

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
    public void AddTourist(GameObject Tourist){
        Tourists.Enqueue(Tourist);
    }

    public GameObject RemoveTourist(){
        if (Tourists.Count == 0 ){
            return null;
        } else {
            return Tourists.Dequeue();
        }
    }

    public void AddDeer(GameObject Deer){
        AvailableDeer.Enqueue(Deer);
    }

    public GameObject RemoveDeer()
    {
        if (AvailableDeer.Count == 0)
        {
            return null;
        }
        else
        {
            return AvailableDeer.Dequeue();
        }
    }

 

    void FindAllDeer()
    {
        GameObject[] allDeer =  GameObject.FindGameObjectsWithTag("Deer");
        foreach (GameObject deer in allDeer)
        {
            AvailableDeer.Enqueue(deer);
        }
        if (allDeer.Length>0){
            world.ChangeState("AvailableDeer", allDeer.Length);
        }
    }
}

