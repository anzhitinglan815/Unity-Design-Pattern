//-------------------------------------------------------------------------------------
//	BuilderPatternExample2.cs
//	建造者模式示例 2：工程师（指挥者）按步骤指挥建造者组装机器人
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace BuilderPatternExample2
{
    public class BuilderPatternExample2 : MonoBehaviour
    {
        void Start()
        {
            // 客户端选择具体建造者，并交给指挥者
            IRobotBuilder oldRobot = new OldRobotBuilder();
            RobotEngineer engineer = new RobotEngineer(oldRobot);

            // 指挥者按固定步骤建造机器人
            engineer.MakeRobot();

            // 从指挥者处取得最终产品并输出
            Robot firstRobot = engineer.GetRobot();
            Debug.Log("First Robot built");
            Debug.Log(firstRobot.ToStringEX());
        }
    }

    /// <summary>
    /// 产品接口：定义机器人各部件的设置方法
    /// </summary>
    public interface IRobotPlan
    {
        void SetRobotHead(string head);

        void SetRobotTorso(string torso);

        void SetRobotArms(string arms);

        void SetRobotLegs(string legs);
    }

    /// <summary>
    /// 产品：机器人，保存各部件并对外提供描述
    /// </summary>
    public class Robot : IRobotPlan
    {
        // 各部件由建造者通过 Set 方法写入，外部只读
        public string head { get; protected set; }
        public string torso { get; protected set; }
        public string arms { get; protected set; }
        public string legs { get; protected set; }

        public void SetRobotHead(string head)
        {
            this.head = head;
        }

        public void SetRobotTorso(string torso)
        {
            this.torso = torso;
        }

        public void SetRobotArms(string arms)
        {
            this.arms = arms;
        }

        public void SetRobotLegs(string legs)
        {
            this.legs = legs;
        }

        // 输出机器人各部件信息
        public string ToStringEX()
        {
            return "Head: " + this.head + ", torso: " + this.torso + ", Arms: " + arms + ", legs: " + legs;
        }
    }




    // 建造者接口就像“蓝图”：规定要构建哪些部件，以及如何取回产品
    // they're kinda like a blueprint these RobotBuilder classes:
    public interface IRobotBuilder
    {
        Robot GetRobot();
        void BuildRobotHead();
        void BuildRobotTorso();
        void BuildRobotArms();
        void BuildRobotLegs();
    }

    /// <summary>
    /// 具体建造者：旧式机器人，决定每个部件使用什么材料
    /// </summary>
    // for each new robot that you might want to have just create a new RobotBuilder Object
    public class OldRobotBuilder : IRobotBuilder
    {
        // 建造者内部维护正在构建的产品
        protected Robot robot { get; set; }

        public OldRobotBuilder()
        {
            this.robot = new Robot();
        }

        // 返回构建完成的产品
        public Robot GetRobot()
        {
            return robot;
        }

        public void BuildRobotHead()
        {
            this.robot.SetRobotHead("Old Head");
        }

        public void BuildRobotTorso()
        {
            this.robot.SetRobotTorso("Old Torso");
        }

        public void BuildRobotArms()
        {
            this.robot.SetRobotArms("Old Arms");
        }

        public void BuildRobotLegs()
        {
            this.robot.SetRobotLegs("Roller Skates");
        }
    }



    /// <summary>
    /// 指挥者：工程师，只负责按固定顺序调用建造方法
    /// </summary>
    // he just calls the method in the Robot Objects (which are defined by the interface, just think of blueprints)
    public class RobotEngineer
    {
        // 持有抽象建造者，可在运行时替换不同的机器人建造者
        public IRobotBuilder robotBuilder { get; protected set; }

        public RobotEngineer(IRobotBuilder builder)
        {
            this.robotBuilder = builder;
        }

        // 构建完成后，从建造者处取回产品
        public Robot GetRobot()
        {
            return this.robotBuilder.GetRobot();
        }

        // 固定的建造流程：头 → 躯干 → 手臂 → 腿
        public void MakeRobot()
        {
            this.robotBuilder.BuildRobotHead();
            this.robotBuilder.BuildRobotTorso();
            this.robotBuilder.BuildRobotArms();
            this.robotBuilder.BuildRobotLegs();
        }
    }


}


// 机器人有4个部件，分别是：头、躯干、手臂、腿
// 抽象的机器人建造者，需要实现4个部件的建造方法，以及获取最终产品的方法
// 具体的机器人建造者，决定每个部件的具体材料

//指挥者，注入建造者，按固定顺序调用建造方法，最后从建造者处取回产品
