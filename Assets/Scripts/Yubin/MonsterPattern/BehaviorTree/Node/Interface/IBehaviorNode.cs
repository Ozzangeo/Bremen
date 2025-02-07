using System;

public interface IBehaviorNode
{
  // 행동 노드 상태
  public enum EBehaviorNodeState { Running, Success, Failure }

  // 노드 상태 반환
  public EBehaviorNodeState Evaluate();
}