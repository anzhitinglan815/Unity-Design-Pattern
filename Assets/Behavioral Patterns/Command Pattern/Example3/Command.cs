
/// <summary>
/// The 'Command' abstract class that we will inherit from
/// 抽象命令接口：声明执行与撤销操作，具体命令负责绑定接收者
/// </summary>
public interface Command
{
        void Execute();     // 执行命令
        void UnExecute();   // 撤销命令
    }
