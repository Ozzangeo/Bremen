using System.Collections.Generic;

namespace Ozi.Weapon.Utility {
    [System.Serializable]
    public class ElementCooldown<E> {
        public Dictionary<E, float> Cooldowns { get; private set; } = new();
        public List<E> CompletedElement { get; private set; } = new();

        public void Update(float delta_time) {
            foreach (var cooldown in Cooldowns) {
                var element = cooldown.Key;
                var remain_time = cooldown.Value;

                var time = remain_time - delta_time;

                Cooldowns[element] = time;

                if (time <= 0.0f) {
                    CompletedElement.Add(element);
                }
            }

            foreach (var element in CompletedElement) {
                Cooldowns.Remove(element);
            }
        }

        public void AddElements(IEnumerable<E> elements, float cooldown) {
            foreach (var element in elements) {
                Cooldowns[element] = cooldown;
            }
        }

        public bool HasCooldown(E element) => Cooldowns.ContainsKey(element);
    }
}