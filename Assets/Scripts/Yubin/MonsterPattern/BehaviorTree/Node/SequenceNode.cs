using UnityEngine;
using System.Collections.Generic;

// 자식 노드중 Failure상태를 가진 노드를 발견할 때 까지 진행
public sealed class SequenceNode : IBehaviorNode
{
  List<IBehaviorNode> childNodes;

  public SequenceNode(List<IBehaviorNode> childNodes) => this.childNodes = childNodes;

  // Success라면 다음 자식으로 이동, 아니라면 자식 노드의 상태 반환
  public IBehaviorNode.EBehaviorNodeState Evaluate()
  {
    if(childNodes == null) return IBehaviorNode.EBehaviorNodeState.Failure;

    foreach(var child in childNodes)
    {
      switch(child.Evaluate())
      {
        case IBehaviorNode.EBehaviorNodeState.Running: return IBehaviorNode.EBehaviorNodeState.Running;
        case IBehaviorNode.EBehaviorNodeState.Failure: return IBehaviorNode.EBehaviorNodeState.Failure;
        case IBehaviorNode.EBehaviorNodeState.Success: continue;
      }
    }

    return IBehaviorNode.EBehaviorNodeState.Success;
  }
}
