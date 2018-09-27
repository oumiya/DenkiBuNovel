using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

/// <summary>
/// ゲームフラグコマンド。指定の番号のゲームフラグをtrueにしたりfalseにしたりする
/// @flag number=0 value=true
/// 上記のようなコマンドで使う
/// </summary>
public class CommandGameFlag : ICommand {
    public string Tag
    {
        get
        {
            return "flag";
        }
    }

    public void Command(Dictionary<string, string> command)
    {
        var number = command["number"];
        var value = command["value"];

        ScenarioManager.Instance.gameFlags.GetComponent<GameFlag>().SetGameFlag(int.Parse(number), bool.Parse(value));
    }
}
