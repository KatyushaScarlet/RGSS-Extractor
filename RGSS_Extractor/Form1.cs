using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RGSS_Extractor
{
	public class Form1 : Form
	{
		private Main_Parser parser = new Main_Parser();

		private List<Entry> entries = new List<Entry>();

		private string archive_path = "";

		private IContainer components;

		private SplitContainer splitContainer1;

		private TreeView explorer_view;

		private OpenFileDialog openFileDialog1;

		private MenuStrip menuStrip1;

		private ToolStripMenuItem fileToolStripMenuItem;

		private ToolStripMenuItem openToolStripMenuItem;

		private ToolStripMenuItem closeArchiveToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem exitToolStripMenuItem;

		private PictureBox pic_preview;

		private ToolStripMenuItem exportArchiveToolStripMenuItem;

		private ContextMenuStrip explorer_menu;

		private ToolStripMenuItem exportToolStripMenuItem;

		public Form1()
		{
			this.InitializeComponent();
		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			this.read_archive(array[0]);
		}

		private void read_archive(string path)
		{
			if (path == "")
			{
				return;
			}
			this.close_archive();
			this.entries = this.parser.parse_file(path);
			if (this.entries != null)
			{
				this.build_file_list(this.entries);
				this.archive_path = path;
			}
		}

		private void export_archive()
		{
			if (this.parser == null)
			{
				return;
			}
			this.parser.export_archive();
		}

		private void close_archive()
		{
			if (this.archive_path != "")
			{
				this.explorer_view.Nodes.Clear();
				this.pic_preview.Image = null;
				this.parser.close_file();
			}
		}

		private void build_file_list(List<Entry> entries)
		{
			for (int i = 0; i < entries.Count; i++)
			{
				Entry entry = entries[i];
				string[] array = entry.name.Split(new char[]
				{
					Path.DirectorySeparatorChar
				});
				TreeNode node = this.get_root(array[0]);
				this.add_path(node, array, entry);
			}
		}

		private void add_path(TreeNode node, string[] pathbits, Entry e)
		{
			for (int i = 1; i < pathbits.Length; i++)
			{
				node = this.add_node(node, pathbits[i]);
			}
			node.Tag = e;
		}

		private TreeNode get_root(string key)
		{
			if (this.explorer_view.Nodes.ContainsKey(key))
			{
				return this.explorer_view.Nodes[key];
			}
			return this.explorer_view.Nodes.Add(key, key);
		}

		private TreeNode add_node(TreeNode node, string key)
		{
			if (node.Nodes.ContainsKey(key))
			{
				return node.Nodes[key];
			}
			return node.Nodes.Add(key, key);
		}

		private void show_image(Entry entry)
		{
			byte[] buffer = this.parser.get_filedata(entry);
			MemoryStream stream = new MemoryStream(buffer);
			Image image = Image.FromStream(stream);
			this.pic_preview.Image = image;
		}

		private void determine_action(Entry entry)
		{
			if (entry.name.EndsWith(".png"))
			{
				this.show_image(entry);
			}
		}

		private void explorer_view_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.explorer_view.SelectedNode == null || this.explorer_view.SelectedNode.Tag == null)
			{
				return;
			}
			Entry entry = (Entry)this.explorer_view.SelectedNode.Tag;
			this.determine_action(entry);
		}

		private void export_nodes(TreeNode node)
		{
			if (node.Tag != null)
			{
				Entry e = (Entry)node.Tag;
				this.parser.export_file(e);
			}
			foreach (TreeNode treeNode in node.Nodes)
			{
				this.export_nodes(treeNode);
				if (treeNode.Tag != null)
				{
					Entry e = (Entry)treeNode.Tag;
					this.parser.export_file(e);
				}
			}
		}

		private void exportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.explorer_view.SelectedNode == null)
			{
				return;
			}
			this.export_nodes(this.explorer_view.SelectedNode);
		}

		private void explorer_view_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			this.explorer_view.SelectedNode = e.Node;
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.openFileDialog1.ShowDialog();
			this.read_archive(this.openFileDialog1.FileName);
		}

		private void exportArchiveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.export_archive();
		}

		private void closeArchiveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.close_archive();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.close_archive();
			Application.Exit();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.explorer_view = new System.Windows.Forms.TreeView();
            this.explorer_menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pic_preview = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.explorer_menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_preview)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.explorer_view);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.Controls.Add(this.pic_preview);
            this.splitContainer1.Size = new System.Drawing.Size(782, 372);
            this.splitContainer1.SplitterDistance = 260;
            this.splitContainer1.TabIndex = 0;
            // 
            // explorer_view
            // 
            this.explorer_view.ContextMenuStrip = this.explorer_menu;
            this.explorer_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.explorer_view.Location = new System.Drawing.Point(0, 0);
            this.explorer_view.Name = "explorer_view";
            this.explorer_view.Size = new System.Drawing.Size(260, 372);
            this.explorer_view.TabIndex = 0;
            this.explorer_view.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.explorer_view_AfterSelect);
            this.explorer_view.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.explorer_view_NodeMouseClick);
            // 
            // explorer_menu
            // 
            this.explorer_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem});
            this.explorer_menu.Name = "contextMenuStrip1";
            this.explorer_menu.Size = new System.Drawing.Size(115, 26);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // pic_preview
            // 
            this.pic_preview.Location = new System.Drawing.Point(4, 4);
            this.pic_preview.Name = "pic_preview";
            this.pic_preview.Size = new System.Drawing.Size(138, 130);
            this.pic_preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pic_preview.TabIndex = 0;
            this.pic_preview.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "RGSS archives|*.rgssad;*.rgss2a;*.rgss3a";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(782, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exportArchiveToolStripMenuItem,
            this.closeArchiveToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.openToolStripMenuItem.Text = "Open Archive";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exportArchiveToolStripMenuItem
            // 
            this.exportArchiveToolStripMenuItem.Name = "exportArchiveToolStripMenuItem";
            this.exportArchiveToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.exportArchiveToolStripMenuItem.Text = "Export Archive";
            this.exportArchiveToolStripMenuItem.Click += new System.EventHandler(this.exportArchiveToolStripMenuItem_Click);
            // 
            // closeArchiveToolStripMenuItem
            // 
            this.closeArchiveToolStripMenuItem.Name = "closeArchiveToolStripMenuItem";
            this.closeArchiveToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.closeArchiveToolStripMenuItem.Text = "Close Archive";
            this.closeArchiveToolStripMenuItem.Click += new System.EventHandler(this.closeArchiveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 397);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "RGSS Extract";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.explorer_menu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_preview)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
