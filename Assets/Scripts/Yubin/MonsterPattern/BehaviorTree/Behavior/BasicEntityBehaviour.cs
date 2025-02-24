using System;
using UnityEngine;

public class BasicEntityBehaviour : MonoBehaviour {
  [Header("기초 능력치")]
  public MonsterStats monsterStats;

  [field: SerializeField] public EntityStats Status { get; set; }
  [field: SerializeField] public int Team { get; set; } = 0;

  public event Action OnHit;
  public event Action OnDead;

  protected virtual void Awake() {
    Status = monsterStats.ToEntityStats();
  }

  public void GetDamage(float damage) {
    Status.health -= damage;

    OnHit?.Invoke();

    if (Status.health <= 0) {
      OnDead?.Invoke();
    }
  }

  public bool IsSameTeam(int team) => Team == team;
  public bool IsSameTeam(BasicEntityBehaviour behaviour) => IsSameTeam(behaviour.Team);
}