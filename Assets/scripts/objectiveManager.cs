using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    public List<Transform> objectives = new List<Transform>();
    public List<string> objectiveNames = new List<string>();

    int currentIndex = 0;

    public TextMeshProUGUI objectiveText;

    objectiveMarker marker;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        marker = FindObjectOfType<objectiveMarker>();

        if (objectives.Count > 0)
        {
            SetObjective(0);
        }
    }

    public void SetObjective(int index)
    {
        if (index >= objectives.Count) return;

        currentIndex = index;

        if (marker != null && currentIndex < objectives.Count)
        {
            marker.target = objectives[currentIndex];
        }

        if (objectiveText != null)
        {
            if (currentIndex < objectiveNames.Count)
                objectiveText.text = objectiveNames[currentIndex];
            else
                objectiveText.text = "Objective Updated";
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
            if (objectiveText != null)
            {
                objectiveText.text = "";
            }

            if (marker != null)
            {
                marker.gameObject.SetActive(false);
            }
        }
    }
}