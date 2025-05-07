using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace media_tracker_desktop
{
    public static class AppElement
    {
        public static void AddFavoriteButtonColumn(DataGridView dataGridView)
        {
            DataGridViewButtonColumn favoriteButtons = new DataGridViewButtonColumn();

            // Add the button properties.
            favoriteButtons.Name = "btnFavorite";
            // Header text is displayed as the column title.
            favoriteButtons.HeaderText = " ";
            favoriteButtons.FlatStyle = FlatStyle.Popup;

            // Add the button column to the data grid if it isn't already.
            if (!dataGridView.Columns.Contains("btnFavorite"))
            {
                dataGridView.Columns.Add(favoriteButtons);
            }
        } 

        public static Panel GetSearchAndSortPanel()
        {
            Panel pnlSearchAndSort = new Panel();

            // Properties:
            pnlSearchAndSort.Dock = DockStyle.Top;
            pnlSearchAndSort.Size = new Size(669, 60);
            pnlSearchAndSort.BackColor = Color.FromArgb(45, 45, 48);

            AddSearchBar(pnlSearchAndSort);
            AddSortButton(pnlSearchAndSort);

            return pnlSearchAndSort;
        }

        // Method: Add search bar for the panel.
        private static void AddSearchBar(Panel panel)
        {
            TextBox txtSearch = new TextBox();

            // Properties:
            txtSearch.Location = new Point(15, 15);
            txtSearch.Width = 350;
            txtSearch.Name = "txtSearch";

            // Add to panel.
            panel.Controls.Add(txtSearch);
        }

        private static void AddSortButton(Panel panel)
        {
            Button btnSort = new Button();

            // Properties:
            btnSort.Location = new Point(400, 15);
            btnSort.Text = "Sort Menu";
            btnSort.AutoSize = true;
            btnSort.BackColor = Color.White;
            btnSort.Name = "btnSort";

            // Add to panel.
            panel.Controls.Add(btnSort);
        }

        public static ContextMenuStrip GetSortMenu(string[] sortOptions)
        {
            ContextMenuStrip sortMenu = new ContextMenuStrip();


            foreach (string option in sortOptions)
            {
                sortMenu.Items.Add(new ToolStripMenuItem(option));
            }

            return sortMenu;
        }

        public static void AddMainIcon(Button btn, Image icon, Size size)
        {
            Image resizedImage = new Bitmap(icon, size);

            btn.Image = resizedImage;
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(30, 0, 0, 0);
        }
    }
}
