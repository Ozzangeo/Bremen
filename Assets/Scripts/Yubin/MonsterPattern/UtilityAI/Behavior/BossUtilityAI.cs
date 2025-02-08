using UnityEngine;

public class BossUtilityAI : MonoBehaviour, IUtilityAI
{
  [Header("보스 상태")]
  public float health = 100f;       // 체력
  public float maxHealth = 100f;    // 최대 체력
  public float attackRange = 8f;    // 공격 범위
  public float fleeDistance = 10f;  // 도망 범위

  [Header("유틸리티 커브")]
  [SerializeField] private AnimationCurve healingCurve; // 체력에 따른 힐링 점수 커브
  [SerializeField] private AnimationCurve attackCurve;  // 거리에 따른 공격 점수 커브
  [SerializeField] private AnimationCurve fleeCurve;    // 체력에 따른 도망 점수 커브

  private Transform player;

  void Start()
  {
    player = GameObject.Find("Player").transform;
  }

  void Update()
  {
    string action = ChooseAction();
    PerformAction(action);
  }

  public string ChooseAction()
  {
    float playerDistance = Vector3.Distance(transform.position, player.position);

    // 점수 계산
    float healingScore = GetHealingScore();
    float attackScore = GetAttackScore(playerDistance);
    float fleeScore = GetFleeScore(playerDistance);
    float chaseScore = GetChaseScore(playerDistance);

    // 체력에 따른 점수 보정
    if(health > maxHealth * 0.5f) { attackScore *= 1.5f; chaseScore *= 1.2f;}
    else { healingScore *= 1.2f; fleeScore *= 1.5f;}

    // 가장 높은 점수를 가진 행동 선택
    if(healingScore > attackScore && healingScore > fleeScore && healingScore > chaseScore) return "Heal";
    if(attackScore > healingScore && attackScore > fleeScore && attackScore > chaseScore) return "Attack";
    if(fleeScore > healingScore && fleeScore > attackScore && fleeScore > chaseScore) return "Flee";
    return "Chase";
  }

  private float GetHealingScore()
  {
    // 체력이 높다면 점수 제한
    if(health >= maxHealth * 0.8f) return 0f;
    return healingCurve.Evaluate(health / maxHealth);
  }

  private float GetAttackScore(float playerDistance)
  {
    // 플레이어와 거리가 멀다면 점수 제한
    if(playerDistance > attackRange) return 0f;
    return attackCurve.Evaluate(playerDistance / attackRange);
  }

  private float GetFleeScore(float playerDistance)
  {
    // 플레이어와 거리가 충분히 멀다면 점수 제한
    if(playerDistance >= fleeDistance) return 0f;
    // 체력이 충분하다면 점수 제한
    if(health > maxHealth * 0.8f) return 0f;
    return fleeCurve.Evaluate(health / maxHealth);
  }

  private float GetChaseScore(float playerDistance)
  {
    // 플레이어와 거리가 가깝다면 점수 제한
    if(playerDistance <= attackRange) return 0f;
    return attackCurve.Evaluate(playerDistance / attackRange);
  }

  public void PerformAction(string action)
  {
    switch(action)
    {
      case "Heal": Heal(); break;
      case "Attack": Attack(); break;
      case "Flee": Flee(); break;
      case "Chase": Chase(); break;
    }
  }

  private void Heal()
  {
    Debug.Log("힐링 중");
    health = Mathf.Min(health + 10f * Time.deltaTime, maxHealth);
  }

  private void Attack()
  {
    Debug.Log("공격 중");
  }

  private void Flee()
  {
    Debug.Log("도망 중");
    Vector3 direction = (transform.position - player.position).normalized;
    transform.position += direction * 3f * Time.deltaTime;
  }

  private void Chase()
  {
    Debug.Log("추적 중");
    transform.position = Vector3.MoveTowards(transform.position, player.position, 5f * Time.deltaTime);
  }
}
