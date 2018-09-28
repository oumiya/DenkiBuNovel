using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// コマンド制御
public class CommandController : SingletonMonoBehaviourFast<CommandController> {
    // 文字を解析しながら呼び出すコマンド
    private readonly List<ICommand> m_commandList = new List<ICommand>()
    {
        new CommandUpdateImage(),	// name=オブジェクト名 image=イメージ名 
		new CommandJumpNextScenario(),	// fileName=シナリオ名
        new CommandGameFlag(),     // number=添字 value=true/false
        new CommandCameraSwitch(), // x=X座標, y=Y座標, z=Z座標
        new CommandModelWarp(),    // name=GameObjectのname x=X座標 y=Y座標 z=Z座標
        new CommandModelEuler(),   // name=GameObjectのname x=X角度 y=Y角度 z=Z角度
	};

    // 文字の表示が完了したタイミングで呼ばれる処理
    private List<IPreCommand> m_preCommandList = new List<IPreCommand>();

    public void PreloadCommand(string line)
    {
        var dic = CommandAnalytics(line);
        foreach (var command in m_preCommandList)
            if (command.Tag == dic["tag"])
                command.PreCommand(dic);
    }

    public bool LoadCommand(string line)
    {
        var dic = CommandAnalytics(line);
        foreach (var command in m_commandList)
        {
            if (command.Tag == dic["tag"])
            {
                command.Command(dic);
                return true;
            }
        }
        return false;
    }

    // コマンドを解析
    private Dictionary<string, string> CommandAnalytics(string line)
    {
        Dictionary<string, string> command = new Dictionary<string, string>();
        // コマンド名を取得
        var tag = Regex.Match(line, "@(\\S+)\\s");
        command.Add("tag", tag.Groups[1].ToString());

        // コマンドのパラメータを取得
        Regex regex = new Regex("(\\S+)=(\\S+)");
        var matches = regex.Matches(line);
        foreach (Match match in matches)
        {
            if (!command.ContainsKey(match.Groups[1].ToString()))
            {
                command.Add(match.Groups[1].ToString(), match.Groups[2].ToString());
            }
            else
            {
                // もし引数が重複していたら引数の値を上書きする
                command[match.Groups[1].ToString()] = match.Groups[2].ToString();
            }

        }

        return command;
    }

    #region UNITY_CALLBACK

    new void Awake()
    {
        base.Awake();

        // PreCommandを取得
        foreach (var command in m_commandList)
        {
            if (command is IPreCommand)
            {
                m_preCommandList.Add((IPreCommand)command);
            }
        }
    }

    #endregion
}