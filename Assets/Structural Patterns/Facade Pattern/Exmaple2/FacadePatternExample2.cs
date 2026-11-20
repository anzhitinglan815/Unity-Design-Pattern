//-------------------------------------------------------------------------------------
//	FacadePatternExample2.cs
//	外观模式示例 2：银行账户操作，用 BankAccountFacade 统一封装多个校验子系统
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

// 该示例模拟 ATM / 网银的一次交易：
// 取款和存款本来需要依次做账号校验、密码校验、余额校验，
// BankAccountFacade 把这些步骤组合成一个 WithdrawCash / DepositCash 接口，
// 客户端不需要知道内部的四个子系统。

namespace FacadePatternExample2
{
    /// <summary>
    /// 客户端：只与银行账户外观交互，完成取款和存款
    /// </summary>
    public class FacadePatternExample2 : MonoBehaviour
    {
        void Start()
        {
            // 创建外观，传入账号 12345678 和安全码 1234
            BankAccountFacade bankAccount = new BankAccountFacade(12345678, 1234);

            // 余额 1000，取 50 应当成功，余额变为 950
            Debug.Log("\"I want to withdraw $50 dollars!\"");
            bankAccount.WithdrawCash(50.00);

            // 取 5000 超过余额，余额校验失败，交易被拒绝
            Debug.Log("\"Mh ok now let me withdraw $5000 dollars!?\"");
            bankAccount.WithdrawCash(5000.00);

            // 存入 300，余额变为 1250
            Debug.Log("\"What tf... maybe save some cash, here are $300 bucks ;-)\"");
            bankAccount.DepositCash(300.00);
        }
    }

    /// <summary>
    /// 子系统：欢迎信息，负责在创建时输出欢迎语
    /// </summary>
    public class WelcomeToBank
    {
        public WelcomeToBank()
        {
            Debug.Log("Welcome to ABC Bank");
            Debug.Log("We are happy to deposit your money :-)");
        }
    }

    /// <summary>
    /// 子系统：账号校验，判断账号是否存在且有效
    /// </summary>
    public class AccountNumberCheck
    {
        // 演示用固定账号，真实项目应查询数据库
        private int accountNumber = 12345678;

        public int GetAccountNumber()
        {
            return accountNumber;
        }

        // 传入账号与内部账号一致则视为有效
        public bool AccountActive(int accNumber)
        {
            if (accNumber == accountNumber)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// 子系统：安全码校验，判断密码是否正确
    /// </summary>
    public class SecurityCodeCheck
    {
        // 演示用固定安全码
        private int securityCode = 1234;

        public int GetSecurityCode()
        {
            return securityCode;
        }

        // 传入安全码与内部安全码一致则通过
        public bool IsCodeCorrect(int code)
        {
            if (code == securityCode)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// 子系统：资金校验与账务处理，维护账户余额
    /// </summary>
    public class FundsCheck
    {
        // 账户初始余额
        private double cashInAccount = 1000.00;

        public double GetCashInAccount()
        {
            return cashInAccount;
        }

        // 扣减余额（供内部取款流程调用）
        protected void DecreaseCashInAccount(double cash)
        {
            cashInAccount -= cash;
        }

        // 增加余额（供内部存款流程调用）
        protected void IncreaseCashInAccount(double cash)
        {
            cashInAccount += cash;
        }

        // 判断余额是否足够取款
        public bool HaveEnoughMoney(double cashToWithdraw)
        {
            if (cashToWithdraw > GetCashInAccount())
            {
                Debug.Log("You don't have enouth money.");
                return false;
            }
            else
            {
                return true;
            }
        }

        // 存款：增加余额并输出当前余额
        public void MakeDeposit(double cash)
        {
            IncreaseCashInAccount(cash);
            Debug.Log("Deposit complete. Current Balance is: " + GetCashInAccount());
        }

        // 取款：扣减余额并输出当前余额
        public void WithdrawMoney(double cash)
        {
            DecreaseCashInAccount(cash);
            Debug.Log("Withdraw complete. Current Balance is: " + GetCashInAccount());
        }
    }

    /// <summary>
    /// The 'Facade' class
    /// 外观：把账号、安全码、资金、欢迎语四个子系统组合成简单的银行操作接口
    /// </summary>
    public class BankAccountFacade
    {
        // 客户端传入的账号与安全码
        private int accountNumber;
        private int securityCode;

        // 外观持有四个子系统，客户端不需要直接接触它们
        AccountNumberCheck accChecker;
        SecurityCodeCheck codeChecker;
        FundsCheck fundChecker;
        WelcomeToBank bankWelcome;

        public BankAccountFacade(int accountNumber, int newSecurityCode)
        {
            this.accountNumber = accountNumber;
            this.securityCode = newSecurityCode;

            // 由外观负责创建子系统
            bankWelcome = new WelcomeToBank();
            codeChecker = new SecurityCodeCheck();
            accChecker = new AccountNumberCheck();
            fundChecker = new FundsCheck();
        }

        public int GetAccountNumber()
        {
            return accountNumber;
        }

        public int GetSecurityCode()
        {
            return securityCode;
        }

        // 取款：依次校验账号、安全码和余额，全部通过才真正扣款
        public void WithdrawCash(double cash)
        {
            if (accChecker.AccountActive(GetAccountNumber())
                && codeChecker.IsCodeCorrect(GetSecurityCode())
                && fundChecker.HaveEnoughMoney(cash))
            {
                fundChecker.WithdrawMoney(cash);
                Debug.Log("Transaction complete.");
            }
            else
            {
                Debug.Log("Transaction failed.");
            }
        }

        // 存款：校验账号和安全码，通过后增加余额（存款不限制金额）
        public void DepositCash(double cash)
        {
            if (accChecker.AccountActive(GetAccountNumber())
                && codeChecker.IsCodeCorrect(GetSecurityCode()))
            {
                fundChecker.MakeDeposit(cash);
                Debug.Log("Transaction complete.");
            }
            else
            {
                Debug.Log("Transaction failed.");
            }
        }
    }

}
