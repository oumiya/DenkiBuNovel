using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlag : MonoBehaviour {

    private bool[] gameFlags;

    /// <summary>
    /// ゲームフラグを取得する
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public bool GetGameFlag(int i)
    {
        return gameFlags[i];
    }

    /// <summary>
    /// ゲームフラグを設定する
    /// </summary>
    /// <param name="i"></param>
    /// <param name="value"></param>
    public void SetGameFlag(int i, bool value)
    {
        gameFlags[i] = value;
    }

	// Use this for initialization
	void Start () {
        gameFlags = new bool[500];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
