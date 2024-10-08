﻿using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsOOBERecreation
{
    public partial class Password : Form
    {
        private Main _mainForm;
        private string _username;
        private string _computerName;

        public Password(Main mainForm, string username, string computerName)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _username = username;
            _computerName = computerName;

            passwordBox.TextChanged += ValidateInput;
            confpasswordBox.TextChanged += ValidateInput;
            passwordHintBox.TextChanged += ValidateInput;
            nextButton.Enabled = true;
        }

        private void ValidateInput(object sender, EventArgs e)
        {
            string password = passwordBox.Text;
            string confirmPassword = confpasswordBox.Text;
            string passwordHint = passwordHintBox.Text;

            // Conditions to enable the next button:
            // 1. If all are empty, allow skipping.
            // 2. If password is entered but no confirmation or hint, do not allow.
            // 3. If password and confirmation are entered but no hint, do not allow.
            // 4. If all fields are filled, allow the button.
            if (string.IsNullOrEmpty(password) && string.IsNullOrEmpty(confirmPassword) && string.IsNullOrEmpty(passwordHint))
            {
                nextButton.Enabled = true;
            }
            else if (!string.IsNullOrEmpty(password) && password == confirmPassword && !string.IsNullOrEmpty(passwordHint))
            {
                nextButton.Enabled = true;
            }
            else
            {
                nextButton.Enabled = false;
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            string password = passwordBox.Text;
            string confirmPassword = confpasswordBox.Text;

            try
            {
                if (string.IsNullOrEmpty(password) && string.IsNullOrEmpty(confirmPassword))
                {
                    ExecuteCommand($"net user \"{_username}\" /add");
                }
                else if (password == confirmPassword)
                {
                    ExecuteCommand($"net user \"{_username}\" \"{password}\" /add");
                }

                ExecuteCommand($"net localgroup Administrators /add \"{_username}\"");
                ChangeComputerName(_computerName);

                ProductKey ProductKeyForm = new ProductKey(_mainForm);
                _mainForm.LoadFormIntoPanel(ProductKeyForm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during account creation: {ex.Message}");

                ProductKey ProductKeyForm = new ProductKey(_mainForm);
                _mainForm.LoadFormIntoPanel(ProductKeyForm);
            }
        }

        private void ExecuteCommand(string command)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }
            }
        }

        private void ChangeComputerName(string computerName)
        {
            try
            {
                ExecuteCommand($"WMIC computersystem where name=\"%COMPUTERNAME%\" call rename name=\"{computerName}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while changing the computer name: {ex.Message}");
            }
        }
    }
}
