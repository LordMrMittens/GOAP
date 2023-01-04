using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTargetManager : MonoBehaviour
{

    public static GlobalTargetManager GTM { get; private set; }
    public Dictionary<string, GameObject[]> potentialTargets = new Dictionary<string, GameObject[]>();
    List<string> allTags = new List<string>();

    void Start()
    {
        GTM = this;
        FindAllTaggedObjects();
    }

    private void FindAllTaggedObjects()
    {
        string[] unityTags = UnityEditorInternal.InternalEditorUtility.tags;
        foreach (string availableTags in unityTags)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(availableTags);
            potentialTargets.Add(availableTags, taggedObjects);
        }
    }

}
