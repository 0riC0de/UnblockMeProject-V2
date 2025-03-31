using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace UnblockMeProject
{
    public class RedBlock
    {
        private Rectangle rectangle;
        private bool isDragging = false;
        private Point clickPosition;
        private TranslateTransform transform = new TranslateTransform();
        private Grid gameBoard;
        private int currentColumn;
        private MainWindow mainWindow;


        public RedBlock(Grid gameBoard , MainWindow mainWindow)
        {
            this.gameBoard = gameBoard;
            this.mainWindow = mainWindow;
            this.currentColumn = 0; // Start at column 2

            rectangle = new Rectangle
            {
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Width = 100, // 2 columns width
                Height = 50,  // 1 row height
                Name = "RedBlockName",
                Tag = "RedBlockTag"
            };

            // Initial position
            Grid.SetRow(rectangle, 2);
            Grid.SetColumn(rectangle, currentColumn);
            Grid.SetColumnSpan(rectangle, 2);

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
                mainWindow.RemoveRec(2 , currentColumn, 2 , true);
                    
            }
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && sender is Rectangle)
            {
                Point currentPosition = e.GetPosition(gameBoard);
                double offsetX = currentPosition.X - clickPosition.X;
                double newLeft = transform.X + offsetX;

                // Calculate grid-based boundaries
                double cellWidth = gameBoard.ColumnDefinitions[0].ActualWidth;
                double currentLeft = currentColumn * cellWidth + newLeft;

                // Allow movement up to the exit (right side beyond the last column)
                double maxLeft = cellWidth * (gameBoard.ColumnDefinitions.Count - 1);

                // Ensure the block can move outside the grid bounds on the right side
                int newColumn = currentColumn + (int)(offsetX / 100) + 1;
                System.Diagnostics.Debug.WriteLine(newColumn);
                if (currentLeft >= 0 && currentLeft <= maxLeft + cellWidth)
                {
                    if (offsetX > 0 && mainWindow.boardModel.IsMoveValid(2, newColumn + 1))
                    {
                        transform.X += offsetX;
                    }
                    else if (offsetX < 0 && mainWindow.boardModel.IsMoveValid(2, newColumn - 2))
                    {
                        transform.X += offsetX;
                    }
                    else
                      Console.WriteLine("NotValid");
                }

                clickPosition = currentPosition;
            }
        }

        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle)
            {
                isDragging = false;
                rectangle.ReleaseMouseCapture();

                // Snap to the nearest grid cell
                double cellWidth = gameBoard.ColumnDefinitions[0].ActualWidth;
                double currentLeft = currentColumn * cellWidth + transform.X;
                int nearestColumn = (int)Math.Round(currentLeft / cellWidth);

                // Allow the block to reach the exit (right side beyond the last column)
                nearestColumn = Math.Max(0, Math.Min(nearestColumn, gameBoard.ColumnDefinitions.Count));

                // Update current column position
                currentColumn = nearestColumn;
                transform.X = 0;
                if(mainWindow.OnBlockMove(2, currentColumn + 1, "Red" , 2 , true))
                    Grid.SetColumn(rectangle, currentColumn);

                // Check if the block has reached the exit (right side beyond the last column)
                if (currentColumn == gameBoard.ColumnDefinitions.Count)
                {
                    MessageBox.Show("Congratulations! You've solved the puzzle!");
                }
            }
        }
    }
}