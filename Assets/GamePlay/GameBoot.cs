using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameInstance.Get().StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        GameInstance.Get().UpdateGame();
    }
}
