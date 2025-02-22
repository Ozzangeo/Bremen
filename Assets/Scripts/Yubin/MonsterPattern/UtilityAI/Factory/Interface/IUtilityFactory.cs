using System;

public interface IUtilityFactory
{
  // 특정 행동을 실행하는 메서드 반환
  Action GetAction(string actionName);
}
