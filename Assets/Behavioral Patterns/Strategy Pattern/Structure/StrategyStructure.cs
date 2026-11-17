//-------------------------------------------------------------------------------------
//	StrategyStructure.cs
//	策略模式最小结构演示：把一组可互换的算法封装成独立的策略类
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace StrategyStructure
{
    public class StrategyStructure : MonoBehaviour
    {
	    void Start ( )
        {
            Context context;

            // 三个上下文分别使用不同的策略
            // 上下文只依赖抽象策略，具体使用哪种算法由创建上下文时传入的策略决定
            // Three contexts following different strategies
            context = new Context(new ConcreteStrategyA());
            context.ContextInterface();

            context = new Context(new ConcreteStrategyB());
            context.ContextInterface();

            context = new Context(new ConcreteStrategyC());
            context.ContextInterface();
        }
    }

    /// <summary>
    /// 抽象策略：声明所有具体算法都必须实现的统一接口
    /// The 'Strategy' abstract class
    /// </summary>
    abstract class Strategy
    {
        // 上下文通过该接口调用具体算法
        public abstract void AlgorithmInterface();
    }

    /// <summary>
    /// 具体策略 A：实现算法 A
    /// A 'ConcreteStrategy' class
    /// </summary>
    class ConcreteStrategyA : Strategy
    {
        public override void AlgorithmInterface()
        {
            Debug.Log("Called ConcreteStrategyA.AlgorithmInterface()");
        }
    }

    /// <summary>
    /// 具体策略 B：实现算法 B
    /// A 'ConcreteStrategy' class
    /// </summary>
    class ConcreteStrategyB : Strategy
    {
        public override void AlgorithmInterface()
        {
            Debug.Log("Called ConcreteStrategyB.AlgorithmInterface()");
        }
    }

    /// <summary>
    /// 具体策略 C：实现算法 C
    /// A 'ConcreteStrategy' class
    /// </summary>
    class ConcreteStrategyC : Strategy
    {
        public override void AlgorithmInterface()
        {
            Debug.Log("Called ConcreteStrategyC.AlgorithmInterface()");
        }
    }

    /// <summary>
    /// 上下文：持有一个抽象策略的引用，把算法调用委托给具体策略
    /// The 'Context' class
    /// </summary>
    class Context
    {
        // 运行时保持对当前策略的引用，替换该引用即可切换算法
        private Strategy _strategy;

        // 构造函数：注入具体的策略对象
        // Constructor
        public Context(Strategy strategy)
        {
            this._strategy = strategy;
        }

        // 对外接口保持不变，内部调用当前策略的算法
        public void ContextInterface()
        {
            _strategy.AlgorithmInterface();
        }
    }
}


//比较简单，在上下文中注入策略即可

