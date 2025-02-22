using UnityEngine;
using System;

// 실제로 행동하는 노드
public sealed class ActionNode : IBehaviorNode
{
  Func<IBehaviorNode.EBehaviorNodeState> onUpdate = null;

  public ActionNode(Func<IBehaviorNode.EBehaviorNodeState> onUpdate) => this.onUpdate = onUpdate;

  // onUpdate가 NULL이 아닐때 실행, NULL 이라면 Failure 반환
  public IBehaviorNode.EBehaviorNodeState Evaluate() => onUpdate?.Invoke() ?? IBehaviorNode.EBehaviorNodeState.Failure;
}
