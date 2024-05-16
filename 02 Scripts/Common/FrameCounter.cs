using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;

public class FrameCounter : MonoBehaviour
{
    [SerializeField]
    bool isShow = false;
    [SerializeField]
    int fontSize;

    ProfilerRecorder drawCallsRecorder;
    ProfilerRecorder setPassCallsRecorder;
    ProfilerRecorder totalMemoryRecorder;
    ProfilerRecorder systemMemoryRecorder;
    ProfilerRecorder gcMemoryRecorder;

    float deltaTime;

    void Start() {
        Application.targetFrameRate = 60;
    }

    void Update() {
        this.deltaTime = (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        if (Input.GetKeyDown(KeyCode.F1)) {
            this.isShow = !this.isShow;
        }
    }
    void OnEnable() {
        this.drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
        this.setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
        this.totalMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
        this.systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        this.gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
    }
    void OnDisable() {
        this.drawCallsRecorder.Dispose();
        this.setPassCallsRecorder.Dispose();
        this.totalMemoryRecorder.Dispose();
        this.systemMemoryRecorder.Dispose();
        this.gcMemoryRecorder.Dispose();
    }

    void OnGUI() {
        if (isShow) {
            StringBuilder sb = new StringBuilder(500);
            if (this.drawCallsRecorder.Valid) {
                sb.AppendLine($"Draw Calls: {this.drawCallsRecorder.LastValue}");
            }
            if (this.setPassCallsRecorder.Valid) {
                sb.AppendLine($"SetPass Calls: {this.setPassCallsRecorder.LastValue}");
            }
            sb.AppendLine($"Total Used Memory: {this.totalMemoryRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"System Used Memory: {this.systemMemoryRecorder.LastValue / (1024 * 1024)} MB");
            sb.AppendLine($"GC Reserved Memory: {this.gcMemoryRecorder.LastValue / (1024 * 1024)} MB");
            float fps = 1.0f / Time.deltaTime;
            float ms = Time.deltaTime * 1000.0f;    
            sb.AppendLine(string.Format("{0:N1} FPS ({1:N1}ms)", fps, ms));

            float widthRatio = Screen.width / 1080f;
            float heightRatio = Screen.height / 1920f;
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = Mathf.RoundToInt(this.fontSize * widthRatio);
            style.normal.textColor = Color.white;
            Rect rect = new Rect(50 * widthRatio, 50 * heightRatio, 500 * widthRatio, 500 * heightRatio);
            GUI.Label(rect, sb.ToString(), style);
        }
    }
}
