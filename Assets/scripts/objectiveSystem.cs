using UnityEngine;
using TMPro;

public class objectiveSystem : MonoBehaviour
{

    public static objectiveSystem instance;

    [SerializeField] TextMeshProUGUI objectiveText;

    int currentObjective;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        updateObjective();
    }

    public void nextObjective()
    {
        currentObjective++;
        updateObjective();
    }

    void updateObjective()
    {
        if (currentObjective == 0)
            objectiveText.text = "Go to the first puzzle";

        else if (currentObjective == 1)
            objectiveText.text = "Go to the second puzzle";

        else if (currentObjective == 2)
            objectiveText.text = "Find the final puzzle";

        else if (currentObjective == 3)
            objectiveText.text = "Defeat the boss";

        else if (currentObjective >= 4)
            objectiveText.text = "Escape the maze";
    }

}