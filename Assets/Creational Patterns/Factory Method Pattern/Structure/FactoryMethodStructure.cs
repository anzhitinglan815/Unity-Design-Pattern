//-------------------------------------------------------------------------------------
//	FactoryMethodStructure.cs
//	工厂方法模式最小结构演示：由子类决定实例化哪个具体产品
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace FactoryMethodStructure
{

    public class FactoryMethodStructure : MonoBehaviour
    {
	    void Start ( )
        {
            // 准备一组创建者，客户端只依赖 Creator 抽象
            // An array of creators
            Creator[] creators = new Creator[2];

            creators[0] = new ConcreteCreatorA();
            creators[1] = new ConcreteCreatorB();

            // 遍历创建者，由各自的工厂方法生产产品
            // Iterate over creators and create products
            foreach (Creator creator in creators)
            {
                Product product = creator.FactoryMethod();
                Debug.Log("Created "+product.GetType().Name);
            }
        }
    }

    /// <summary>
    /// 抽象产品：工厂方法所创建对象的公共接口
    /// The 'Product' abstract class
    /// </summary>
    abstract class Product
    {
    }

    /// <summary>
    /// 具体产品 A
    /// A 'ConcreteProduct' class
    /// </summary>
    class ConcreteProductA : Product
    {
    }

    /// <summary>
    /// 具体产品 B
    /// A 'ConcreteProduct' class
    /// </summary>
    class ConcreteProductB : Product
    {
    }

    /// <summary>
    /// 抽象创建者：声明工厂方法，把实例化延迟到子类
    /// The 'Creator' abstract class
    /// </summary>
    abstract class Creator
    {
        // 工厂方法：返回 Product，具体创建哪个产品由子类决定
        public abstract Product FactoryMethod();
    }

    /// <summary>
    /// 具体创建者 A：重写工厂方法，返回具体产品 A
    /// A 'ConcreteCreator' class
    /// </summary>
    class ConcreteCreatorA : Creator
    {
        public override Product FactoryMethod()
        {
            return new ConcreteProductA();
        }
    }

    /// <summary>
    /// 具体创建者 B：重写工厂方法，返回具体产品 B
    /// A 'ConcreteCreator' class
    /// </summary>
    class ConcreteCreatorB : Creator
    {
        public override Product FactoryMethod()
        {
            return new ConcreteProductB();
        }
    }
}


// 工厂方法模式：工厂A生成产品A，工厂B生成产品B
