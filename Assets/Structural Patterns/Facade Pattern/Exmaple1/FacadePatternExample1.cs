//-------------------------------------------------------------------------------------
//	FacadePatternExample1.cs
//	外观模式示例 1：贷款资格审核，用 Mortgage 统一封装银行、信用、贷款三个子系统
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

// 该示例演示真实场景中的外观模式：
// Mortgage（贷款申请）对外只暴露一个 IsEligible 方法，内部依次调用
// Bank、Credit、Loan 三个子系统的检查，客户端不需要逐个了解这些子系统。

namespace FacadePatternExample1
{
    /// <summary>
    /// 客户端：创建贷款申请（外观），只调用一个高层方法完成资格审核
    /// </summary>
    public class FacadePatternExample1 : MonoBehaviour
    {
        void Start()
        {
            // Facade
            // 外观对象内部持有银行、信用、贷款等子系统
            Mortgage mortgage = new Mortgage();

            // Evaluate mortgage eligibility for customer
            // 客户端只提交客户和金额，具体审核流程由外观负责
            Customer customer = new Customer("Ann McKinsey");
            bool eligible = mortgage.IsEligible(customer, 125000);

            // 输出最终结果：Approved 或 Rejected
            Debug.Log("\n" + customer.Name +
                " has been " + (eligible ? "Approved" : "Rejected"));
        }
    }

    /// <summary>
    /// The 'Subsystem ClassA' class
    /// 子系统：银行，负责检查客户的存款是否足够
    /// </summary>
    class Bank
    {
        // 示例中固定返回 true；真实项目里应查询账户余额
        public bool HasSufficientSavings(Customer c, int amount)
        {
            Debug.Log("Check bank for " + c.Name);
            return true;
        }
    }

    /// <summary>
    /// The 'Subsystem ClassB' class
    /// 子系统：信用，负责检查客户的信用记录
    /// </summary>
    class Credit
    {
        // 示例中固定返回 true；真实项目里应查询信用评分
        public bool HasGoodCredit(Customer c)
        {
            Debug.Log("Check credit for " + c.Name);
            return true;
        }
    }

    /// <summary>
    /// The 'Subsystem ClassC' class
    /// 子系统：贷款，负责检查客户是否有不良贷款记录
    /// </summary>
    class Loan
    {
        // 示例中固定返回 true；真实项目里应查询贷款历史
        public bool HasNoBadLoans(Customer c)
        {
            Debug.Log("Check loans for " + c.Name);
            return true;
        }
    }

    /// <summary>
    /// Customer class
    /// 客户数据类，只保存姓名，不属于外观模式的角色
    /// </summary>
    class Customer
    {
        private string _name;

        // Constructor
        public Customer(string name)
        {
            this._name = name;
        }

        // Gets the name
        public string Name
        {
            get { return _name; }
        }
    }

    /// <summary>
    /// The 'Facade' class
    /// 外观：把三个子系统的检查组合成一个简单的审核接口
    /// </summary>
    class Mortgage
    {
        // 外观统一持有并创建子系统，客户端不会直接访问它们
        private Bank _bank = new Bank();
        private Loan _loan = new Loan();
        private Credit _credit = new Credit();

        // 高层接口：依次检查存款、不良贷款和信用记录
        // 使用 else if 串联，表示一旦某项不通过，后续检查就不再执行
        public bool IsEligible(Customer cust, int amount)
        {
            Debug.Log(cust.Name + "applies for " + amount+ " loan\n");

            bool eligible = true;

            // Check creditworthyness of applicant
            if (!_bank.HasSufficientSavings(cust, amount))
            {
                eligible = false;
            }
            else if (!_loan.HasNoBadLoans(cust))
            {
                eligible = false;
            }
            else if (!_credit.HasGoodCredit(cust))
            {
                eligible = false;
            }

            return eligible;
        }
    }
}
