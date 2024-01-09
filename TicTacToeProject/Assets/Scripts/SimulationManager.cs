using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationManager
{
    private static int maxDepth = 9;

    public static MoveInfo Sim(Space[,] grid, SignType playerSign, SimulationSO simulationSO)
    {
        List<(Space[,], List <MoveInfo>)> iterations = new List<(Space[,], List<MoveInfo>)>();
        Dictionary<MoveInfo, MoveCost> winningMoves = new Dictionary<MoveInfo, MoveCost>();
        maxDepth = simulationSO.depth;

        for (int i = 0; i < simulationSO.iterations; i++)
        {
            Space[,] newGrid = GetCopy(grid);
            List<MoveInfo> newInfos = new List<MoveInfo>();
            HandleRandomMove(newGrid, 0, newInfos, playerSign);

            iterations.Add((newGrid, newInfos));
        }

        foreach (var iteration in iterations)
        {
            if (GameManager.GetWinningSignType(iteration.Item1) == playerSign)
            {
                if (!winningMoves.ContainsKey(iteration.Item2[0]))
                {
                    winningMoves.Add(iteration.Item2[0], new MoveCost());
                }

                winningMoves[iteration.Item2[0]].moveRepeats++;
                winningMoves[iteration.Item2[0]].moveLength += iteration.Item2.Count;
            }
        }

        MoveInfo theBestMove;

        if (winningMoves.Count > 0)
        {
            theBestMove = winningMoves.OrderByDescending(x => x.Value.Cost).Last().Key;
        }
        else
        {
            (Space[,], List<MoveInfo>) getRandomIteration = iterations[Random.Range(0, iterations.Count)];
            theBestMove = getRandomIteration.Item2[0];
        }

        return theBestMove;
    }

    public class MoveCost
    {
        public int moveLength;
        public int moveRepeats;
        public int Cost { get => moveLength / moveRepeats; }
    }


    private static void HandleRandomMove(Space[,] grid, byte depth, List<MoveInfo> MoveInfos, SignType playerSign)
    {
        (byte, byte)? result = GetRandomMove(grid);
        depth++;

        if (result == null || depth >= maxDepth || GameManager.GetWinningSignType(grid) != SignType.None)
        {
            return;
        }
        else
        {
            grid[(byte)result.Value.Item1, (byte)result.Value.Item2].Select(playerSign);
            MoveInfos.Add(new MoveInfo() { signType = playerSign, coordinates = ((byte)result.Value.Item1, (byte)result.Value.Item2) }) ;
            SignType otherPlayerSign = playerSign == SignType.X ? SignType.O : SignType.X;  //make move for other player to simulate whole turn of game
            HandleRandomMove(grid, depth, MoveInfos, otherPlayerSign);
        }
    }

    private static  (byte, byte)? GetRandomMove(Space[,] grid)
    {
        List<Space> gridList = grid.Cast<Space>().ToList();
        List<Space> legalSpaces = gridList.Where(x => x.currentSignType == SignType.None).ToList();

        if (legalSpaces.Count > 1)
        {
            int randomId = Random.Range(0, legalSpaces.Count - 1);
            return legalSpaces[randomId].coordinates;
        }
        else if (legalSpaces.Count == 1)
        {
            return legalSpaces[0].coordinates;
        }

        return null;
    }

    private static Space[,] GetCopy(Space[,] spaces) 
    {
        Space[,] newGrid = new Space[spaces.GetLength(0), spaces.GetLength(1)];
        
        for (int i = 0; i < spaces.GetLength(0); i++)
        {
            for (int j = 0; j < spaces.GetLength(1); j++)
            {
                newGrid[i, j] = spaces[i, j].GetClone();
            }
        }

        return newGrid;
    }

    private static Space[,] CastSpaces(Space[] spaces)
    {
        Space[,] grid = new Space[3, 3];

        byte index = 0;

        for (byte i = 0; i < 3; i++)
        {
            for (byte j = 0; j < 3; j++)
            {
                spaces[index].coordinates = (i, j);
                grid[i, j] = spaces[index];

                index++;
            }
        }

        return grid;
    }
}