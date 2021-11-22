using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ZAD1.App;

namespace ZAD1
{
    public partial class MainWin : Form
    {
        private readonly FileLogic fLogic;
        private readonly AppConfig config;

        public MainWin()
        {
            InitializeComponent();

            fLogic = new FileLogic(tbOdrediste.Text);

            rbFSWNeaktivan.Checked = true;

            config = AppConfig.UcitajKonfiguraciju();
            SetIzvor(config.Izvor);
            SetOdrediste(config.Odrediste);
        }

        private void SetIzvor(string path)
        {
            if (!Directory.Exists(path)) return;

            tbIzvor.Text = path;
            fsWatcher.Path = tbIzvor.Text;
        }

        private void SetOdrediste(string path)
        {
            if (!Directory.Exists(path) || path == tbIzvor.Text) return;

            tbOdrediste.Text = path;
            fLogic.SetKeysPath(path);
        }

        private void btnIzmeniFSPutanju_Click(object sender, System.EventArgs e)
        {
            folderBrowser.SelectedPath = tbIzvor.Text;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowser.SelectedPath;
                if (!string.IsNullOrEmpty(tbIzvor.Text))
                {
                    if (tbOdrediste.Text == path)
                    {
                        ShowError("Odredisni folder ne moze biti ujedno i izvorni folder!");
                        return;
                    }
                }

                SetIzvor(path);
                config.Izvor = path;
                config.SacuvajKonfiguraciju();
            }
        }

        private void btnIzmeniOdrediste_Click(object sender, System.EventArgs e)
        {
            folderBrowser.SelectedPath = tbOdrediste.Text;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowser.SelectedPath;
                if (!string.IsNullOrEmpty(tbIzvor.Text))
                {
                    if (tbIzvor.Text == path)
                    {
                        ShowError("Izvorni folder ne moze biti ujedno i odredisni folder!");
                        return;
                    }
                }

                SetOdrediste(path);
                config.Odrediste = path;
                config.SacuvajKonfiguraciju();
            }
        }

        private void rbFSWAktivan_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbFSWAktivan.Checked)
            {
                if (!Directory.Exists(tbIzvor.Text))
                {
                    rbFSWNeaktivan.Checked = true;
                    ShowError("Izabrani direktorijum za Izvor FSW ne postoji!");
                    return;
                }
                if (!Directory.Exists(tbOdrediste.Text))
                {
                    rbFSWNeaktivan.Checked = true;
                    ShowError("Izabrani direktorijum za Odrediste FSW ne postoji!");
                    return;
                }


                rbFSWAktivan.BackColor = Color.DarkGreen;
                rbFSWAktivan.ForeColor = Color.White;
                rbFSWNeaktivan.BackColor = System.Drawing.SystemColors.Control;
                rbFSWNeaktivan.ForeColor = Color.DarkRed;

                fsWatcher.EnableRaisingEvents = true;
                fsWatcher.Path = tbIzvor.Text;
                btnDekodiranje.Enabled = false;
                btnKodiranje.Enabled = false;

                btnIzmeniIzvor.Enabled = false;
                btnIzmeniOdrediste.Enabled = false;
            }
            else if (rbFSWNeaktivan.Checked)
            {
                rbFSWAktivan.BackColor = System.Drawing.SystemColors.Control;
                rbFSWAktivan.ForeColor = Color.DarkGreen;
                rbFSWNeaktivan.BackColor = Color.DarkRed;
                rbFSWNeaktivan.ForeColor = Color.White;

                fsWatcher.EnableRaisingEvents = false;
                btnDekodiranje.Enabled = true;
                btnKodiranje.Enabled = true;

                btnIzmeniIzvor.Enabled = true;
                btnIzmeniOdrediste.Enabled = true;
            }
        }

        private void btnKodiranje_Click(object sender, System.EventArgs e)
        {
            if (!Directory.Exists(tbOdrediste.Text))
            {
                ShowError("Niste izabrali validan odredisni folder!");
                return;
            }

            openFileDiag.InitialDirectory = tbIzvor.Text;
            if (openFileDiag.ShowDialog() == DialogResult.OK)
                fLogic.EncryptFile(openFileDiag.FileName, tbOdrediste.Text);
        }

        private void btnDekodiranje_Click(object sender, System.EventArgs e)
        {
            if (!Directory.Exists(tbOdrediste.Text))
            {
                ShowError("Niste izabrali validan odredisni folder koji je neophodan zbog lokacije ključa!");
                return;
            }

            openFileDiag.InitialDirectory = tbIzvor.Text;
            if (openFileDiag.ShowDialog() == DialogResult.OK)
            {
                string decryptedText = fLogic.DecryptFile(openFileDiag.FileName);

                ShowInfo("Uspesno dekodiranje", $"Uspesno ste izvršili dekodiranje fajla: {openFileDiag.FileName}.\nSada izaberite lokaciju za cuvanje fajla.");

                saveFileDiag.FileName = $"{Path.GetFileNameWithoutExtension(openFileDiag.FileName)}Decrypted.txt";
                if (saveFileDiag.ShowDialog() == DialogResult.OK)
                    FileLogic.SaveTextToFile(decryptedText, saveFileDiag.FileName);
            }
        }

        private void fsWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                lblStatus.Text = $"[FSW] Pronasao sam fajl {e.Name}";
                if (File.Exists(e.FullPath))
                {
                    FileLogic logic = new FileLogic(tbOdrediste.Text);
                    logic.EncryptFile(e.FullPath, tbOdrediste.Text);
                    lblStatus.Text = $"[FSW] Enkripcija fajla {e.Name} zavrsena!";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
            }
        }

        private void ShowError(string text)
        {
            MessageBox.Show(this, text, "GRESKA", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowInfo(string title, string text)
        {
            MessageBox.Show(this, text, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
