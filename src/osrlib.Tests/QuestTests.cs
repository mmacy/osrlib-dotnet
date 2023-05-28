namespace osrlib.Tests
{
    public class QuestTests
    {
        [Fact]
        public void Quest_IsNotStarted_When_No_Piece_Obtained()
        {
            // Arrange
            var piece1 = new QuestPiece("1", "Piece 1", true);
            var piece2 = new QuestPiece("2", "Piece 2", true);
            var quest = new Quest(1, "Quest 1", "This is a quest.", new List<QuestPiece> { piece1, piece2 });

            // Assert
            Assert.Equal(QuestStatus.NotStarted, quest.Status);
        }

        [Fact]
        public void Quest_IsInProgress_When_Any_Piece_Obtained()
        {
            // Arrange
            var piece1 = new QuestPiece("1", "Piece 1", true);
            var piece2 = new QuestPiece("2", "Piece 2", false);
            var quest = new Quest(1, "Quest 1", "This is a quest.", new List<QuestPiece> { piece1, piece2 });

            // Act
            piece2.Status = QuestPieceStatus.Obtained;

            // Assert
            Assert.Equal(QuestStatus.InProgress, quest.Status);
        }
        
        [Fact]
        public void Quest_IsCompleted_When_All_Pieces_Obtained()
        {
            // Arrange
            var piece1 = new QuestPiece("1", "Piece 1", true);
            var piece2 = new QuestPiece("2", "Piece 2", true);
            var quest = new Quest(1, "Quest 1", "This is a quest.", new List<QuestPiece> { piece1, piece2 });

            // Act
            piece1.Status = QuestPieceStatus.Obtained;
            piece2.Status = QuestPieceStatus.Obtained;

            // Assert
            Assert.Equal(QuestStatus.Completed, quest.Status);
        }

        [Fact]
        public void QuestPiece_StatusChanged_Event_Invoked_When_Status_Changed()
        {
            // Arrange
            var piece = new QuestPiece("1", "Piece 1", true);
            var isStatusChangedTriggered = false;
            piece.StatusChanged += (qp, oldStatus, newStatus) => isStatusChangedTriggered = true;

            // Act
            piece.Status = QuestPieceStatus.Obtained;

            // Assert
            Assert.True(isStatusChangedTriggered);
        }
        
        [Fact]
        public void QuestCompleted_Event_Is_Invoked_When_All_Required_Pieces_Obtained()
        {
            // Arrange
            var piece1 = new QuestPiece("1", "Piece 1", true);
            var piece2 = new QuestPiece("2", "Piece 2", false);
            var quest = new Quest(1, "Quest 1", "This is a quest.", new List<QuestPiece> { piece1, piece2 });
            var isCompletedTriggered = false;
            quest.QuestCompleted += () => isCompletedTriggered = true;

            // Act
            piece1.Status = QuestPieceStatus.Obtained;

            // Assert
            Assert.True(isCompletedTriggered);
        }

        [Fact]
        public void QuestCompleted_Event_Is_Not_Invoked_When_Only_NonRequired_Pieces_Obtained()
        {
            // Arrange
            var piece1 = new QuestPiece("1", "Piece 1", true);
            var piece2 = new QuestPiece("2", "Piece 2", false);
            var quest = new Quest(1, "Quest 1", "This is a quest.", new List<QuestPiece> { piece1, piece2 });
            var isCompletedTriggered = false;
            quest.QuestCompleted += () => isCompletedTriggered = true;

            // Act
            piece2.Status = QuestPieceStatus.Obtained;

            // Assert
            Assert.False(isCompletedTriggered);
        }
    }
}
