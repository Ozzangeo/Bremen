using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Ozi.ChartPlayer {
    public static class BremenChartAudioLoader {
        private static async Task<AudioClip> LoadAudioClip(string path, AudioType type) {
            using var web_request = UnityWebRequestMultimedia.GetAudioClip(path, type);

            await web_request.SendWebRequest();

            if (web_request.result != UnityWebRequest.Result.Success) {
                return null;
            }

            return DownloadHandlerAudioClip.GetContent(web_request);
        }

        public static AudioClip LoadAudioClip(string path) {
            var task = LoadAudioClip(path, AudioType.MPEG);
            task.Wait();

            if (task.Result == null) {
                task = LoadAudioClip(path, AudioType.OGGVORBIS);

                task.Wait();
            }

            return task.Result;
        }
    }
}