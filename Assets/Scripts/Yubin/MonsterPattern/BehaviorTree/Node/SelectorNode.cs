using UnityEngine;
using System.Collections.Generic;

// 자식 노드중 Success나 Running 상태를 가진 노드가 있다면 정지
public sealed class SelectorNode : IBehaviorNode
{
  List<IBehaviorNode> childNodes;

  public SelectorNode(List<IBehaviorNode> childNodes) => this.childNodes = childNodes;

  // Failure라면 다음 자식으로 이동, 아니라면 자식 노드의 상태 반환
  public IBehaviorNode.EBehaviorNodeState Evaluate()
  {
    if(childNodes == null) return IBehaviorNode.EBehaviorNodeState.Failure;

    foreach(var child in childNodes)
    {
      switch(child.Evaluate())
      {
        case IBehaviorNode.EBehaviorNodeState.Running: return IBehaviorNode.EBehaviorNodeState.Running;
        case IBehaviorNode.EBehaviorNodeState.Success: return IBehaviorNode.EBehaviorNodeState.Success;
      }
    }

    return IBehaviorNode.EBehaviorNodeState.Failure;
  }
}
