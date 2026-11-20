/// <summary>
/// The 'Invoker' class that makes calls to execute the commands
/// 调用者：接收键盘/UI 输入，创建命令并维护命令历史，支持撤销与重做
/// </summary>

using UnityEngine;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{
    public float moveDistance = 10f;        // 每次移动的距离
    public GameObject objectToMove;         // 被移动的目标物体（需在 Inspector 中指定）

    private MoveCommandReceiver moveCommandReciever;    // 接收者
    private List<MoveCommand> commands = new List<MoveCommand>();   // 命令历史
    private int currentCommandNum = 0;                  // 当前命令位置（撤销/重做游标）

    void Start()
    {
        // 创建接收者，命令将在执行时把移动交给它
        moveCommandReciever = new MoveCommandReceiver();

        // 目标物体必须有引用，否则本次演示无法进行
        if (objectToMove == null)
        {
            Debug.LogError("objectToMove must be assigned via inspector");
            this.enabled = false;
        }
    }


    // 撤销：游标回退一位，执行该命令的反向操作
    public void Undo()
    {
        if (currentCommandNum > 0)
        {
            currentCommandNum--;
            MoveCommand moveCommand = commands[currentCommandNum];
            moveCommand.UnExecute();
        }
    }

    // 重做：执行游标处的命令，并把游标前移
    public void Redo()
    {
        if (currentCommandNum < commands.Count)
        {
            MoveCommand moveCommand = commands[currentCommandNum];
            currentCommandNum++;
            moveCommand.Execute();
        }
    }

    // 创建一条移动命令，立即执行并记录到历史中
    private void Move(MoveDirection direction)
    {
        MoveCommand moveCommand = new MoveCommand(moveCommandReciever, direction, moveDistance, objectToMove);
        moveCommand.Execute();
        commands.Add(moveCommand);
        currentCommandNum++;
    }


    //Simple move commands to attach to UI buttons
    // 供 UI 按钮调用的简单移动接口
    public void MoveUp() { Move(MoveDirection.up); }
    public void MoveDown() { Move(MoveDirection.down); }
    public void MoveLeft() { Move(MoveDirection.left); }
    public void MoveRight() { Move(MoveDirection.right); }

    //Shows what's going on in the command list
    // 在屏幕上显示命令历史，">" 标记当前游标位置
    void OnGUI()
    {
        string label = "   start";
        if (currentCommandNum == 0)
        {
            label = ">" + label;
        }
        label += "\n";

        for (int i = 0; i < commands.Count; i++)
        {
            if (i == currentCommandNum - 1)
                label += "> " + commands[i].ToString() + "\n";
            else
                label += "   " + commands[i].ToString() + "\n";

        }
        GUI.Label(new Rect(0, 0, 400, 800), label);
    }

    // 键盘输入：方向键移动，R 重做，U 撤销
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Redo();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Undo();
        }
    }
}
