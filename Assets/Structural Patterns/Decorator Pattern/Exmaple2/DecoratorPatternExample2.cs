//-------------------------------------------------------------------------------------
//	DecoratorPatternExample2.cs
//	装饰模式示例 2：用配料层层装饰披萨，动态计算描述和价格
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

// 该示例把“披萨”作为组件，把“配料”作为装饰器：
// 每加一种配料就套一层装饰器，GetDescription/GetCost 会沿装饰链递归累加。
// 新增配料只需新增一个装饰器类，不需要为每种组合创建披萨子类。

namespace DecoratorPatternExample2
{
    /// <summary>
    /// 客户端：从基础披萨开始，依次套上奶酪和番茄酱两层装饰
    /// </summary>
    public class DecoratorPatternExample2 : MonoBehaviour
    {
        void Start()
        {
            // 组装顺序：PlainPizza → 被 Mozzarella 包装 → 再被 TomatoSauce 包装
            // 构造时从内向外执行，因此先输出奶酪的日志，再输出番茄酱的日志
            IPizza basicPizza = new TomatoSauce(new Mozzarella(new PlainPizza()));

            // GetDescription/GetCost 会从最外层向内层递归，逐层追加内容与价格
            Debug.Log("Ingredients of Pizza: " + basicPizza.GetDescription());
            Debug.Log("Total Cost: " + basicPizza.GetCost());
        }
    }


    /// <summary>
    /// The 'Component' interface
    /// 抽象组件：披萨和配料装饰器共同实现的接口
    /// </summary>
    public interface IPizza
    {
        string GetDescription();
        double GetCost();
    }


    /// <summary>
    /// The 'ConcreteComponent' class
    /// 具体组件：基础披萨（薄饼），提供最原始的描述和价格
    /// </summary>
    public class PlainPizza : IPizza
    {
        public string GetDescription()
        {
            return "Thin Dough";
        }

        public double GetCost()
        {
            return 4.00;
        }
    }



    /// <summary>
    /// The 'Decorator' abstract class
    /// 抽象配料装饰器：持有被装饰的披萨，并默认把请求转发给它
    /// </summary>
    public abstract class ToppingDecorator : IPizza
    {
        // 被装饰的对象，可以是基础披萨，也可以是另一层配料
        protected IPizza tempPizza;

        public ToppingDecorator(IPizza newPizza)
        {
            this.tempPizza = newPizza;
        }


        // 默认实现：原样转发描述
        public virtual string GetDescription()
        {
            return tempPizza.GetDescription();
        }

        // 默认实现：原样转发价格
        public virtual double GetCost()
        {
            return tempPizza.GetCost();
        }
    }



    /// <summary>
    /// 具体装饰器：马苏里拉奶酪，描述追加配料名，价格增加 0.50
    /// </summary>
    public class Mozzarella : ToppingDecorator
    {
        public Mozzarella(IPizza newPizza) : base(newPizza)
        {
            // 示例在构造时输出日志，便于观察装饰发生的顺序
            Debug.Log("Adding Dough");
            Debug.Log("Adding Morarella");
        }

        // 在内层描述后追加奶酪
        public override string GetDescription()
        {
            return tempPizza.GetDescription() + ", Mozzarella";
        }

        // 在内层价格上累加奶酪费用
        public override double GetCost()
        {
            return tempPizza.GetCost() + 0.50;
        }
    }


    /// <summary>
    /// 具体装饰器：番茄酱，描述追加配料名，价格增加 0.35
    /// </summary>
    public class TomatoSauce : ToppingDecorator
    {
        public TomatoSauce(IPizza newPizza) : base(newPizza)
        {
            Debug.Log("Adding Sauce");
        }

        // 在内层描述后追加番茄酱
        public override string GetDescription()
        {
            return tempPizza.GetDescription() + ", Tomato Sauce";
        }

        // 在内层价格上累加番茄酱费用
        public override double GetCost()
        {
            return tempPizza.GetCost() + 0.35;
        }
    }




    namespace BadStyleExample
    {
        // 反面示例：把每种披萨组合都做成子类
        // 这样会为每种配料组合产生一个子类；一旦某种配料价格变化，
        // 所有包含它的披萨子类都要跟着修改，类的数量也会迅速膨胀。
        public abstract class Pizza
        {
            public abstract void SetDescription(string newDescription);
            public abstract string GetDescription();
            public abstract double GetCost();
            //public abstract bool HasFontina();
        }

        // 每个“披萨 + 配料组合”都要单独写一个子类，难以扩展
        public class ThreeCheesePizza : Pizza
        {
            public override void SetDescription(string newDescription)
            {
            }

            public override string GetDescription()
            {
                return "Mozarella, Fontina, Parmesan, Cheese Pizza";
            }

            public override double GetCost()
            {
                return 10.00;
            }

        }
    }

}
