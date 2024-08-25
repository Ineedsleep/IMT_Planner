public class Rank
{
    private static readonly int[] _fragmentsNeeded = { 0, 15, 30, 50, 80, 120 };
    private int _currentRank = 0;

    public int CurrentRank
    {
        get => _currentRank;
        set
        {
            if (value is >= 0 and <= 5 )
                _currentRank = value;
        }
    }

    // Returns remaining fragments needed to level up
    public int FragmentsNeededToLevelUp
    {
        get
        {
            // If maximum rank reached, return 0
            if (CurrentRank == 5)
            {
                return 0;
            }
            // Otherwise, return the needed fragments for the next level
            else
            {
                return _fragmentsNeeded[CurrentRank + 1];
            }
        }
    }

    // Attempts to raise the rank by amount of fragments passed, returns true if successful
    public bool TryLevelUp(int fragments)
    {
        // If enough fragments are passed and the rank is not maximum
        if (fragments >= FragmentsNeededToLevelUp && CurrentRank < 5)
        {
            CurrentRank++;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Constructor that initializes the rank to zero
    public Rank()
    {
    }

    public Rank(int rank)
    {
        CurrentRank = rank;
    }
}