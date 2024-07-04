using UnityEngine;
using UnityEngine.UI;

public class UIRedDotForm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUI.skin.button.fontSize = 30;

        if (GUILayout.Button("显示A/B/C/D", GUILayout.Width(200), GUILayout.Height(80)))
        {
            GameEntry.RedDot.Set("A/B/C/D", true);
        }

        if (GUILayout.Button("隐藏A/B/C/D", GUILayout.Width(200), GUILayout.Height(80)))
        {
            GameEntry.RedDot.Set("A/B/C/D", false);
        }

        if (GUILayout.Button("显示A/B/C", GUILayout.Width(200), GUILayout.Height(80)))
        {
            GameEntry.RedDot.Set("A/B/C", true);
        }

        if (GUILayout.Button("隐藏A/B/C", GUILayout.Width(200), GUILayout.Height(80)))
        {
            GameEntry.RedDot.Set("A/B/C", false);
        }
    }
}
