using Botec.CommandProcessor.Enums;

namespace Botec.CommandProcessor.Utilities;

public static class RandomWithProbability
{
    private static Random _random;
    private static int[,] _probabilityMatrix;

    static RandomWithProbability()
    {
        _random = new Random();
        _probabilityMatrix = GetProbabilityMatrix();
    }

    public static int GetRandomNumber()
    {
        // TODO: magic numbers
        var chanceIndex = _random.Next(0, 100);
        for (var i = 0; i < 6; i++)
        {
            if (chanceIndex > _probabilityMatrix[i, 0] && chanceIndex < _probabilityMatrix[i, 1])
            {
                return _random.Next(_probabilityMatrix[i, 2], _probabilityMatrix[i, 3]);
            }
        }

        return 0;
    }

    private static int[,] GetProbabilityMatrix()
    {
        // Example:
        // { 40, 70, 10, 15 }
        // 40 - lower probability limit
        // 70 - upper probability limit
        // 10 - lower limit of generation
        // 15 - upper limit of generation
        
        return new[,]
        {
            { 0, 40, 0, 10 },
            { 41, 71, 10, 15 },
            { 72, 78, 15, 20 },
            { 79, 98, -25, 0},
            { 99, 99, (int)CockConstants.Double, (int)CockConstants.Double},
            { 100, 100, (int)CockConstants.Circumcision, (int)CockConstants.Circumcision}
        };
    }
}