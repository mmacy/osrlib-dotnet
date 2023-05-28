/// <summary>
/// Represents a quest in a game, which consists of multiple quest pieces that can be obtained. A quest is completed only when all its required pieces are obtained.
/// </summary>
/// <remarks>
/// This class allows tracking of the quest status and provides an event that notifies subscribers when the quest is completed. Pieces should be added to a quest using the constructor.
/// </remarks>
/// <example>
/// This sample shows how to create an instance of the Quest class and subscribe to the QuestCompleted event.
/// <code>
/// QuestPiece piece1 = new QuestPiece("1", "Piece 1", true);
/// QuestPiece piece2 = new QuestPiece("2", "Piece 2", false);
/// Quest quest = new Quest(1, "Quest 1", "This is a quest.", new List&lt;QuestPiece&gt; { piece1, piece2 });
/// 
/// quest.QuestCompleted += () => Console.WriteLine("Quest completed!");
/// </code>
/// </example>
public class Quest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Quest"/> class with specified ID, name, and optional description and pieces.
    /// </summary>
    /// <param name="id">The ID of the quest.</param>
    /// <param name="name">The name of the quest.</param>
    /// <param name="description">The description of the quest. Defaults to an empty string if not provided.</param>
    /// <param name="pieces">The list of pieces for this quest. Defaults to an empty list if not provided.</param>
    public Quest(int id, string name, string description = "", List<QuestPiece> pieces = null)
    {
        Id = id;
        Name = name;
        Description = description;
        Pieces = pieces ?? new List<QuestPiece>();

        foreach (var piece in Pieces)
        {
            piece.StatusChanged += OnQuestPieceStatusChanged;
        }
    }

    /// <summary>
    /// Gets the ID of the quest.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the name of the quest.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the description of the quest.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Gets the list of pieces of the quest.
    /// </summary>
    public List<QuestPiece> Pieces { get; }

    /// <summary>
    /// Event triggered when all required pieces of the quest are obtained.
    /// </summary>
    /// <remarks>
    /// This event can be used to track the completion of the quest and to trigger further actions when the quest is completed.
    /// </remarks>
    public event Action QuestCompleted;

    /// <summary>
    /// Gets the status of the quest.
    /// </summary>
    /// <remarks>
    /// The status is determined based on the statuses of the quest's pieces: if all required pieces are obtained, the quest is completed; 
    /// if at least one piece is obtained, the quest is in progress; otherwise, the quest is not started.
    /// </remarks>
    public QuestStatus Status =>
        Pieces.Where(p => p.IsRequired).All(p => p.Status == QuestPieceStatus.Obtained) ? QuestStatus.Completed :
        Pieces.Any(p => p.Status == QuestPieceStatus.Obtained) ? QuestStatus.InProgress :
        QuestStatus.NotStarted;

    private void OnQuestPieceStatusChanged(QuestPiece piece, QuestPieceStatus oldStatus, QuestPieceStatus newStatus)
    {
        // Only need to check if the quest is completed when a piece is obtained
        if (newStatus == QuestPieceStatus.Obtained)
        {
            // If all required pieces have been obtained, the quest is completed
            if (Pieces.Where(p => p.IsRequired).All(p => p.Status == QuestPieceStatus.Obtained))
            {
                QuestCompleted?.Invoke();
            }
        }
    }
}
