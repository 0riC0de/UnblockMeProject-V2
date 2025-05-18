using System;
using System.Windows;
using System.Windows.Controls;

namespace UnblockMeProject
{
    public partial class BoardSelectionDialog : Window
    {
        public int SelectedBoard { get; private set; } = 1; // Default to board 1
        public int SelectedDepth { get; private set; } = 3; // Default to depth 3

        public BoardSelectionDialog()
        {
            InitializeComponent();

            // Use Loaded event to safely initialize after XAML rendering is complete
            this.Loaded += BoardSelectionDialog_Loaded;
        }

        private void BoardSelectionDialog_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize the ComboBox selection
            BoardComboBox.SelectedIndex = 0;

            // Update the description for the default selection
            UpdateBoardDescription(0);
        }

        private void BoardComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selectedIndex = BoardComboBox.SelectedIndex;

                // Hide all images first
                HideAllImages();

                // Show the selected image
                ShowSelectedImage(selectedIndex);

                // Update the description
                UpdateBoardDescription(selectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating selection: " + ex.Message);
            }
        }

        private void HideAllImages()
        {
            // Check each image before hiding to avoid null reference
            if (Board1Image != null) Board1Image.Visibility = Visibility.Collapsed;
            if (Board2Image != null) Board2Image.Visibility = Visibility.Collapsed;
            if (Board3Image != null) Board3Image.Visibility = Visibility.Collapsed;
            if (Board4Image != null) Board4Image.Visibility = Visibility.Collapsed;
            if (Board5Image != null) Board5Image.Visibility = Visibility.Collapsed;
            if (Board6Image != null) Board6Image.Visibility = Visibility.Collapsed;
        }

        private void ShowSelectedImage(int selectedIndex)
        {
            try
            {
                switch (selectedIndex)
                {
                    case 0:
                        if (Board1Image != null) Board1Image.Visibility = Visibility.Visible;
                        break;
                    case 1:
                        if (Board2Image != null) Board2Image.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        if (Board3Image != null) Board3Image.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        if (Board4Image != null) Board4Image.Visibility = Visibility.Visible;
                        break;
                    case 4:
                        if (Board5Image != null) Board5Image.Visibility = Visibility.Visible;
                        break;
                    case 5:
                        if (Board6Image != null) Board6Image.Visibility = Visibility.Visible;
                        break;
                }
            }
            catch (Exception ex)
            {
                // Show fallback text if there's an error
                if (FallbackText != null)
                {
                    FallbackText.Text = "Error showing preview: " + ex.Message;
                    FallbackText.Visibility = Visibility.Visible;
                }
            }
        }

        private void UpdateBoardDescription(int boardIndex)
        {
            if (BoardDescriptionText == null) return;

            // Set the board description
            switch (boardIndex)
            {
                case 0:
                    BoardDescriptionText.Text = "";
                    break;
                case 1:
                    BoardDescriptionText.Text = "Hard! Use Depth 3";
                    break;
                case 2:
                    BoardDescriptionText.Text = "Use depth 1";
                    break;
                case 3:
                    BoardDescriptionText.Text = "Use Depth 1";
                    break;
                case 4:
                    BoardDescriptionText.Text = "Use Depth 1";
                    break;
                case 5:
                    BoardDescriptionText.Text = "Use Depth 1";
                    break;

                default:
                    BoardDescriptionText.Text = "";
                    break;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedBoard = BoardComboBox.SelectedIndex + 1;
            SelectedDepth = (int)DepthSlider.Value;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}