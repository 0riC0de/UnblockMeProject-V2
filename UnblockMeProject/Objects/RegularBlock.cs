using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UnblockMeProject
{
    public class RegularBlock
    {
        private Rectangle rectangle;
        private bool isDragging = false;
        private Point clickPosition;
        private TranslateTransform transform = new TranslateTransform();
        private Grid gameBoard;
        private int currentRowOrColumn;
        private bool isHorizontal;
        private MainWindow mainWindow;

        public int Row { get; set; }
        public int Column { get; set; }
        public int RowSpan { get; set; }
        public int ColumnSpan { get; set; }
        public Brush Fill { get; set; }

        public RegularBlock(Grid gameBoard, int row, int column, int rowSpan, int columnSpan, bool isHorizontal , MainWindow mainWindow)
        {
            this.gameBoard = gameBoard;
            this.mainWindow = mainWindow;
            this.Row = row;
            this.Column = column;
            this.RowSpan = rowSpan;
            this.ColumnSpan = columnSpan;
            this.Fill = Brushes.Blue;
            this.isHorizontal = isHorizontal;
            this.currentRowOrColumn = isHorizontal ? column : row;

            InitializeBlock();
        }

        private void InitializeBlock()
        {
            rectangle = new Rectangle
            {
                Fill = Fill,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Width = ColumnSpan * 50, // Assume each column is 50 units wide
                Height = RowSpan * 50, // Assume each row is 50 units high
                Name = "RegularBlockName",
                Tag = "RegularBlockTag"
            };

            Grid.SetRow(rectangle, Row);
            Grid.SetColumn(rectangle, Column);
            Grid.SetRowSpan(rectangle, RowSpan);
            Grid.SetColumnSpan(rectangle, ColumnSpan);

            rectangle.RenderTransform = transform;

            rectangle.MouseDown += Rectangle_MouseDown;
            rectangle.MouseMove += Rectangle_MouseMove;
            rectangle.MouseUp += Rectangle_MouseUp;

            gameBoard.Children.Add(rectangle);
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle)
            {
                isDragging = true;
                clickPosition = e.GetPosition(gameBoard);
                rectangle.CaptureMouse();
                if (!isHorizontal)
                    mainWindow.RemoveRec(currentRowOrColumn, Column, RowSpan + ColumnSpan - 1, isHorizontal);
                else
                    mainWindow.RemoveRec(Row, currentRowOrColumn, RowSpan + ColumnSpan - 1, isHorizontal);

            }
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && sender is Rectangle)
            {
                if (isHorizontal)
                {
                    Point currentPosition = e.GetPosition(gameBoard);
                    double offsetX = currentPosition.X - clickPosition.X;
                    double newLeft = transform.X + offsetX;

                    // Calculate grid-based boundaries
                    double cellWidth = gameBoard.ColumnDefinitions[0].ActualWidth;
                    double currentLeft = currentRowOrColumn * cellWidth + newLeft;

                    // Allow movement up to the exit (right side beyond the last column)
                    double maxLeft = cellWidth * (gameBoard.ColumnDefinitions.Count - 1) - ColumnSpan * 50;

                    int newColumn = currentRowOrColumn + (int)(offsetX / 100) + ColumnSpan;
                    if (currentLeft >= 0 && currentLeft <= maxLeft + cellWidth)
                    {
                        if (mainWindow.boardModel.IsMoveValid(Row, newColumn))
                        {
                            transform.X += offsetX;
                        }
                    }

                    clickPosition = currentPosition;
                }
                else
                {
                    Point currentPosition = e.GetPosition(gameBoard);
                    double offsetY = currentPosition.Y - clickPosition.Y;
                    double newTop = transform.Y + offsetY;

                    // Calculate grid-based boundaries
                    double cellHeight = gameBoard.RowDefinitions[0].ActualHeight;
                    double currentTop = currentRowOrColumn * cellHeight + newTop;

                    // Allow movement up to the exit (bottom side beyond the last row)
                    double maxTop = cellHeight * (gameBoard.RowDefinitions.Count - 1) - RowSpan * 50;

                    int newRow = currentRowOrColumn + (int)(offsetY / 100);
                    if (currentTop >= 0 && currentTop <= maxTop + cellHeight)
                    {
                        if (mainWindow.boardModel.IsMoveValidRec(currentRowOrColumn, Column, RowSpan , isHorizontal))
                        {
                            transform.Y += offsetY;
                        }
                    }

                    clickPosition = currentPosition;
                }
            }
        }
        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle)
            {
                if (isHorizontal)
                {
                    isDragging = false;
                    rectangle.ReleaseMouseCapture();

                    // Snap to the nearest grid cell
                    double cellWidth = gameBoard.ColumnDefinitions[0].ActualWidth;
                    double currentLeft = currentRowOrColumn * cellWidth + transform.X;
                    int nearestColumn = (int)Math.Round(currentLeft / cellWidth);

                    // Allow the block to reach the exit (right side beyond the last column)
                    nearestColumn = Math.Max(0, Math.Min(nearestColumn, gameBoard.ColumnDefinitions.Count));

                    // Update current column position
                    currentRowOrColumn = nearestColumn;
                    transform.X = 0;
                    mainWindow.OnBlockMove(Row, currentRowOrColumn, "Blue" , ColumnSpan, isHorizontal);
                    Grid.SetColumn(rectangle, currentRowOrColumn);
                }
                else
                {
                    isDragging = false;
                    rectangle.ReleaseMouseCapture();

                    // Snap to the nearest grid cell
                    double cellHeight = gameBoard.RowDefinitions[0].ActualHeight;
                    double currentTop = currentRowOrColumn * cellHeight + transform.Y;
                    int nearestRow = (int)Math.Round(currentTop / cellHeight);

                    // Allow the block to reach the exit (bottom side beyond the last row)
                    nearestRow = Math.Max(0, Math.Min(nearestRow, gameBoard.RowDefinitions.Count));

                    // Update current row position
                    currentRowOrColumn = nearestRow;
                    transform.Y = 0;
                    mainWindow.OnBlockMove(currentRowOrColumn, Column, "Blue" , RowSpan , isHorizontal);
                    Grid.SetRow(rectangle, currentRowOrColumn);
                }
            }
        }
        
    }
}