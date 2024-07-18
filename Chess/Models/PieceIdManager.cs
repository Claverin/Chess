public static class PieceIdManager
{
    private static int _currentId;

    public static int GetNextId()
    {
        return ++_currentId;
    }

    public static void Reset()
    {
        _currentId = 0;
    }
}