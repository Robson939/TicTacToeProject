using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameTests
    {
        [UnityTest]
        public IEnumerator Hint()
        {
            var gameManager = new GameManager();

            Space[,] grid = new Space[3, 3];
            gameManager.SetGrid(grid);

            SimulationSO hintData = ScriptableObject.CreateInstance<SimulationSO>();
            hintData.iterations = 150;
            hintData.depth = 10;

            //  X X -
            //  - O -
            //  O - -

            grid[0, 0].Select(SignType.X);
            grid[1, 0].Select(SignType.X);
            grid[1, 1].Select(SignType.O);
            grid[0, 2].Select(SignType.O);

            MoveInfo bestMove = SimulationManager.Sim(grid, SignType.X, hintData);
            MoveInfo expectedResult = new MoveInfo() { coordinates = (2,0), signType= SignType.X };

            yield return null;

            Assert.IsTrue(expectedResult.Equals(bestMove));
        }

        [UnityTest]
        public IEnumerator Undo()
        {
            var gameManager = new GameManager();

            Space[,] grid = new Space[3, 3];
            gameManager.SetGrid(grid);

            Space[,] expectedGrid = new Space[3, 3];
            gameManager.SetGrid(expectedGrid);

            Stack<MoveInfo> moveInfoList = new Stack<MoveInfo>();
            grid[2, 0].Select(SignType.X);
            grid[1, 1].Select(SignType.O);
            moveInfoList.Push(new MoveInfo() { signType = SignType.X, coordinates = (2, 0) });
            moveInfoList.Push(new MoveInfo() { signType = SignType.O, coordinates = (1, 1) });

            MoveInfo move = moveInfoList.Pop();
            grid[move.coordinates.Item1, move.coordinates.Item2].Select(SignType.None);
            move = moveInfoList.Pop();
            grid[move.coordinates.Item1, move.coordinates.Item2].Select(SignType.None);

            yield return null;

            Assert.IsTrue(CompateTwoGrids(grid, expectedGrid));
        }

        [UnityTest]
        public IEnumerator Win()
        {
            var gameManager = new GameManager();

            Space[,] grid = new Space[3, 3];
            gameManager.SetGrid(grid);

            //  X X X
            //  - O -
            //  O - -

            grid[0, 0].Select(SignType.X);
            grid[1, 0].Select(SignType.X);
            grid[2, 0].Select(SignType.X);
            grid[1, 1].Select(SignType.O);
            grid[0, 2].Select(SignType.O);

            SignType winningSing = GameManager.GetWinningSignType(grid);

            yield return null;

            Assert.IsTrue(SignType.X == winningSing);
        }

        [UnityTest]
        public IEnumerator Lose()
        {
            var gameManager = new GameManager();

            Space[,] grid = new Space[3, 3];
            gameManager.SetGrid(grid);

            //  X X X
            //  - O -
            //  O - -

            grid[0, 0].Select(SignType.X);
            grid[1, 0].Select(SignType.X);
            grid[2, 0].Select(SignType.X);
            grid[1, 1].Select(SignType.O);
            grid[0, 2].Select(SignType.O);

            SignType winningSing = GameManager.GetWinningSignType(grid);

            yield return null;

            Assert.IsTrue(SignType.O != winningSing);
        }

        [UnityTest]
        public IEnumerator Draw()
        {
            var gameManager = new GameManager();

            Space[,] grid = new Space[3, 3];
            gameManager.SetGrid(grid);

            //  X X O
            //  O O X
            //  X X O

            grid[0, 0].Select(SignType.X);
            grid[1, 0].Select(SignType.X);
            grid[2, 0].Select(SignType.O);

            grid[0, 1].Select(SignType.O);
            grid[1, 1].Select(SignType.O);
            grid[2, 1].Select(SignType.X);

            grid[0, 2].Select(SignType.X);
            grid[1, 2].Select(SignType.X);
            grid[2, 2].Select(SignType.O);

            SignType winningSing = GameManager.GetWinningSignType(grid);

            yield return null;

            Assert.IsTrue(SignType.None == winningSing);
        }

        private bool CompateTwoGrids(Space[,] grid, Space[,] expectedGrid)
        {
            for (int x = 0; x < grid.GetLength(0)-1; x++)
            {
                for (int y = 0; y < grid.GetLength(1)-1; y++)
                {
                    if (!(grid[x, y].coordinates == expectedGrid[x, y].coordinates && grid[x, y].currentSignType == expectedGrid[x, y].currentSignType))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}