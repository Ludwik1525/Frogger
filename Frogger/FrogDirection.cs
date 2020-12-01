namespace Frogger
{
    class FrogDirection 
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
        }

        Direction currentDirection;
        GameState currentGameState;

        public FrogDirection(Direction currentDirection)
        {
            this.currentDirection = currentDirection;
        }

        public Direction GetCurrentDirection()
        {
            return currentDirection;
        }

        public void MoveUp()
        {
            ChangeDirection(Direction.Up);
        }

        public void MoveDown()
        {
            ChangeDirection(Direction.Down);
        }

        public void MoveRight()
        {
            ChangeDirection(Direction.Right);
        }

        public void MoveLeft()
        {
            ChangeDirection(Direction.Left);
        }

        private void ChangeDirection(Direction direction)
        {
            this.currentDirection = direction;
        }

        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }

        private void ChangeGameState(GameState gameState)
        {
            this.currentGameState = gameState;
        }

        public void StartMoving()
        {
            ChangeGameState(GameState.Moving);
        }

        public void StopMoving()
        {
            ChangeGameState(GameState.NotMoving);
        }
    }
}


