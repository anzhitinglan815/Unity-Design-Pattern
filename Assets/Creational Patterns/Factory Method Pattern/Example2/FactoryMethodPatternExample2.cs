//-------------------------------------------------------------------------------------
//	FactoryMethodPatternExample2.cs
//	工厂方法模式示例 2：把敌机创建逻辑集中到工厂类，对比散落在客户端的“坏代码”
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace FactoryMethodPatternExample2
{
    public class FactoryMethodPatternExample2 : MonoBehaviour
    {
        // 客户端只持有工厂引用，不直接 new 具体敌机
        EnemyShipFactory factory;
        void Start()
        {
            factory = new EnemyShipFactory();
            Debug.Log("please type U、R、B for test~");
        }

        void Update( )
        {
            // 按 U：生成 UFO 敌机
            if (Input.GetKeyDown(KeyCode.U))
            {
                EnemyShip ship = factory.MakeEnemyShip(EnemyShipFactory.ShipType.UFO);
                DoStuffEnemy(ship);
            }
            // 按 R：生成火箭敌机
            else if (Input.GetKeyDown(KeyCode.R))
            {
                EnemyShip ship = factory.MakeEnemyShip(EnemyShipFactory.ShipType.Rocket);
                DoStuffEnemy(ship);
            }
            // 按 B：生成 Boss 敌机
            else if (Input.GetKeyDown(KeyCode.B))
            {
                EnemyShip ship = factory.MakeEnemyShip(EnemyShipFactory.ShipType.Endboss);
                DoStuffEnemy(ship);
            }
        }

        // 对敌机的通用操作：客户端只面向 EnemyShip 抽象
        void DoStuffEnemy(EnemyShip enemyShip)
        {
            enemyShip.displayEnemyShip();
            enemyShip.followHeroShip();
            enemyShip.enemyShipShoots();
        }

    }

    /// <summary>
    /// 敌机工厂：集中管理创建逻辑，客户端只需传入类型
    /// （这是工厂方法的一种简化形式，也常称为“简单工厂”或参数化工厂）
    /// </summary>
    // move that desicion making into another class:
    public class EnemyShipFactory
    {
        // 可供创建的敌机类型
        public enum ShipType
        {
            UFO,
            Rocket,
            Endboss
        }
        ;

        // 根据类型参数返回对应的具体敌机，客户端不关心构造细节
        public EnemyShip MakeEnemyShip(ShipType shipType)
        {
            if (shipType == ShipType.UFO)
                return new UFOEnemyShip();
            else if (shipType == ShipType.Rocket)
                return new RocketEnemyShip();
            else if (shipType == ShipType.Endboss)
                return new BigEnemyShip();
            else
                return null;
        }
    }

    /// <summary>
    /// 反例：客户端自己判断类型并直接 new，创建逻辑分散在业务代码中
    /// </summary>
    public class BadCodeExample : MonoBehaviour
    {
        void Start()
        {
            // 直接依赖具体类，新增敌机类型时必须修改这里
            EnemyShip ufo = new UFOEnemyShip();
            Debug.Log("What kind of ship do you want? (R / U)");
        }

        // yes its dynamic but it is also modifyable
        // you don't want this sort of stuff going on inside of your actual program
        // lots of if and else statements and logic
        void Update()
        {
            // 反例：创建逻辑与业务逻辑耦合，新增类型要改这段分支
            //return; // < "deactivating"


            if (Input.GetKeyDown(KeyCode.U))
            {
                EnemyShip ship = new UFOEnemyShip();
                DoStuffEnemy(ship);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                EnemyShip ship = new RocketEnemyShip();
                DoStuffEnemy(ship);
            }
        }

        // 对敌机的通用操作，与工厂版本的调用方式一致
        void DoStuffEnemy(EnemyShip enemyShip)
        {
            enemyShip.displayEnemyShip();
            enemyShip.followHeroShip();
            enemyShip.enemyShipShoots();
        }
    }

    /// <summary>
    /// 抽象产品：敌机，定义所有敌机的通用行为
    /// </summary>
    public abstract class EnemyShip
    {
        public string name;
        public double damage;

        public void followHeroShip()
        {
            Debug.Log(name + " is following the hero.");
        }

        public void displayEnemyShip()
        {
            Debug.Log(name + " is on the screen.");
        }

        public void enemyShipShoots()
        {
            Debug.Log(name + " attacks and does " + damage + " damage");
        }
    }

    /// <summary>
    /// 具体产品：UFO 敌机
    /// </summary>
    public class UFOEnemyShip : EnemyShip
    {
        public UFOEnemyShip()
        {
            name = "UFOEnemyShip";
            damage = 20;
        }
    }

    /// <summary>
    /// 具体产品：火箭敌机
    /// </summary>
    public class RocketEnemyShip : EnemyShip
    {
        public RocketEnemyShip()
        {
            name = "RocketEnemyShip";
            damage = 10;
        }
    }

    /// <summary>
    /// 具体产品：Boss 敌机（继承 UFO，但覆盖名称和伤害）
    /// </summary>
    public class BigEnemyShip : UFOEnemyShip
    {
        public BigEnemyShip()
        {
            name = "BigEnemyShip";
            damage = 40;
        }
    }

}
