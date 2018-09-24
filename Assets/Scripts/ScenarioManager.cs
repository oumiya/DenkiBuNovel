using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

[RequireComponent(typeof(TextController))]
public class ScenarioManager : SingletonMonoBehaviourFast<ScenarioManager> {

    public string LoadFileName;

    private string[] m_scenarios;
    private int m_currentLine = 0;
    private bool m_isCallPreload = false;

    private TextController m_textController;
    private CommandController m_commandController;

    void RequestNextLine()
    {
        var currentText = m_scenarios[m_currentLine];

        m_textController.SetNextLine(CommandProcess(currentText));
        m_currentLine++;
        m_isCallPreload = false;
    }

    public void UpdateLines(string fileName)
    {
        string scenarioText = ReadFile(fileName);
        if (scenarioText == null)
        {
            Debug.LogError("シナリオファイルが見つかりませんでした");
            Debug.LogError("ScenarioManagerを無効化します");
            enabled = false;
            return;
        }
        m_scenarios = scenarioText.Split(new string[] { "@br" }, System.StringSplitOptions.None);
        m_currentLine = 0;
    }

    /// <summary>
    /// シナリオファイルの読み込み
    /// </summary>
    private string ReadFile(string fileName)
    {
        string scenarioText = string.Empty;

        FileInfo fi = new FileInfo(Application.streamingAssetsPath + "/Scenario/" + fileName);

        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                scenarioText = sr.ReadToEnd();
            }
        }
        catch(System.Exception e)
        {
            scenarioText = null;
        }

        return scenarioText;
    }

    private string CommandProcess(string line)
    {
        var lineReader = new StringReader(line);
        var lineBuilder = new StringBuilder();
        var text = string.Empty;
        while ((text = lineReader.ReadLine()) != null)
        {
            var commentCharacterCount = text.IndexOf("//");
            if (commentCharacterCount != -1)
            {
                text = text.Substring(0, commentCharacterCount);
            }

            if (!string.IsNullOrEmpty(text))
            {
                if (text[0] == '@' && m_commandController.LoadCommand(text))
                    continue;
                lineBuilder.AppendLine(text);
            }
        }

        return lineBuilder.ToString();
    }

    #region UNITY_CALLBACK

    void Start()
    {
        m_textController = GetComponent<TextController>();
        m_commandController = GetComponent<CommandController>();

        UpdateLines(LoadFileName);
        RequestNextLine();
    }

    void Update()
    {
        if (m_textController.IsCompleteDisplayText)
        {
            if (m_currentLine < m_scenarios.Length)
            {
                if (!m_isCallPreload)
                {
                    m_commandController.PreloadCommand(m_scenarios[m_currentLine]);
                    m_isCallPreload = true;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    RequestNextLine();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_textController.ForceCompleteDisplayText();
            }
        }
    }

    #endregion
}