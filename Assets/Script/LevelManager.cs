using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> levels;
    private int currentLevel = 0;
    private WeatherController gameManager;

    private void Start()
    {
        gameManager = GetComponent<WeatherController>();
        enableChildren(levels[currentLevel], true);
        foreach (GameObject level in levels)
        {
            if (level != levels[currentLevel])
                enableChildren(level, false);
        }
    }

    public void enableChildren(GameObject parent, bool value)
    {
        parent.SetActive(value);
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).gameObject.SetActive(value);
        }
    }

   

    public void setObjectsToBlur()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0;i < levels[currentLevel].transform.childCount; i++)
        {
            GameObject child = levels[currentLevel].transform.GetChild(i).gameObject;
            if (child.layer != LayerMask.GetMask("UI") || child.layer != LayerMask.GetMask("BG Blur")) 
                list.Add(child);
        }
        gameManager.objectsToBlur = list;
    }

    public void LoadNextLevel()
    {
        currentLevel++;
        enableChildren(levels[currentLevel-1], false);
        enableChildren(levels[currentLevel], true);
        setObjectsToBlur();
    }


}
