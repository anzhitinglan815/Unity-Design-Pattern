//-------------------------------------------------------------------------------------
//	CommandExample1.cs
//	命令模式示例 1：带无限次撤销/重做功能的简易计算器
//-------------------------------------------------------------------------------------

// 该示例演示命令模式在真实场景中的用法：每次计算都封装成命令对象，
// 命令被记录在列表中，从而支持任意次数的 Undo 和 Redo。
// 注意：C# 中 operator 是关键字，加 @ 前缀（@operator）即可作为标识符使用。

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CommandExample1
{
    /// <summary>
    /// 客户端：模拟用户按键计算，然后连续撤销和重做
    /// </summary>
    public class CommandExample1 : MonoBehaviour
    {
	    void Start ( )
        {
            // 创建用户（调用者），由它记录并管理命令历史
            User user = new User();

            // 用户依次按下计算器按钮：+100、-50、*10、/2
            user.Compute('+', 100);
            user.Compute('-', 50);
            user.Compute('*', 10);
            user.Compute('/', 2);

            // 撤销最近 4 条命令
            user.Undo(4);

            // 重做 3 条命令
            user.Redo(3);
        }
    }

    /// <summary>
    /// The 'Command' abstract class
    /// 抽象命令：执行与撤销两个接口
    /// </summary>
    abstract class Command
    {
        public abstract void Execute();
        public abstract void UnExecute();   // 撤销上一步操作
    }

    /// <summary>
    /// The 'ConcreteCommand' class
    /// 具体命令：保存一次计算所需的操作符、操作数和接收者
    /// </summary>
    class CalculatorCommand : Command
    {
        private char _operator;
        private int _operand;
        private Calculator _calculator;

        // Constructor
        public CalculatorCommand(Calculator calculator,
          char @operator, int operand)
        {
            this._calculator = calculator;
            this._operator = @operator;
            this._operand = operand;
        }

        // Gets operator
        public char Operator
        {
            set { _operator = value; }
        }

        // Get operand
        public int Operand
        {
            set { _operand = value; }
        }

        // Execute new command
        // 执行命令：让接收者按原操作符计算
        public override void Execute()
        {
            _calculator.Operation(_operator, _operand);
        }

        // Unexecute last command
        // 撤销命令：使用相反的操作符再计算一次，即可回到原值
        public override void UnExecute()
        {
            _calculator.Operation(Undo(_operator), _operand);
        }

        // Returns opposite operator for given operator
        // 返回与给定操作符相反的操作符，用于撤销
        private char Undo(char @operator)
        {
            switch (@operator)
            {
                case '+': return '-';
                case '-': return '+';
                case '*': return '/';
                case '/': return '*';
                default:
                    throw new
            ArgumentException("@operator");
            }
        }
    }

    /// <summary>
    /// The 'Receiver' class
    /// 接收者：持有当前计算结果，真正执行四则运算
    /// </summary>
    class Calculator
    {
        private int _curr = 0;  // 当前累计结果

        public void Operation(char @operator, int operand)
        {
            switch (@operator)
            {
                case '+': _curr += operand; break;
                case '-': _curr -= operand; break;
                case '*': _curr *= operand; break;
                case '/': _curr /= operand; break;
            }
            Debug.Log("Current value = " + _curr+ " ( following "+ @operator+operand+" )");
        }
    }

    /// <summary>
    /// The 'Invoker' class
    /// 调用者：维护命令历史，负责执行、撤销和重做
    /// </summary>
    class User
    {
        // Initializers
        private Calculator _calculator = new Calculator();          // 接收者
        private List<Command> _commands = new List<Command>();      // 已执行的命令历史
        private int _current = 0;                                   // 当前命令位置（撤销/重做游标）

        // 重做：从当前位置依次向前执行命令
        public void Redo(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                if (_current < _commands.Count - 1)
                {
                    Command command = _commands[_current++];
                    command.Execute();
                }
            }
        }

        // 撤销：游标回退，并调用命令的反向操作
        public void Undo(int levels)
        {
            Debug.Log("\n---- Undo "+ levels + " levels");
            // Perform undo operations
            for (int i = 0; i < levels; i++)
            {
                if (_current > 0)
                {
                    Command command = _commands[--_current] as Command;
                    command.UnExecute();
                }
            }
        }

        // 执行一次计算：创建具体命令、执行并加入历史列表
        public void Compute(char @operator, int operand)
        {
            // Create command operation and execute it
            Command command = new CalculatorCommand(
              _calculator, @operator, operand);
            command.Execute();

            // Add command to undo list
            _commands.Add(command);
            _current++;
        }
    }
}