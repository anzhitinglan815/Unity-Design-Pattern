//-------------------------------------------------------------------------------------
//	TemplateMethodPatternExample1.cs
//	模板方法模式示例 1：用同一套“做三明治”流程制作不同类型的潜艇堡
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

/*
 模板方法模式用于创建一组需要执行相似步骤的子类。
 做法是在抽象类中提供一个“模板方法”，它内部按固定顺序调用一系列方法，
 所有子类都会执行这套流程，但子类可以重写其中某些步骤，甚至通过“钩子”决定某一步是否执行。
 与逐个重写完整流程相比，子类只需关心自己不同的部分，公共流程由基类统一维护。
*/

namespace TemplateMethodPatternExample1
{

    /// <summary>
    /// 客户端：分别制作意大利潜艇堡和素食潜艇堡，观察相同流程下的不同输出
    /// </summary>
    public class TemplateMethodPatternExample1 : MonoBehaviour
    {
        void Start()
        {
            // 基类统一提供制作流程，具体步骤由各自子类实现
            Hoagie cust12Hoagie = new ItalienHoagie();
            cust12Hoagie.MakeSandwich();

            Hoagie cust13Hoagie = new VeggieHoagie();
            cust13Hoagie.MakeSandwich();
        }
    }

    /// <summary>
    /// The 'AbstractClass' abstract class
    /// 抽象类：定义制作潜艇堡的算法骨架，具体配料步骤延迟到子类实现
    /// </summary>
    public abstract class Hoagie
    {
        // 模板方法：规定制作顺序，子类不能改变整体流程
        public void MakeSandwich()
        {
            Debug.Log("Making new Sandwich");

            CutBun();

            // 每个步骤前先询问“钩子”，由子类决定是否执行该步骤
            if (CustomerWantsMeat())
            {
                AddMeat();
            }

            if (CustomerWantsCheese())
            {
                AddCheese();
            }

            if (CustomerWantsVegetables())
            {
                AddVegetables();
            }

            if (CustomerWantsCondiments())
            {
                AddCondiments();
            }

            WrapTheHoagie();
        }

        // 基本操作：必需的步骤由子类必须实现（抽象方法）
        protected abstract void AddMeat();
        protected abstract void AddCheese();
        protected abstract void AddVegetables();
        protected abstract void AddCondiments();

        // 钩子（Hook）：提供默认实现，子类可选择性地重写
        // 默认所有步骤都执行，素食潜艇堡通过重写这些方法跳过肉类和奶酪
        protected virtual bool CustomerWantsMeat() { return true; } // << called Hook
        protected virtual bool CustomerWantsCheese() { return true; }
        protected virtual bool CustomerWantsVegetables() { return true; }
        protected virtual bool CustomerWantsCondiments() { return true; }

        // 固定步骤：所有子类共用，子类无法重写
        protected void CutBun()
        {
            Debug.Log("Bun is Cut");
        }

        protected void WrapTheHoagie()
        {
            Debug.Log("Hoagie is wrapped.");
        }
    }


    /// <summary>
    /// 具体类：意大利潜艇堡，实现全部配料步骤
    /// </summary>
    public class ItalienHoagie : Hoagie
    {
        protected override void AddMeat()
        {
            Debug.Log("Adding the Meat: Salami");
        }

        protected override void AddCheese()
        {
            Debug.Log("Adding the Cheese: Provolone");
        }

        protected override void AddVegetables()
        {
            Debug.Log("Adding the Vegetables: Tomatoes");
        }

        protected override void AddCondiments()
        {
            Debug.Log("Adding the Condiments: Vinegar");
        }
    }



    /// <summary>
    /// 具体类：素食潜艇堡，重写钩子跳过肉类与奶酪
    /// </summary>
    public class VeggieHoagie : Hoagie
    {
        // 抽象方法必须实现，这里留空——因为钩子会阻止该步骤被调用
        protected override void AddMeat()
        {
        }

        protected override void AddCheese()
        {
        }

        protected override void AddVegetables()
        {
            Debug.Log("Adding the Vegetables: Tomatoes");
        }

        protected override void AddCondiments()
        {
            Debug.Log("Adding the Condiments: Vinegar");
        }

        // 重写钩子：返回 false 表示不需要肉类和奶酪
        protected override bool CustomerWantsMeat() { return false; }
        protected override bool CustomerWantsCheese() { return false; }

    }


    namespace BadExample
    {
        // 反面示例：不用模板方法，每个子类都要重写整套制作流程
        // 一旦流程变化（例如新增步骤或某类潜艇堡不做某一步），就要修改所有子类，大量代码重复
        public class ItalienHoagie
        {
            // 完整流程被复制在每个子类中，没有统一的算法骨架
            public void MakeSandwich()
            {
                CutBun();
                AddMeat();
                AddCheese();
                AddVegtables();
                AddCondiments();
                WrapHoagie();
            }

            public void CutBun()
            {
                Debug.Log("Hoagie is Cut");
            }

            public void AddMeat()
            {
                Debug.Log("Added Meat");
            }

            public void AddCheese()
            {
                Debug.Log("Added Cheese");
            }

            public void AddVegtables()
            {
                Debug.Log("Added Vegies");
            }

            public void AddCondiments()
            {
                Debug.Log("Added Condiments");
            }

            public void WrapHoagie()
            {
                Debug.Log("Wrapped Hoagie");
            }
        }
    }
}
