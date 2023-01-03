using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{

    public Text states;
    public TestCharacter person;

    void LateUpdate() {

        // Dictionary of states
        Dictionary<string, int> worldStates = World.Instance.GetWorld().GetAllStates();
        Dictionary<string, int> BeliefStates = person.beliefs.GetAllStates();
        // Clear out the states text

        states.text = "";

        // Cycle through them all and store in states.text
        foreach (KeyValuePair<string, int> s in BeliefStates)
        {

            states.text += s.Key + ", " + s.Value + "\n";
        }
    }
}
