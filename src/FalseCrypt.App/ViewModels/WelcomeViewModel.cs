﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using FalseCrypt.Crypto;
using ModernApplicationFramework.Controls.Dialogs.Native;
using ModernApplicationFramework.Input.Command;

namespace FalseCrypt.App.ViewModels
{
    [Export(typeof(WelcomeViewModel))]
    public class WelcomeViewModel : Screen
    {
        public ICommand EncryptFileCommand => new Command(EncryptFile, () => true);

        public ICommand EncryptFolderCommand => new Command(EncryptFolder, () => true);

        public ICommand DecryptFileCommand => new Command(DecryptFile, () => true);

        public ICommand DecryptFolderCommand => new Command(DecryptFolder, () => true);

        private void EncryptFile()
        {
            var files = GetFiles();
            if (files == null || !files.Any())
                return;

            var password = ShowPasswordEnter();
            if (string.IsNullOrEmpty(password))
                return;

            foreach (var file in files)
            {
                if (!File.Exists(file))
                    continue;
                EncryptionCryptoWrapper.EncryptFileWithPassword(new FileInfo(file), password);
            }

            MessageBox.Show("Successfully encrypted");
        }

        private void EncryptFolder()
        {
            var folder = GetFolder();
            if (string.IsNullOrEmpty(folder))
                return;

            var password = ShowPasswordEnter();
            if (string.IsNullOrEmpty(password))
                return;

            var files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
            var keyData = WeakPasswordDerivation.DerivePassword(password);

            foreach (var file in files)
            {
                if (!File.Exists(file))
                    continue;

               
                EncryptionCryptoWrapper.EncryptFile(new FileInfo(file), keyData.Key, keyData.Salt);
            }
            MessageBox.Show("Successfully encrypted");
        }

        private void DecryptFile()
        {
            var files = GetFiles("falsecrypt File (*.falsecrypt)|*.falsecrypt");
            if (files == null || !files.Any())
                return;

            var password = ShowPasswordEnter();
            if (string.IsNullOrEmpty(password))
                return;

            foreach (var file in files)
            {
                if (!File.Exists(file))
                    continue;
                try
                {
                    EncryptionCryptoWrapper.DecryptFileWithPassword(new FileInfo(file), password);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Wrong password");
                    return;
                }    
            }
            MessageBox.Show("Successfully decrypted");
        }

        private void DecryptFolder()
        {
            var folder = GetFolder();
            if (string.IsNullOrEmpty(folder))
                return;

            var password = ShowPasswordEnter();
            if (string.IsNullOrEmpty(password))
                return;

            var files = Directory.GetFiles(folder, "*.falsecrypt", SearchOption.AllDirectories);

            var keyData = WeakPasswordDerivation.DerivePassword(password);

            foreach (var file in files)
            {
                if (!File.Exists(file))
                    continue;
                try
                {
                    EncryptionCryptoWrapper.DecryptFile(new FileInfo(file), keyData.Key);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Wrong password");
                    return;
                }
                
            }

            MessageBox.Show("Successfully decrypted");
        }

        private static IReadOnlyCollection<string> GetFiles(string filter = null)
        {
            var fd = new NativeOpenFileDialog
            {
                Multiselect = true,
                CheckFileExists = true,
            };

            if (!string.IsNullOrEmpty(filter))
                fd.Filter = filter;

            var result = fd.ShowDialog();
            if (result == null || !result.Value)
                return null;
            return fd.FileNames;
        }

        private static string GetFolder()
        {
            var fd = new NativeFolderBrowserDialog();
            var result = fd.ShowDialog();
            if (result == null || !result.Value)
                return null;

            return fd.SelectedPath;
        }

        private string ShowPasswordEnter()
        {
            var wm = IoC.Get<IWindowManager>();
            var selectionViewModel = IoC.Get<EnterPasswordViewModel>();
            wm.ShowDialog(selectionViewModel);
            return selectionViewModel.Password;
        }
    }
}
