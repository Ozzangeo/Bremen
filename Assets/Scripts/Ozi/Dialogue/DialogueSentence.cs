﻿using UnityEngine;

namespace Ozi.Dialogue {
    [System.Serializable]
    public class DialogueSentence {
        [field: TextArea]
        [field: SerializeField] public string Text { get; private set; }
        [field: SerializeField] public float Delay { get; private set; } = 0.0f;
    }
}