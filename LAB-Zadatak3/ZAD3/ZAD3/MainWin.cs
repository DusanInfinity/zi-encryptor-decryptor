using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ZAD1.App;
using ZAD2.App;

namespace ZAD1
{
    public partial class MainWin : Form
    {
        private readonly FileLogic fLogic;
        private readonly AppConfig config;
        private int currentAlgorithmIndex = 0;
        private bool crtActive = false;

        public MainWin()
        {
            InitializeComponent();

            fLogic = new FileLogic(new RC4(), tbOdrediste.Text, false);
            cBoxIzborAlgoritma.Items.AddRange(new[] { "RC4", "XTEA" });
            cBoxIzborAlgoritma.SelectedIndex = 0;

            rbFSWNeaktivan.Checked = true;
            rbCRTNeaktivan.Checked = true;

            config = AppConfig.UcitajKonfiguraciju();
            SetIzvor(config.Izvor);
            SetOdrediste(config.Odrediste);
            SetAlgorithm(config.Algoritam);
            SetCRTActive(config.CRTActive);
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

        private static CipherAlgorithm GetAlgoritmInstanceByIndex(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        return new RC4();
                    }
                case 1:
                    {
                        return new XTEA();
                    }
                default:
                    return null;
            }
        }

        private void SetAlgorithm(int algorithmIndex)
        {
            if (currentAlgorithmIndex == algorithmIndex) return;


            CipherAlgorithm algorithm = GetAlgoritmInstanceByIndex(algorithmIndex);
            if (algorithm == null) return;

            fLogic.SetAlgorithm(algorithm);
            fLogic.ToggleCRTMode(crtActive);
            currentAlgorithmIndex = algorithmIndex;

            if (config.Algoritam != algorithmIndex)
            {
                config.Algoritam = algorithmIndex;
                config.SacuvajKonfiguraciju();
            }
            else
            {
                // Dolazi do izvesenja ovog koda u slucaju da je ucitan algoritam iz konfiguracije
                if (algorithmIndex < cBoxIzborAlgoritma.Items.Count)
                    cBoxIzborAlgoritma.SelectedIndex = algorithmIndex;
            }
        }

        private void SetCRTActive(bool active)
        {
            if (crtActive == active) return;

            fLogic.ToggleCRTMode(active);
            crtActive = active;

            if (config.CRTActive != active)
            {
                config.CRTActive = active;
                config.SacuvajKonfiguraciju();
            }
            else
            {
                // Dolazi do izvesenja ovog koda u slucaju da je ucitan algoritam iz konfiguracije
                rbCRTAktivan.Checked = true; // algoritam je sigurno aktivan zato sto po defaultu algoritam je neaktivan
            }
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

                cBoxIzborAlgoritma.Enabled = false;

                rbCRTAktivan.Enabled = false;
                rbCRTNeaktivan.Enabled = false;
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

                cBoxIzborAlgoritma.Enabled = true;

                rbCRTAktivan.Enabled = true;
                rbCRTNeaktivan.Enabled = true;
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
                string decryptedText;
                try
                {
                    decryptedText = fLogic.DecryptFile(openFileDiag.FileName);
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                    return;
                }

                ShowInfo("Uspesno dekodiranje", $"Uspesno ste izvršili dekodiranje fajla: {openFileDiag.FileName}.\nSada izaberite lokaciju za cuvanje fajla.");

                saveFileDiag.FileName = $"{Path.GetFileNameWithoutExtension(openFileDiag.FileName)}Decrypted.txt";
                if (saveFileDiag.ShowDialog() == DialogResult.OK)
                    fLogic.SaveTextToFile(decryptedText, saveFileDiag.FileName);
            }
        }

        private void fsWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                lblStatus.Text = $"[FSW] Pronasao sam fajl {e.Name}";
                if (File.Exists(e.FullPath))
                {
                    CipherAlgorithm algorithm = GetAlgoritmInstanceByIndex(currentAlgorithmIndex);
                    if (algorithm == null) return;

                    FileLogic logic = new FileLogic(algorithm, tbOdrediste.Text, crtActive);
                    logic.EncryptFile(e.FullPath, tbOdrediste.Text);
                    lblStatus.Text = $"[FSW - {algorithm.GetType().Name}] Enkripcija fajla {e.Name} zavrsena!";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Exception: {ex.Message}";
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

        private void cBoxIzborAlgoritma_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;

            SetAlgorithm(cmb.SelectedIndex);
        }

        private void rbCRTAktivan_CheckedChanged(object sender, EventArgs e)
        {
            SetCRTActive(rbCRTAktivan.Checked);
        }
    }
}
