using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    [Header("Objectives")]
    public List<Transform> objectives = new List<Transform>();
    public List<string> objectiveNames = new List<string>();

    int currentIndex = 0;

    [Header("UI")]
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

        if (marker != null)
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

        Debug.Log("Objective: " + objectives[currentIndex].name);
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
            Debug.Log("ALL OBJECTIVES COMPLETE");

            if (objectiveText != null)
            {
                objectiveText.text = "All Objectives Completo!";
            }

            if (marker != null)
            {
                marker.gameObject.SetActive(false);
            }
        }
    }
}
