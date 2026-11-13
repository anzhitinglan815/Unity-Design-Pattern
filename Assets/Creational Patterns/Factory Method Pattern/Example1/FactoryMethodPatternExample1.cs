//-------------------------------------------------------------------------------------
//	FactoryMethodPatternExample1.cs
//	工厂方法模式示例 1：文档由子类决定包含哪些页面
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This real-world code demonstrates the Factory method offering flexibility in creating different documents.
//The derived Document classes Report and Resume instantiate extended versions of the Document class. Here, the Factory Method is called in the constructor of the Document base class.

namespace FactoryMethodPatternExample1
{
    public class FactoryMethodPatternExample1 : MonoBehaviour
    {
        void Start()
        {
            // 注意：基类构造函数会调用工厂方法 CreatePages
            // Note: constructors call Factory Method
            Document[] documents = new Document[2];

            documents[0] = new Resume();
            documents[1] = new Report();

            // 输出每份文档包含的页面，验证子类创建的页面组合
            // Display document pages
            foreach (Document document in documents)
            {
                Debug.Log("\n" + document.GetType().Name + "--");
                foreach (Page page in document.Pages)
                {
                    Debug.Log(" " + page.GetType().Name);
                }
            }

        }
    }

    /// <summary>
    /// 抽象产品：文档页面的公共基类
    /// The 'Product' abstract class
    /// </summary>
    abstract class Page
    {
    }

    /// <summary>
    /// 具体产品：技能页
    /// A 'ConcreteProduct' class
    /// </summary>
    class SkillsPage : Page
    {
    }

    /// <summary>
    /// 具体产品：教育经历页
    /// A 'ConcreteProduct' class
    /// </summary>
    class EducationPage : Page
    {
    }

    /// <summary>
    /// 具体产品：工作经验页
    /// A 'ConcreteProduct' class
    /// </summary>
    class ExperiencePage : Page
    {
    }

    /// <summary>
    /// 具体产品：引言页
    /// A 'ConcreteProduct' class
    /// </summary>
    class IntroductionPage : Page
    {
    }

    /// <summary>
    /// 具体产品：结果页
    /// A 'ConcreteProduct' class
    /// </summary>
    class ResultsPage : Page
    {
    }

    /// <summary>
    /// 具体产品：结论页
    /// A 'ConcreteProduct' class
    /// </summary>
    class ConclusionPage : Page
    {
    }

    /// <summary>
    /// 具体产品：总结页
    /// A 'ConcreteProduct' class
    /// </summary>
    class SummaryPage : Page
    {
    }

    /// <summary>
    /// 具体产品：参考文献页
    /// A 'ConcreteProduct' class
    /// </summary>
    class BibliographyPage : Page
    {
    }

    /// <summary>
    /// 抽象创建者：定义文档的通用流程，并把“创建页面”交给子类
    /// The 'Creator' abstract class
    /// </summary>
    abstract class Document
    {
        // 页面集合，由子类的工厂方法填充
        private List<Page> _pages = new List<Page>();

        // 构造函数中调用抽象工厂方法，此时子类已确定
        // Constructor calls abstract Factory method
        public Document()
        {
            this.CreatePages();
        }

        // 只读页面列表，供客户端查看
        public List<Page> Pages
        {
            get { return _pages; }
        }

        // 工厂方法：具体创建哪些页面由子类实现
        // Factory Method
        public abstract void CreatePages();
    }

    /// <summary>
    /// 具体创建者：简历，决定简历包含的页面组合
    /// A 'ConcreteCreator' class
    /// </summary>
    class Resume : Document
    {
        // 工厂方法实现：依次创建简历所需的页面
        // Factory Method implementation
        public override void CreatePages()
        {
            Pages.Add(new SkillsPage());
            Pages.Add(new EducationPage());
            Pages.Add(new ExperiencePage());
        }
    }

    /// <summary>
    /// 具体创建者：报告，决定报告包含的页面组合
    /// A 'ConcreteCreator' class
    /// </summary>
    class Report : Document
    {
        // 工厂方法实现：依次创建报告所需的页面
        // Factory Method implementation
        public override void CreatePages()
        {
            Pages.Add(new IntroductionPage());
            Pages.Add(new ResultsPage());
            Pages.Add(new ConclusionPage());
            Pages.Add(new SummaryPage());
            Pages.Add(new BibliographyPage());
        }
    }
}
