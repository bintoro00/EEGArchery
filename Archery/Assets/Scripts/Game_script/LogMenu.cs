using System.Collections;
using System.Collections.Generic; // Queueのために必要
using UnityEngine;
using UnityEngine.UI;
using System.Text; // StringBuilderのために必要

public class LogMenu : MonoBehaviour
{
    public Text _text;
    // ログを何個まで保持するか
    [SerializeField] int m_MaxLogCount = 20;

    // ログの文字列を入れておくためのQueue
    Queue<string> m_LogMessages = new Queue<string>();

    // ログの文字列を結合するのに使う
    StringBuilder m_StringBuilder = new StringBuilder();
 
    void Start(){
        Application.logMessageReceived += LogReceived;
    }
    void LogReceived(string text, string stackTrace, LogType type){
        m_LogMessages.Enqueue(text);

        // ログの個数が上限を超えていたら、最古のものを削除する
        while(m_LogMessages.Count > m_MaxLogCount){
            m_LogMessages.Dequeue();
        }
    }

    void OnGUI(){
        // StringBuilderの内容をリセット
        m_StringBuilder.Length = 0;

        // ログの文字列を結合する（1個ごとに末尾に改行を追加）
        foreach (string s in m_LogMessages){
            m_StringBuilder.Append(s).Append(System.Environment.NewLine);
        }

        // 画面に表示
        _text.text = m_StringBuilder.ToString();
    }
}
