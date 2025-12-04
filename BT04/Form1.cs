using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT04
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // ToolTip cho các control
            toolTip1.SetToolTip(btnBrowseSource, "Chọn file nguồn để sao chép");
            toolTip1.SetToolTip(txtDest, "Chọn thư mục đích");
            toolTip1.SetToolTip(btnCopy, "Nhấn để sao chép file");

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
        }

        private void btnBrowseSource_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Chọn file nguồn";
            ofd.Filter = "Tất cả tập tin|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
                txtSource.Text = ofd.FileName;
        }

        private void btnBrowseDest_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Chọn thư mục đích";

            if (fbd.ShowDialog() == DialogResult.OK)
                txtDest.Text = fbd.SelectedPath;
        }

        private async void btnCopy_Click(object sender, EventArgs e)
        {
            string src = txtSource.Text;
            string destFolder = txtDest.Text;

            if (!File.Exists(src))
            {
                MessageBox.Show("File nguồn không tồn tại!");
                return;
            }
            if (!Directory.Exists(destFolder))
            {
                MessageBox.Show("Thư mục đích không tồn tại!");
                return;
            }

            string fileName = Path.GetFileName(src);
            string dest = Path.Combine(destFolder, fileName);

            progressBar1.Value = 0;
            lblStatus.Text = "Đang sao chép: " + fileName;
            toolTip1.SetToolTip(progressBar1, "Đang sao chép...");

            await CopyFileAsync(src, dest);

            progressBar1.Value = 100;
            lblStatus.Text = "Hoàn thành!";
            toolTip1.SetToolTip(progressBar1, "Hoàn tất sao chép");
            MessageBox.Show("Sao chép thành công!");
        }

        private async Task CopyFileAsync(string source, string dest)
        {
            const int bufferSize = 1024 * 1024; // 1MB
            byte[] buffer = new byte[bufferSize];
            string fileName = Path.GetFileName(source);

            using (FileStream sourceStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            using (FileStream destStream = new FileStream(dest, FileMode.Create))
            {
                long totalBytes = sourceStream.Length;
                long totalCopied = 0;

                int bytesRead;
                while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await destStream.WriteAsync(buffer, 0, bytesRead);
                    totalCopied += bytesRead;

                    int percent = (int)(totalCopied * 100 / totalBytes);
                    if (percent > 100) percent = 100;

                    progressBar1.Value = percent;
                    lblStatus.Text = $"Đang sao chép: {fileName} - {percent}%";

                    toolTip1.SetToolTip(progressBar1, lblStatus.Text);

                    await Task.Delay(50); // để progress bar đi từ từ
                }
            }
        }

    }
}
