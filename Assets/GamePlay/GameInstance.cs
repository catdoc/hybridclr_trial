using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance
{
    static GameInstance _Inst = null;
    public static GameInstance Get() 
    {
        if (_Inst == null) 
        {
            _Inst = new GameInstance();
        }
        return _Inst;
    }

    public void StartGame() 
    {
        Debug.Log("StartGame ... ");
    }

    public void UpdateGame()
    {
        Debug.Log("Update WX Game ... ");
    }
}
