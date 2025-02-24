using UnityEngine;

namespace Ozi.Weapon.Entity {
    [CreateAssetMenu(fileName = "EntityStatusObject", menuName = "ScriptableObjects/EntityStatusObject", order = 2)]
    public class EntityStatusObject : ScriptableObject {
        [field: SerializeField] public EntityStatus Status { get; private set; }
    }
}