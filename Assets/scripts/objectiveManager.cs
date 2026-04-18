using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    public List<Transform> objectives = new List<Transform>();
    int currentIndex = 0;

    objectiveMarker marker;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        marker = FindObjectOfType<objectiveMarker>();
        SetObjective(0);
    }

    public void SetObjective(int index)
    {
        if (index >= objectives.Count) return;

        currentIndex = index;

        if (marker != null)
        {
            marker.target = objectives[currentIndex];
        }
    }

    public void CompleteObjective()
    {
        currentIndex++;

        if (currentIndex < objectives.Count)
        {
            SetObjective(currentIndex);
        }
        else
        {
            Debug.Log("ALL OBJECTIVES COMPLETE 🎉");

            if (marker != null)
            {
                marker.gameObject.SetActive(false);
            }
        }
    }
}
