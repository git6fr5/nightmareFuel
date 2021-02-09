using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {LevelSelect}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public Image levelImage;
    public Text levelName;
    public Text levelDescriptor;

    public Sprite[] levelSprites;

    /* --- Internal Variables --- */
    private string[] levels = new string[] { "ZombieScene", "PinkEyeScene", "ClownScene", "BeeScene" };
    private string[] levelNames = new string[] { "Bob", "Steve", "Jack", "Jenny" };
    private string[] levelDescriptions = new string[] { "Zombies and Bad Breath", "Pink Eye and Electrocution", "Clowns and Fire", "Bees and Whales(?)" };

    public int levelIndex = 0;
    public string selectedLevel;



    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        selectedLevel = levels[levelIndex];
        levelImage.sprite = levelSprites[levelIndex];
    }

    public void SelectLevel(int index)
    {
        levelIndex = index % levels.Length;
        if (levelIndex == -1) { levelIndex = levels.Length - 1; }
        selectedLevel = levels[levelIndex];
        levelImage.sprite = levelSprites[levelIndex];
        levelName.text = levelNames[levelIndex];
        levelDescriptor.text = "Terrified of " + levelDescriptions[levelIndex];
    }
}
