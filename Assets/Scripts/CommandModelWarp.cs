using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラを瞬間移動させて切り替える
/// </summary>
public class CommandModelWarp : ICommand {

    public string Tag
    {
        get
        {
            return "model_warp";
        }
    }

    public void Command(Dictionary<string, string> command)
    {
        var name = command["name"];
        var x = command["x"];
        var y = command["y"];
        var z = command["z"];

        float fx = float.Parse(x);
        float fy = float.Parse(y);
        float fz = float.Parse(z);

        GameObject model = GameObject.Find(name);
        model.transform.position = new Vector3(fx, fy, fz);
    }
}
