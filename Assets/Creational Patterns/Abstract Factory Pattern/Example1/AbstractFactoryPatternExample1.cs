//-------------------------------------------------------------------------------------
//	AbstractFactoryPatternExample1.cs
//	抽象工厂模式示例 1：不同大陆的工厂创建各自配套的食草动物与食肉动物
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;


//This structural code demonstrates the Abstract Factory pattern creating parallel hierarchies of objects.
//Object creation has been abstracted and there is no need for hard-coded class names in the client code.

namespace AbstractFactoryPatternExample1
{
    public class AbstractFactoryPatternExample1 : MonoBehaviour
    {
        void Start()
        {
            // 创建并运行“非洲动物世界”（非洲产品族）
            // Create and run the African animal world
            ContinentFactory africa = new AfricaFactory();
            AnimalWorld world = new AnimalWorld(africa);
            world.RunFoodChain();

            // 创建并运行“美洲动物世界”（美洲产品族）
            // Create and run the American animal world
            ContinentFactory america = new AmericaFactory();
            world = new AnimalWorld(america);
            world.RunFoodChain();
        }
    }


    /// <summary>
    /// 抽象工厂：声明创建食草动物和食肉动物的接口
    /// The 'AbstractFactory' abstract class
    /// </summary>
    abstract class ContinentFactory
    {
        public abstract Herbivore CreateHerbivore();
        public abstract Carnivore CreateCarnivore();
    }

    /// <summary>
    /// 具体工厂 1：非洲大陆，创建非洲的食草、食肉动物
    /// The 'ConcreteFactory1' class
    /// </summary>
    class AfricaFactory : ContinentFactory
    {
        public override Herbivore CreateHerbivore()
        {
            return new Wildebeest();
        }
        public override Carnivore CreateCarnivore()
        {
            return new Lion();
        }
    }

    /// <summary>
    /// 具体工厂 2：美洲大陆，创建美洲的食草、食肉动物
    /// The 'ConcreteFactory2' class
    /// </summary>
    class AmericaFactory : ContinentFactory
    {
        public override Herbivore CreateHerbivore()
        {
            return new Bison();
        }
        public override Carnivore CreateCarnivore()
        {
            return new Wolf();
        }
    }

    /// <summary>
    /// 抽象产品 A：食草动物
    /// The 'AbstractProductA' abstract class
    /// </summary>
    abstract class Herbivore
    {
    }

    /// <summary>
    /// 抽象产品 B：食肉动物，需要捕食食草动物
    /// The 'AbstractProductB' abstract class
    /// </summary>
    abstract class Carnivore
    {
        public abstract void Eat(Herbivore h);
    }

    /// <summary>
    /// 具体产品 A1：角马（非洲食草动物）
    /// The 'ProductA1' class
    /// </summary>
    class Wildebeest : Herbivore
    {
    }

    /// <summary>
    /// 具体产品 B1：狮子（非洲食肉动物）
    /// The 'ProductB1' class
    /// </summary>
    class Lion : Carnivore
    {
        public override void Eat(Herbivore h)
        {
            // 狮子捕食角马
            // Eat Wildebeest
            Debug.Log(this.GetType().Name +" eats " + h.GetType().Name);
        }
    }

    /// <summary>
    /// 具体产品 A2：野牛（美洲食草动物）
    /// The 'ProductA2' class
    /// </summary>
    class Bison : Herbivore
    {
    }

    /// <summary>
    /// 具体产品 B2：狼（美洲食肉动物）
    /// The 'ProductB2' class
    /// </summary>
    class Wolf : Carnivore
    {
        public override void Eat(Herbivore h)
        {
            // 狼捕食野牛
            // Eat Bison
            Debug.Log(this.GetType().Name +" eats " + h.GetType().Name);
        }
    }

    /// <summary>
    /// 客户端：动物世界，只依赖抽象工厂与抽象产品
    /// The 'Client' class
    /// </summary>
    class AnimalWorld
    {
        private Herbivore _herbivore;
        private Carnivore _carnivore;

        // 注入抽象工厂，由它创建同一大陆配套的两种动物
        // Constructor
        public AnimalWorld(ContinentFactory factory)
        {
            _carnivore = factory.CreateCarnivore();
            _herbivore = factory.CreateHerbivore();
        }

        // 运行食物链：食肉动物捕食食草动物
        public void RunFoodChain()
        {
            _carnivore.Eat(_herbivore);
        }
    }
}
