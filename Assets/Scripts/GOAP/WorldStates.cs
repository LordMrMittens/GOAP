using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class worldState
{
    public string key;
    public int value;
    public bool carryOver = true;
}
public class WorldStates
{
    public Dictionary<string, int> states;
    public WorldStates()
    {
        states = new Dictionary<string, int>();
    }

    public bool HasState(string key)
    {
        return states.ContainsKey(key);
    }

    public void AddState(string key, int value)
    {
        states.Add(key, value);
    }
    public void AddSingleState(string key, int value)
    {
        if (!HasState(key))
        {
            states.Add(key, value);
        }
        else
        {
            return;
        }
    }

    public void ChangeState(string key, int value)
    {
        // If it contains this key
        if (HasState(key)) {

            // Add the value to the state
            states[key] += value;
            // If it's less than zero then remove it
            if (states[key] <= 0) {

                // Call the RemoveState method
                RemoveState(key);
            }
        } else {

            AddState(key, value);
        }
    }

    public void RemoveState(string key){
        if(states.ContainsKey(key)){
            states.Remove(key);
        }
    }
    public Dictionary<string, int> GetAllStates(){
        return states;
    }
}

