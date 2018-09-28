using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラを瞬間移動させて切り替える
/// </summary>
public class CommandCameraSwitch : ICommand {

    public string Tag
    {
        get
        {
            return "camera_switch";
        }
    }

    public void Command(Dictionary<string, string> command)
    {
        var x = command["x"];
        var y = command["y"];
        var z = command["z"];

        float fx = float.Parse(x);
        float fy = float.Parse(y);
        float fz = float.Parse(z);

        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        mainCamera.transform.position = new Vector3(fx, fy, fz);
    }
}
