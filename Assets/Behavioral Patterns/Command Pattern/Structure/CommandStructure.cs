//-------------------------------------------------------------------------------------
//	CommandStructure.cs
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace CommandStructure
{
    /// <summary>
    /// 客户端：组装接收者、命令与调用者，并触发命令执行
    /// </summary>
    public class CommandStructure : MonoBehaviour
    {
	    void Start ( )
        {
            // 创建接收者（真正干活的类）、命令（把请求封装成对象）和调用者（发起请求）
            Receiver receiver = new Receiver();
            Command command = new ConcreteCommand(receiver);
            Invoker invoker = new Invoker();

            // 调用者只认识抽象命令，不知道具体执行什么
            invoker.SetCommand(command);
            invoker.ExecuteCommand();
        }
    }

    /// <summary>
    /// The 'Command' abstract class
    /// 抽象命令：声明执行操作的接口，并持有接收者引用
    /// </summary>
    abstract class Command
    {
        protected Receiver receiver;

        // Constructor
        public Command(Receiver receiver)
        {
            this.receiver = receiver;
        }

        // 执行命令：具体做什么由子类调用接收者完成
        public abstract void Execute();
    }

    /// <summary>
    /// The 'ConcreteCommand' class
    /// 具体命令：把“接收者 + 动作”绑定在一起
    /// </summary>
    class ConcreteCommand : Command
    {
        // Constructor
        public ConcreteCommand(Receiver receiver) :
          base(receiver)
        {
        }

        // 将请求转发给接收者执行
        public override void Execute()
        {
            receiver.Action();
        }
    }

    /// <summary>
    /// The 'Receiver' class
    /// 接收者：真正知道如何完成请求的类
    /// </summary>
    class Receiver
    {
        public void Action()
        {
          Debug.Log("Called Receiver.Action()");
        }
    }

    /// <summary>
    /// The 'Invoker' class
    /// 调用者：持有命令并触发执行，不关心命令内部细节
    /// </summary>
    class Invoker
    {
        private Command _command;

        // 设置要执行的命令，可在运行时替换
        public void SetCommand(Command command)
        {
            this._command = command;
        }

        // 触发命令执行
        public void ExecuteCommand()
        {
            _command.Execute();
        }
    }

}