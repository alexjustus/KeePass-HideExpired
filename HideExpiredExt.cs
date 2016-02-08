﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KeePass;
using KeePass.Plugins;
using KeePass.UI;
using KeePassLib;

namespace HideExpired
{
    /// <summary>
    /// KeePass plugin which hides expired entries in list views.
    /// </summary>
    public class HideExpiredExt : Plugin
    {
        private IPluginHost host;
        private ToolStripMenuItem menuItem;
        private bool isChecked = false;

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        /// <returns>true if the plugin was initialised successfully; otherwise, false.</returns>
        public override bool Initialize(IPluginHost host)
        {
            this.host = host;

            // Get a reference to the 'Edit' menu item container.
            var menuSearch = this.host.MainWindow.MainMenu.Items.Find("m_menuEdit", false);
            if (menuSearch.Length == 0)
                return false;

            var editMenu = menuSearch[0] as ToolStripDropDownItem;
            if (editMenu == null)
                return false;

            // Find the 'Show All Expired Entries' item.
            var index = -1;
            for (int i = 0; i < editMenu.DropDownItems.Count - 1; i++)
            {
                if (editMenu.DropDownItems[i].Name.Equals("m_menuEditShowExpired", StringComparison.OrdinalIgnoreCase))
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
                return false;

            // Add new menu entry after the 'Show Expired' item.
            this.menuItem = new ToolStripMenuItem();
            this.menuItem.Text = "Hide Expired Entries";
            this.menuItem.Click += this.OnMenuHideExpired;
            editMenu.DropDownItems.Insert(index + 1, this.menuItem);

            return true;
        }

        /// <summary>
        /// Performs clean up when the host terminates the plugin.
        /// </summary>
        public override void Terminate()
        {
            // Remove our event handlers.
            this.menuItem.Click -= this.OnMenuHideExpired;
        }

        /// <summary>
        /// Event handler for when the Hide Expired menu item is clicked.
        /// </summary>
        private void OnMenuHideExpired(object sender, EventArgs e)
        {
            if (this.isChecked)
                this.isChecked = false;
            else
                this.isChecked = true;

            UIUtil.SetChecked(this.menuItem, isChecked);
            this.host.MainWindow.UpdateUI(false, null, false, null, true, null, false);
        }
    }
}