using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTargetManager : MonoBehaviour
{

    public static GlobalTargetManager GTM { get; private set; }
    public Dictionary<string, List<Target>> potentialTargets = new Dictionary<string, List<Target>>();
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
            List<Target> taggedTargets =  new List<Target>();
            foreach (GameObject taggedObject in taggedObjects)
            {
                Target taggedTarget = taggedObject.GetComponent<Target>();
                if(taggedTarget != null){
                    taggedTargets.Add(taggedTarget);
                } else {
                    Debug.LogError($"Potential target has no Target component : {taggedObject.name}");
                }
            }
            potentialTargets.Add(availableTags, taggedTargets);
        }
    }

}
