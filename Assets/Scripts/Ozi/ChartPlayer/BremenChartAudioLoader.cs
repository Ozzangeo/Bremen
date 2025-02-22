using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Ozi.ChartPlayer {
    public static class BremenChartAudioLoader {
        public static readonly AudioType[] ReadableAudioTypes = new[] {
            AudioType.MPEG,
            AudioType.OGGVORBIS,
        };

        public static AudioClip LoadAudioClip(string path) {
            var timeout = TimeSpan.FromSeconds(5.0f / ReadableAudioTypes.Length);

            foreach (var readable in ReadableAudioTypes) {
                using var web_request = UnityWebRequestMultimedia.GetAudioClip(path, readable);

                var start = DateTime.Now;
                
                web_request.SendWebRequest();

                while (!web_request.isDone
                    && DateTime.Now - start < timeout) { }

                if (web_request.result == UnityWebRequest.Result.Success) {
                    Debug.Log($"Succeed Audio Convert {{ Type: {readable} }}");
                    
                    return DownloadHandlerAudioClip.GetContent(web_request);
                }
            }

            Debug.Log($"Failed Audio Convert {{ Path: {path} }}");

            return null;
        }
    }
}