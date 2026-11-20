//-------------------------------------------------------------------------------------
//	DecoratorPatternExample1.cs
//	装饰模式示例 1：给图书馆馆藏动态增加“可借阅”功能
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 该示例演示真实场景中的装饰模式：
// Book、Video 是原始馆藏；Borrowable 是装饰器，可以在不修改 Book/Video 的情况下
// 为某个馆藏动态添加借阅功能，并记录借阅人。

namespace DecoratorPatternExample1
{

    /// <summary>
    /// 客户端：创建书籍和视频，再用 Borrowable 装饰视频并借出
    /// </summary>
    public class DecoratorPatternExample1 : MonoBehaviour
    {
        void Start()
        {
            // Create book
            // 书籍作为普通馆藏直接展示
            Book book = new Book("Worley", "Inside ASP.NET", 10);
            book.Display();

            // Create video
            // 视频稍后会被装饰，先展示原始信息
            Video video = new Video("Spielberg", "Jaws", 23, 92);
            video.Display();

            // Make video borrowable, then borrow and display
            // 用 Borrowable 包装视频，动态获得“可借阅”能力
            Debug.Log("\nMaking video borrowable:");

            Borrowable borrowvideo = new Borrowable(video);
            borrowvideo.BorrowItem("Customer #1");
            borrowvideo.BorrowItem("Customer #2");

            // 装饰器的 Display 会先显示原始视频信息，再追加借阅人列表
            borrowvideo.Display();
        }
    }

    /// <summary>
    /// The 'Component' abstract class
    /// 抽象组件：所有馆藏和装饰器共同实现的接口
    /// </summary>
    abstract class LibraryItem
    {
        private int _numCopies;

        // Property
        // 馆藏副本数量；装饰器借出/归还时会修改它
        public int NumCopies
        {
            get { return _numCopies; }
            set { _numCopies = value; }
        }

        // 展示馆藏信息，由具体馆藏实现
        public abstract void Display();
    }

    /// <summary>
    /// The 'ConcreteComponent' class
    /// 具体组件：书籍
    /// </summary>
    class Book : LibraryItem
    {
        private string _author;
        private string _title;

        // Constructor
        public Book(string author, string title, int numCopies)
        {
            this._author = author;
            this._title = title;
            this.NumCopies = numCopies;
        }

        // 展示书籍的基础信息
        public override void Display()
        {
            Debug.Log("\nBook ------ ");
            Debug.Log(" Author: "+ _author);
            Debug.Log(" Title: "+ _title);
            Debug.Log(" # Copies: "+ NumCopies);
        }
    }

    /// <summary>
    /// The 'ConcreteComponent' class
    /// 具体组件：视频
    /// </summary>
    class Video : LibraryItem
    {
        private string _director;
        private string _title;
        private int _playTime;

        // Constructor
        public Video(string director, string title,
          int numCopies, int playTime)
        {
            this._director = director;
            this._title = title;
            this.NumCopies = numCopies;
            this._playTime = playTime;
        }

        // 展示视频的基础信息
        public override void Display()
        {
            Debug.Log("\nVideo ----- ");
            Debug.Log(" Director: "+ _director);
            Debug.Log(" Title: "+ _title);
            Debug.Log(" # Copies: "+ NumCopies);
            Debug.Log(" Playtime: "+ _playTime+ "\n");
        }
    }

    /// <summary>
    /// The 'Decorator' abstract class
    /// 抽象装饰器：持有一个馆藏（组件）引用，并把 Display 转发给它
    /// </summary>
    abstract class Decorator : LibraryItem
    {
        // 被装饰的馆藏；装饰器与组件实现同一接口，因此可以层层嵌套
        protected LibraryItem libraryItem;

        // Constructor
        public Decorator(LibraryItem libraryItem)
        {
            this.libraryItem = libraryItem;
        }

        // 默认行为：原样转发给被装饰对象
        public override void Display()
        {
            libraryItem.Display();
        }
    }

    /// <summary>
    /// The 'ConcreteDecorator' class
    /// 具体装饰器：给馆藏增加“可借阅”职责
    /// </summary>
    class Borrowable : Decorator
    {
        // 记录借阅人；这是装饰器新增的状态
        protected List<string> borrowers = new List<string>();

        // Constructor
        public Borrowable(LibraryItem libraryItem)
          : base(libraryItem)
        {
        }

        // 借出：记录借阅人并减少副本数
        public void BorrowItem(string name)
        {
            borrowers.Add(name);
            libraryItem.NumCopies--;
        }

        // 归还：移除借阅记录并恢复副本数
        public void ReturnItem(string name)
        {
            borrowers.Remove(name);
            libraryItem.NumCopies++;
        }

        // 先展示原始馆藏信息，再追加借阅人列表
        public override void Display()
        {
            base.Display();

            foreach (string borrower in borrowers)
            {
                Debug.Log(" borrower: " + borrower);
            }
        }
    }
}
