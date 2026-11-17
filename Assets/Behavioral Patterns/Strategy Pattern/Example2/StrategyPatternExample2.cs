//-------------------------------------------------------------------------------------
//	StrategyPatternExample2.cs
//	策略模式示例二：把“飞行方式”抽离为可替换的行为策略
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace StrategyPatternExample2
{
    public class StrategyPatternExample2 : MonoBehaviour
    {
        void Start()
        {
            // 创建狗和鸟两个示例对象
            // create dog and bird example objects
            Animal sparky = new Dog();
            Animal tweety = new Bird();

            // 初始状态下，狗不会飞，鸟会飞
            Debug.Log("Dog: " + sparky.TryToFly());
            Debug.Log("Bird: " + tweety.TryToFly());

            // 运行时替换狗的行为策略，使其具备飞行能力
            // change behaviour of dog
            sparky.SetFlyingAbility(new ItFlys());

            // 切换策略后，狗和鸟都能飞
            Debug.Log("Dog: " + sparky.TryToFly());
            Debug.Log("Bird: " + tweety.TryToFly());
        }
    }


    // 使用接口解耦，把会变化的行为放到独立的策略类中
    // Using Interfaces for decoupling
    // putting behaviour that varies in different classes
    public interface IFly
    {
        string Fly();
    }

    // 具体策略：会飞的行为
    // Class that holds behaviour for objects that can fly
    class ItFlys : IFly
    {
        public string Fly()
        {
            return "Flying high";
        }
    }

    // 具体策略：不会飞的行为
    // Class that holds behaviour for objects that can not fly
    class CantFly : IFly
    {
        public string Fly()
        {
            return "I can't fly";
        }
    }

    // 具体策略：飞得极快的行为
    class FlyingSuperFast : IFly
    {
        public string Fly()
        {
            return "Fly super fast";
        }
    }


    // 上下文：持有一个飞行策略实例，本身不实现具体飞行行为
    // Classes that hold an instance of the behaviours above:
    public class Animal
    {
        // 组合策略对象，而不是把行为写死在继承体系中
        // 这样行为可以在运行时替换
        // hereby adding the behaviour
        // it also can change that way
        public IFly flyingType;

        // 把飞行请求委托给当前策略
        public string TryToFly()
        {
            return flyingType.Fly();
        }

        // 运行时切换飞行策略
        public void SetFlyingAbility(IFly newFlyingType)
        {
            this.flyingType = newFlyingType;
        }
    }

    // 子类只负责选择默认策略，不重复实现飞行行为
    // derived objects that vary from the base Animal:
    public class Dog : Animal
    {
        public Dog()
        {
            // 狗默认不会飞
            flyingType = new CantFly();
        }
    }

    public class Bird : Animal
    {
        public Bird()
        {
            // 鸟默认会飞
            flyingType = new ItFlys();
        }
    }
}

// Rembember:
// ALWAYS
// eliminate duplicate code
// eliminate technices that allow one class to affect others