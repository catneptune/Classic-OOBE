﻿using System;
using System.Windows.Forms;

namespace WindowsOOBERecreation
{
    public partial class Main : Form
    {
        private Panel mainPanel;
        public Main()
        {
            InitializeComponent();

            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            this.Controls.Add(mainPanel);

            LoadStartForm();
        }

        public void LoadFormIntoPanel(Form form)
        {
            mainPanel.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            mainPanel.Controls.Add(form);
            form.Show();
        }

        private void LoadStartForm()
        {
            Start startForm = new Start(this);
            LoadFormIntoPanel(startForm);
        }

        private void LoadPasswordForm()
        {
            Password passwordForm = new Password(this);
            LoadFormIntoPanel(passwordForm);
        }

        private void LoadTimeAndDateForm()
        {
            TimeAndDate timeAndDateForm = new TimeAndDate(this);
            LoadFormIntoPanel(timeAndDateForm);
        }

    }
}
