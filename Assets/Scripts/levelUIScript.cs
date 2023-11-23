using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelUIScript : MonoBehaviour
{
    public int levelID;

    public void selectCurrentLevel() 
    {
        FindObjectOfType<mainMenu>().changeSelectedLevel(levelID);
    }
}
