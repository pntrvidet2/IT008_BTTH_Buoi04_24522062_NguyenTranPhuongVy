using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT02
{
    public partial class Form1 : Form
    {
        private FontDialog fontDialog = new FontDialog();

        private string currentFile = "";
        public Form1()
        {
            InitializeComponent();


        }
        private void LoadSizes()
        {
            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int s in sizes)
            {
                toolStripSize.Items.Add(s);
            }
            int defaultIndex = Array.IndexOf(sizes, 14);
            if (defaultIndex >= 0)
                toolStripSize.SelectedIndex = defaultIndex;
        }
        private void LoadFonts()
        {
            foreach (FontFamily font in FontFamily.Families)
            {
                toolStripFont.Items.Add(font.Name);
            }
            toolStripFont.SelectedItem = "Tahoma"; // Default font Arial
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFonts();
            LoadSizes();
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;     
            exitToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
        }
       

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbContent.Clear();
            rtbContent.Font = new Font("Tahoma", 12);
            toolStripSize.SelectedItem = 12;
            toolStripFont.SelectedItem = "Tahoma";
            currentFile = "";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Rich Text Format (*.rtf)|*.rtf|Text files (*.txt)|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.EndsWith(".rtf"))
                    rtbContent.LoadFile(ofd.FileName, RichTextBoxStreamType.RichText);
                else
                    rtbContent.Text = System.IO.File.ReadAllText(ofd.FileName);
                currentFile = ofd.FileName;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFile))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Rich Text Format (*.rtf)|*.rtf";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Kiểm tra file đã tồn tại
                    if (System.IO.File.Exists(sfd.FileName))
                    {
                        DialogResult dr = MessageBox.Show(
                            "File này đã tồn tại. Bạn có muốn ghi đè không?",
                            "Xác nhận ghi đè",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (dr == DialogResult.No)
                        {
                            return; // không ghi đè, thoát
                        }
                    }

                    rtbContent.SaveFile(sfd.FileName, RichTextBoxStreamType.RichText);
                    currentFile = sfd.FileName;
                    MessageBox.Show("Đã lưu thành công!");
                }
            }
            else
            {
                // Nếu file đã mở trước đó
                rtbContent.SaveFile(currentFile, RichTextBoxStreamType.RichText);
                MessageBox.Show("Đã lưu thành công!");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
                Application.Exit();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                rtbContent.SelectionFont = fd.Font;
                toolStripSize.SelectedItem = (int)fd.Font.Size;
                toolStripFont.SelectedItem = fd.Font.Name;
            }
        }

        private void toolStripFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripFont.SelectedItem != null)
            {
                string fontName = toolStripFont.SelectedItem.ToString();
                float fontSize = rtbContent.SelectionFont?.Size ?? 12;
                rtbContent.SelectionFont = new Font(fontName, fontSize);
            }
        }

        private void toolStripSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripSize.SelectedItem != null)
            {
                float fontSize = float.Parse(toolStripSize.SelectedItem.ToString());
                string fontName = rtbContent.SelectionFont?.Name ?? "Tahoma";
                rtbContent.SelectionFont = new Font(fontName, fontSize);
            }
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Font currentFont = rtbContent.SelectionFont;
            if (currentFont != null)
            {
                FontStyle newFontStyle = currentFont.Style ^ FontStyle.Bold;
                rtbContent.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Font currentFont = rtbContent.SelectionFont;
            if (currentFont != null)
            {
                FontStyle newFontStyle = currentFont.Style ^ FontStyle.Italic;
                rtbContent.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
        }

        private void underlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Font currentFont = rtbContent.SelectionFont;
            if (currentFont != null)
            {
                FontStyle newFontStyle = currentFont.Style ^ FontStyle.Underline;
                rtbContent.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }

        }

        private void ToggleStrikeThrough(object sender, EventArgs e)
        {
            Font currentFont = rtbContent.SelectionFont;
            if (currentFont != null)
            {
                FontStyle newFontStyle = currentFont.Style ^ FontStyle.Strikeout;
                rtbContent.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
        }

        private void rtbContent_TextChanged(object sender, EventArgs e)
        {

        }
    }
}


