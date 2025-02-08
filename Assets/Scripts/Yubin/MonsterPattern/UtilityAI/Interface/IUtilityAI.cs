public interface IUtilityAI
{
  // 행동 점수를 계산하여 최적의 행동을 반환
  string ChooseAction();

  // 행동 수행
  void PerformAction(string action);
}
