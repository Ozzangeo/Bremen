using UnityEngine;
using Ozi.Weapon.Entity;

public class MonsterAttackPlayer : MonoBehaviour
{
  public float damage = 0f;

  public void Initialize(float damage)
  {
    this.damage = damage;
  }

  BasicEntityBehaviour basicEntityBehaviour;

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      basicEntityBehaviour = other.GetComponent<BasicEntityBehaviour>();
      basicEntityBehaviour.Hit(damage);
    }
  }
}
