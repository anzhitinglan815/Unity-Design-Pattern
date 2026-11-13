# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 项目概览

基于 Unity 2022.3.20f1 的设计模式教学项目，用于熟悉设计模式的 Demo 教程，包含 23 种 GoF 设计模式和 9 种游戏编程模式。每个模式都是独立的演示场景，并非可运行的游戏：演示结果通过 `Debug.Log` 输出到 Console。项目没有构建流程、没有共享运行时框架，也没有自动化测试（尽管安装了 `com.unity.test-framework`）。

## 常用命令

- 使用 Unity Hub 以 **2022.3.20f1** 版本打开项目，或执行：
  `Unity.exe -projectPath "d:\zcl\Projects\unity-design\Unity-Design-Pattern"`
- 运行演示：在编辑器中打开任意 `.unity` 场景并进入 Play 模式，查看 Console。场景未加入 Build Settings，请在编辑器中运行而不是打包。
- 编译：资源刷新时由 Unity 自动编译。所有脚本都归属于 `Assembly-CSharp`（没有 `.asmdef` 文件），因此没有独立的构建或 lint 步骤。
- 测试：不存在。除非在编辑器 Console 中确认过，否则不要声称改动已验证。

## 目录结构

```
Assets/
  Behavioral Patterns/      GoF 行为型模式
  Creational Patterns/      GoF 创建型模式
  Structural Patterns/      GoF 结构型模式
  Game Programming Patterns/ 《游戏编程模式》中的模式
```

每个模式文件夹遵循相同结构：

- `Structure/` —— 该模式角色类的最小规范实现；场景名为 `Test<Pattern>Structure.unity`。
- `Example1/`、`Example2/` …… —— 贴近实际的用法示例，每个示例一个场景（`<Pattern>ExampleN.unity`）。注意：许多文件夹存在 `Exmaple1` 这一拼写错误，请保留现有拼写。
- `README.md` —— 模式定义、参与者说明和 UML 图片链接。

每个 `Structure`/`Example` 文件夹都是自包含的：包含场景和驱动脚本；需要时，预制体、材质、模型等资源也放在示例文件夹内。

## 代码约定

- 新建或修改代码时，注释统一使用简体中文。
- 每个演示使用独立命名空间（如 `StrategyStructure`、`ObjectPoolPatternExample`），部分演示没有命名空间；角色类和 `MonoBehaviour` 驱动通常位于同一文件，角色类一般为 internal（不写访问修饰符）。
- 文档和角色标记使用 `/// <summary> The 'Context' class </summary>` 这类 XML 注释。部分游戏模式代码和注释为中文。
- 演示逻辑在 `Start()` 中启动，并通过 `Debug.Log` 输出；仅少数示例使用 `OnGUI` 按钮。
- 没有启动器、共享管理器或场景加载代码 —— 场景本身就是入口。
- 仓库根目录和各模式文件夹下的 `README.md` 使用指向 `github.com/QianMo/Unity-Design-Pattern` 的绝对链接（包括 UML 图片）。修改 git remote 不会改写这些链接。
- 新增模式时：按类别/模式创建文件夹，包含 `Structure` 和 `Example` 子文件夹，场景与脚本命名保持一致，并补充模式 `README.md` 和 UML 图片引用。

## 文档约定

- 新增的 `.md` 文档统一保存到 `.zhangzhao/Documents/` 目录下。
- 例外：仓库根目录的 `CLAUDE.md` 以及已存在于各模式文件夹中的 `README.md` 保持原位，不做迁移。
