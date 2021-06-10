using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{

    /* --- Components --- */
    public Text levelName;
    public Text levelDescriptor;

    /* --- Internal Variables --- */
    public Level[] levels;
    public int levelIndex = 0;
    public Level selectedLevel;



    /*--- Unity Methods ---*/
    void Start()
    {
        SelectLevel(0);
    }

    public void SelectLevel(int index)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].gameObject.SetActive(false);
        }

        levelIndex = index % levels.Length;
        if (levelIndex == -1) { levelIndex = levels.Length - 1; }
        selectedLevel = levels[levelIndex];
        selectedLevel.gameObject.SetActive(true);
        levelName.text = selectedLevel.levelName;
        levelDescriptor.text = selectedLevel.levelDescription;
    }

}
