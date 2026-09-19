using System;
using System.Collections.Generic;

public interface IWeighted
{
    float Weight { get; }
}

public abstract class WeightedSelector<T> where T : IWeighted
{
    /// <summary>
    /// Takes in a list of weighted items and uses the seed to select a random item based on the weight.
    /// </summary>
    /// <param name="items">The list of weighted items to select from.</param>
    /// <param name="seed">The seed for the random number generator.</param>
    /// <returns>A randomly selected item from the list based on weight.</returns>
    public static T SelectWeightedRandom(IList<T> items, int seed)
    {
        System.Random random = new(seed);

        if (items == null || items.Count == 0) return default;

        // Add up all weights
        float totalWeight = 0f;
        for (int i = 0; i < items.Count; i++)
        {
            totalWeight += Math.Max(0f, items[i].Weight);
        }

        if (totalWeight <= 0f) return default;

        // System.Random.NextDouble() returns a value between 0.0 and 1.0. Multiply by total weight to get a value between 0 and totalWeight
        double randomValue = random.NextDouble() * totalWeight;

        // Actually select item based on weight
        double currentCumulativeWeight = 0f;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Weight <= 0f) continue; // skip current item if its weight is zero or negative

            currentCumulativeWeight += items[i].Weight; // add the current item's weight in

            // and check if the random number picked above is less than or equal to the cumulative weight of the current item
            // (example: 2 items with 50 weight means 100 total. if randomValue is 60, the first item's cumulative weight is 50,
            // so it's not selected, but the second one's cumulative is 100, so it is)
            if (randomValue <= currentCumulativeWeight) 
            {
                return items[i];
            }
        }

        return items[items.Count - 1];
    }
}
