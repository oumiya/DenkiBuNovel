using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {
    /// <summary>
    /// シナリオを格納する
    /// </summary>
    public string[] scenarios;
    /// <summary>
    /// uiTextへの参照を持つ
    /// </summary>
    [SerializeField]
    public Text uiText;
    /// <summary>
    /// 1文字の表示にかかる時間
    /// </summary>
    [SerializeField][Range(0.001f, 0.3f)]
    public float intervalForCharacterDisplay = 0.05f;
    /// <summary>
    /// 現在の文字列
    /// </summary>
    string currentText = string.Empty;
    /// <summary>
    /// 表示にかかる時間
    /// </summary>
    float timeUnitDisplay = 0;
    /// <summary>
    /// 文字列の表示を開始した時間
    /// </summary>
    float timeElapsed = 1;
    /// <summary>
    /// 表示中の文字数
    /// </summary>
    int lastUpdateCharacter = -1;
    public bool IsCompleteDisplayText
    {
        get { return Time.time > timeElapsed + timeUnitDisplay; }
    }
    /// <summary>
    /// 強制的に全文表示する
    /// </summary>
    public void ForceCompleteDisplayText()
    {
        timeUnitDisplay = 0;
    }
    /// <summary>
    /// 次に表示する文字列をセットする
    /// </summary>
    public void SetNextLine(string text)
    {
        currentText = text;

        // 想定時間と現在の時刻をキャッシュ
        timeUnitDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;

        // 文字数カウンタを初期化
        lastUpdateCharacter = -1;

    }

    // Update is called once per frame
    void Update () {
        // クリックから経過した時間が想定時間の何%かを確認し、表示文字数を出す
        int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUnitDisplay) * currentText.Length);

        if (displayCharacterCount != lastUpdateCharacter)
        {
            uiText.text = currentText.Substring(0, displayCharacterCount);
            lastUpdateCharacter = displayCharacterCount;
        }
	}
}
