/// <summary>
/// Represents a quest piece in the game.
/// </summary>
public class QuestPiece
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuestPiece"/> class.
    /// </summary>
    /// <param name="id">The ID of the quest piece.</param>
    /// <param name="name">The name of the quest piece.</param>
    /// <param name="isRequired">Whether the quest piece is required to complete the quest.</param>
    /// <param name="description">The description of the quest piece. Defaults to an empty string if not provided.</param>
    /// <param name="status">The initial status of the quest piece. Defaults to <see cref="QuestPieceStatus.NotObtained"/> if not provided.</param>
    public QuestPiece(string id, string name, bool isRequired, string description = "", QuestPieceStatus status = QuestPieceStatus.NotObtained)
    {
        Id = id;
        Name = name;
        Description = description;
        IsRequired = isRequired;
        Status = status;
    }
    
    /// <summary>
    /// Gets the ID of the quest piece.
    /// </summary>
    public string Id { get; init; }

    /// <summary>
    /// Gets the name of the quest piece.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the description of the quest piece.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Indicates whether this quest piece is required to complete the quest.
    /// </summary>
    public bool IsRequired { get; }

    private QuestPieceStatus _status;

    /// <summary>
    /// Gets or sets the status of this quest piece.
    /// </summary>
    public QuestPieceStatus Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                var oldStatus = _status;
                _status = value;
                StatusChanged?.Invoke(this, oldStatus, _status);
            }
        }
    }

    /// <summary>
    /// Event triggered when the status of the quest piece changes.
    /// The event handler receives three arguments:
    /// - An instance of <see cref="QuestPiece"/>, which represents the quest piece whose status has changed.
    /// - A value from the <see cref="QuestPieceStatus"/> enumeration representing the old status of the quest piece.
    /// - A value from the <see cref="QuestPieceStatus"/> enumeration representing the new status of the quest piece.
    /// </summary>
    /// <remarks>
    /// The StatusChanged event can be used to track changes to the quest piece's status in real time, and to trigger further actions when a status change occurs.
    /// </remarks>
    /// <example>
    /// This sample shows how to use the StatusChanged event.
    /// <code>
    /// QuestPiece piece = new QuestPiece(1, "Piece 1", "First piece", true, QuestPieceStatus.NotObtained);
    /// piece.StatusChanged += (qp, oldStatus, newStatus) =>
    /// {
    ///     Console.WriteLine($"QuestPiece {qp.Id}: Status changed from {oldStatus} to {newStatus}");
    /// };
    /// </code>
    /// </example>
    public event Action<QuestPiece, QuestPieceStatus, QuestPieceStatus> StatusChanged;
}