/// <summary>
/// A simple example of a class inheriting from a command pattern
/// This handles execution of the command as well as unexecution of the command
/// 具体命令：保存移动所需的全部信息（目标、方向、距离、接收者）
/// 执行时转交给接收者；撤销时改用相反方向再移动一次
/// </summary>

using UnityEngine;

    // A basic enum to describe our movement
    // 移动方向枚举
    public enum MoveDirection { up, down, left, right };

    class MoveCommand : Command
    {
        private MoveDirection _direction;       // 移动方向
        private MoveCommandReceiver _receiver;  // 接收者
        private float _distance;                // 移动距离
        private GameObject _gameObject;         // 目标物体


        //Constructor
        public MoveCommand(MoveCommandReceiver reciever, MoveDirection direction, float distance, GameObject gameObjectToMove)
        {
            this._receiver = reciever;
            this._direction = direction;
            this._distance = distance;
            this._gameObject = gameObjectToMove;
        }


        //Execute new command
        // 执行：按指定方向移动
        public void Execute()
        {
            _receiver.MoveOperation(_gameObject, _direction, _distance);
        }


        //Undo last command
        // 撤销：沿相反方向移动相同距离，回到原位
        public void UnExecute()
        {
            _receiver.MoveOperation(_gameObject, InverseDirection(_direction), _distance);
        }


        //invert the direction for undo
        // 取得相反方向
        private MoveDirection InverseDirection(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.up:
                    return MoveDirection.down;
                case MoveDirection.down:
                    return MoveDirection.up;
                case MoveDirection.left:
                    return MoveDirection.right;
                case MoveDirection.right:
                    return MoveDirection.left;
                default:
                    Debug.LogError("Unknown MoveDirection");
                    return MoveDirection.up;
            }
        }


        //So we can show this command in debug output easily
        // 便于在界面上显示命令内容
        public override string ToString()
        {
            return _gameObject.name + " : " + MoveDirectionString(_direction) + " : " + _distance.ToString();
        }


        //Convert the MoveDirection enum to a string for debug
        // 把方向枚举转换成可读字符串
        public string MoveDirectionString(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.up:
                    return "up";
                case MoveDirection.down:
                    return "down";
                case MoveDirection.left:
                    return "left";
                case MoveDirection.right:
                    return "right";
                default:
                    return "unkown";
            }
        }
    }
