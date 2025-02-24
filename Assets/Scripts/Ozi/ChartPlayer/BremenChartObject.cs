using Ozi.ChartEditor;
using UnityEngine;

namespace Ozi.ChartPlayer {
    [CreateAssetMenu(fileName = "BremenChart", menuName = "Bremen/ChartObject")]
    public class BremenChartObject : ScriptableObject {
        [field: SerializeField] public TextAsset BremenChartText { get; private set; }
        [field: SerializeField] public AudioClip Clip { get; private set; }

        public BremenChart Chart => JsonUtility.FromJson<BremenChart>(BremenChartText.text);
    }
}