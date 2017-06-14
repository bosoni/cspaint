/**
 * @file ObjListDialog.cs
 * @author mjt, mixut@hotmail.com
 * 
 * @created 22.10.2006
 * 
 * näyttää listan objekteista
 * 
*/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace cspaint
{
	public partial class ObjList
	{
		public static bool opened=false; // jos ikkuna auki, niin true
		
		TreeNode rootNode;
		public ObjList()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			rootNode=treeView1.Nodes.Add("Objects");

			this.Closing += new System.ComponentModel.CancelEventHandler(this.XClosing);
			opened=true;
			
			treeView1.KeyDown+=new KeyEventHandler(OnKeyDown);
        }

		/**
		 * jos delete nappia painetaan, poistetaan objekti/layeri 
		 */ 
		private void OnKeyDown(object s, KeyEventArgs e)
		{
			if(e.KeyData==Keys.Delete)
			{
				delete();
			}
		}
  
		/**
		 * jos dialogi suljetaan, poista objektin valinta 
		 */ 
		private void XClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			PaintWindow.SelectedObject=-1;
			opened=false;
		}
	
		void delete()
		{
			if(treeView1.SelectedNode==rootNode) return;
			if(treeView1.SelectedNode==null) return;
			
			Tools.paintWindow.remove(treeView1.SelectedNode.Text);

			treeView1.SelectedNode.Remove();
			treeView1.SelectedNode=rootNode;
			
			Tools.paintWindow.updateAll();
		}
		
		/**
		 * remove 
		 */ 
		void Button1Click(object sender, System.EventArgs e)
		{
			delete();
		}

		/**
		 * kutsutaan jos valitaan joku objekti listasta 
		 */ 
		void TreeView1AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			Console.WriteLine(treeView1.SelectedNode.Text+" selected");
			Tools.paintWindow.select(treeView1.SelectedNode.Text);
			
		}
		/**
		 * päivittää objektit objlistiin 
		 */ 
		void Button2Click(object sender, System.EventArgs e)
		{
			Tools.updateList();
		}
		
		/**
		 * lisää nimi listaan, oikean tason alle
		 */ 
		public void add(string name, string layername)
		{ 
			// aseta nodeen layername-niminen taso
			TreeNode node=rootNode.FirstNode;
			while(true)
			{
				if(node.Text==layername) break;
				node=node.NextNode;
				if(node==null) return; // bugaa jos tähän
			}
			node.Nodes.Add(name);
			node.Expand(); // avaa lista
			
		}
		/**
		 * lisää taso 
		 */ 
		public void addLayer(string name)
		{
			rootNode.Nodes.Add(name);
			rootNode.Expand(); // avaa lista
		}
			
		/**
		 * tyhjennä lista 
		 */ 
		public void clear()
		{
			treeView1.Nodes.Remove(rootNode);
			rootNode=treeView1.Nodes.Add("Objects");
		}
		
		
		void ObjListLoad(object sender, System.EventArgs e)
		{
		}
		
		/**
		 * move up : siirtää valitun tason ylemmäs
		 */ 
		void Button3Click(object sender, System.EventArgs e)
		{
			if(treeView1.SelectedNode==rootNode) return;
			if(treeView1.SelectedNode==null) return;
			Tools.paintWindow.moveUp(treeView1.SelectedNode.Text);
			Tools.updateList();
		}
		
		/**
		 * move down : siirtää valitun tason alemmas
		 */ 
		void Button4Click(object sender, System.EventArgs e)
		{
			if(treeView1.SelectedNode==rootNode) return;
			if(treeView1.SelectedNode==null) return;
			Tools.paintWindow.moveDown(treeView1.SelectedNode.Text);
			Tools.updateList();
		}
	}
}
