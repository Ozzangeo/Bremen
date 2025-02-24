using System.Linq;

namespace Ozi.Weapon {
    public class KeyboardWeapon : BasicWeapon {
        private const float NORMAL_ATTACK_BUFF_RADIUS = 10.0f;
        private const float NORMAL_ATTACK_DEBUFF_RADIUS = 10.0f;
        private const float NORMAL_ATTACK_DAMAGE = 70.0f;
        private const float NORMAL_ATTACK_BUFF_TIME = 7.0f;
        private const float NORMAL_ATTACK_DEBUFF_TIME = 5.0f;

        private const float PLAY_ATTACK_HEAL_RADIUS = 4.0f;

        private const float PLAY_ATTACK_HEAL = 3.0f;

        protected override void NormalAttack() {
            var position = Owner.transform.position;

            // owner included
            var team_entites = 
                FindEntities(position, NORMAL_ATTACK_BUFF_RADIUS)
                .Where(o => o.IsSameTeam(Owner));
            var other_team_entites =
                FindEntities(position, NORMAL_ATTACK_DEBUFF_RADIUS)
                .Where(o => !o.IsSameTeam(Owner));

            // buff team entities logic

            // debuff other team entities logic

            EntitiesHit(other_team_entites, NORMAL_ATTACK_DAMAGE);
        }
        protected override void PlayAttack() {
            var position = Owner.transform.position;
            var entities = FindEntities(position, PLAY_ATTACK_HEAL_RADIUS);

            // owner included
            var team_entites = entities.Where(o => o.IsSameTeam(Owner));

            var combo = ChartPlayer.Combo;

            EntitiesHeal(team_entites, PLAY_ATTACK_HEAL * combo);
        }
        protected override void SpecialAttack() {
            // why not exsit special attack in plan document?
        }
    }
}