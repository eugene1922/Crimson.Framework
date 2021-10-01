using System;
using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour {

    string label = "";
    float count;
    private GUIStyle style = new GUIStyle();

    IEnumerator Start ()
    {
        GUI.depth = 2;
        style.font = Font.CreateDynamicFontFromOSFont("Mono", 30);
        style.normal.textColor = Color.white;
        
        while (true) {
            if (Time.timeScale > 0) {
                yield return new WaitForSeconds (0.05f);
                count = (1 / Time.deltaTime);
                label = $"FPS:   {Mathf.Round (count)} \n" +
                        $"Delta: {(Time.deltaTime*1000):N}";
            } else {
                label = "Pause";
            }
            yield return new WaitForSeconds (0.5f);
        }
    }

    private void Update()
    {
        FrameTimingManager.CaptureFrameTimings();
    }

    void OnGUI ()
    {
        GUI.Label (new Rect (50, 40, 100, 25), label, style);
    }
}