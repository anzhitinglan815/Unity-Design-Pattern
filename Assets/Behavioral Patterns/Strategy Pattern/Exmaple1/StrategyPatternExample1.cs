//-------------------------------------------------------------------------------------
//	StrategyPatternExample1.cs
//	策略模式示例一：把排序算法封装为可互换的策略
//-------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This real-world code demonstrates the Strategy pattern which encapsulates sorting algorithms in the form of sorting objects.
//This allows clients to dynamically change sorting strategies including Quicksort, Shellsort, and Mergesort.
// 中文说明：本例把排序算法封装成独立的策略对象，
// 客户端可以在运行时动态切换快速排序、希尔排序、归并排序等算法，而无需修改 SortedList 本身。

namespace StrategyPatternExample1
{
    public class StrategyPatternExample1 : MonoBehaviour
    {
        void Start()
        {
            // 同一个上下文对象，通过切换策略使用不同的排序算法
            // Two contexts following different strategies
            SortedList studentRecords = new SortedList();

            // 添加待排序的学生姓名
            studentRecords.Add("Samual");
            studentRecords.Add("Jimmy");
            studentRecords.Add("Sandra");
            studentRecords.Add("Vivek");
            studentRecords.Add("Anna");

            // 运行时依次替换排序策略，SortedList 的调用方式保持不变
            studentRecords.SetSortStrategy(new QuickSort());
            studentRecords.Sort();

            studentRecords.SetSortStrategy(new ShellSort());
            studentRecords.Sort();

            studentRecords.SetSortStrategy(new MergeSort());
            studentRecords.Sort();

        }
    }

    /// <summary>
    /// 抽象策略：声明统一的排序接口，各具体算法实现该方法
    /// The 'Strategy' abstract class
    /// </summary>
    abstract class SortStrategy
    {
        public abstract void Sort(List<string> list);
    }

    /// <summary>
    /// 具体策略：快速排序
    /// A 'ConcreteStrategy' class
    /// </summary>
    class QuickSort : SortStrategy
    {
        public override void Sort(List<string> list)
        {
            // 直接使用 List 的默认排序（内部为快速排序）
            list.Sort(); // Default is Quicksort
            Debug.Log("-------QuickSorted list------- ");
        }
    }

    /// <summary>
    /// 具体策略：希尔排序
    /// A 'ConcreteStrategy' class
    /// </summary>
    class ShellSort : SortStrategy
    {
        public override void Sort(List<string> list)
        {
            // 演示用：未真正实现希尔排序，只输出提示信息
            //list.ShellSort(); not-implemented
            Debug.Log("-------ShellSorted list------- ");
        }
    }

    /// <summary>
    /// 具体策略：归并排序
    /// A 'ConcreteStrategy' class
    /// </summary>
    class MergeSort : SortStrategy
    {
        public override void Sort(List<string> list)
        {
            // 演示用：未真正实现归并排序，只输出提示信息
            //list.MergeSort(); not-implemented
            Debug.Log("-------MergeSorted list------- ");
        }
    }

    /// <summary>
    /// 上下文：维护数据列表，并把排序工作委托给当前策略
    /// The 'Context' class
    /// </summary>
    class SortedList
    {
        private List<string> _list = new List<string>();
        // 当前使用的排序策略
        private SortStrategy _sortstrategy;

        // 替换排序策略：客户端借此在不同算法之间切换
        public void SetSortStrategy(SortStrategy sortstrategy)
        {
            this._sortstrategy = sortstrategy;
        }

        // 添加上下文自身的数据
        public void Add(string name)
        {
            _list.Add(name);
        }

        public void Sort()
        {
            // 具体采用哪种排序由注入的策略决定
            _sortstrategy.Sort(_list);
            // 遍历并输出排序结果
            // Iterate over list and display results
            foreach (string name in _list)
            {
                Debug.Log(" " + name);
            }
        }
    }
}