using Ozi.ChartEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ozi.ChartPlayer {
    public class BremenChartPlayer : MonoBehaviour {
        [SerializeField] private AudioSource _source;
        [SerializeField] private List<BremenNote> _notes;

        public void LoadChart(BremenChart chart, string work_space) {
            _source.volume = chart.volume;
            
            _source.clip = BremenChartAudioLoader.LoadAudioClip(Path.Combine(work_space, chart.song_filename));
        }
    }
}