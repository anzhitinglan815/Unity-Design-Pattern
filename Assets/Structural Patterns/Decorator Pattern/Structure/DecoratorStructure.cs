//-------------------------------------------------------------------------------------
//	DecoratorStructure.cs
//	装饰模式的最小结构：像套娃一样层层包装，动态叠加职责
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace DecoratorStructure
{
    /// <summary>
    /// 客户端：创建具体组件和两个装饰器，并手动组装成装饰链
    /// </summary>
    public class DecoratorStructure : MonoBehaviour
    {
	    void Start ( )
        {
            // 创建具体组件（被装饰对象）和两个具体装饰器
            ConcreteComponent c = new ConcreteComponent();
            ConcreteDecoratorA d1 = new ConcreteDecoratorA();
            ConcreteDecoratorB d2 = new ConcreteDecoratorB();

            // 组装装饰链：d1 包装 c，d2 再包装 d1
            // 调用 d2.Operation() 时，会沿着链依次向内转发
            d1.SetComponent(c);
            d2.SetComponent(d1);

            // 执行顺序：d2 → d1 → c，返回时再依次执行各装饰器附加的行为
            d2.Operation();
        }
    }

    /// <summary>
    /// The 'Component' abstract class
    /// 抽象组件：定义被装饰对象和装饰器共同实现的接口
    /// </summary>
    abstract class Component
    {
        public abstract void Operation();
    }

    /// <summary>
    /// The 'ConcreteComponent' class
    /// 具体组件：原始对象，提供基础行为
    /// </summary>
    class ConcreteComponent : Component
    {
        public override void Operation()
        {
            Debug.Log("ConcreteComponent.Operation()");
        }
    }

    /// <summary>
    /// The 'Decorator' abstract class
    /// 抽象装饰器：持有组件引用，并实现与组件相同的接口
    /// 这样装饰器才能替代组件，也可以再被其他装饰器包装
    /// </summary>
    abstract class Decorator : Component
    {
        protected Component component;

        // 注入被装饰的对象，可在运行时替换
        public void SetComponent(Component component)
        {
            this.component = component;
        }

        // 默认行为：把请求原样转发给被装饰对象
        public override void Operation()
        {
            if (component != null)
            {
                component.Operation();
            }
        }
    }

    /// <summary>
    /// The 'ConcreteDecoratorA' class
    /// 具体装饰器 A：先执行被装饰对象，再附加自己的行为
    /// </summary>
    class ConcreteDecoratorA : Decorator
    {
        public override void Operation()
        {
            base.Operation();   // 先让内层对象执行
            Debug.Log("ConcreteDecoratorA.Operation()");  // 再附加 A 的职责
        }
    }

    /// <summary>
    /// The 'ConcreteDecoratorB' class
    /// 具体装饰器 B：可以附加多个行为，也可改变执行顺序
    /// </summary>
    class ConcreteDecoratorB : Decorator
    {
        public override void Operation()
        {
            base.Operation();       // 先执行内层对象
            AddedBehavior();        // 附加 B 特有的行为
            Debug.Log("ConcreteDecoratorB.Operation()");
        }

        // 装饰器新增的职责；示例中留空
        void AddedBehavior()
        {
        }
    }

}
