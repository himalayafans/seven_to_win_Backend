namespace SevenToWinBackend.Library.Strategy;

/// <summary>
/// 策略处理抽象类
/// </summary>
public abstract class BaseStrategyHandler
{
    /// <summary>
    /// 后继者
    /// </summary>
    protected BaseStrategyHandler? Successor;

    public void SetSuccessor(BaseStrategyHandler successor)
    {
        Successor = successor;
    }
    /// <summary>
    /// 处理请求
    /// </summary>
    public abstract void Handle(PlayResult result);
}