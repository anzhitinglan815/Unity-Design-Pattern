/// <summary>
/// The 'Receiver' class - this handles what a move command actually does
/// 接收者：真正执行移动操作，命令对象只负责把参数转交给它
/// </summary>

using UnityEngine;

    class MoveCommandReceiver
    {
        // 按方向对目标物体执行位移
        public void MoveOperation(GameObject gameObjectToMove, MoveDirection direction, float distance)
        {
            switch (direction)
            {
                case MoveDirection.up:
                    MoveY(gameObjectToMove, distance);
                    break;
                case MoveDirection.down:
                    MoveY(gameObjectToMove, -distance);
                    break;
                case MoveDirection.left:
                    MoveX(gameObjectToMove, -distance);
                    break;
                case MoveDirection.right:
                    MoveX(gameObjectToMove, distance);
                    break;
            }
        }

        // 沿 Y 轴移动
        private void MoveY(GameObject gameObjectToMove, float distance)
        {
            Vector3 newPos = gameObjectToMove.transform.position;
            newPos.y += distance;
            gameObjectToMove.transform.position = newPos;
        }

        // 沿 X 轴移动
        private void MoveX(GameObject gameObjectToMove, float distance)
        {
            Vector3 newPos = gameObjectToMove.transform.position;
            newPos.x += distance;
            gameObjectToMove.transform.position = newPos;
        }
    }

