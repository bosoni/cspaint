/**
 * @file ToolsDialog.cs
 * @author mjt, mixut@hotmail.com
 * 
 * @created 22.10.2006
 * 
 * @edited 28.6.2007
 * 
 * työkalu dialogi
 * 
 */
 
using System;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Tao.OpenGl;


namespace cspaint
{
	struct Vector2f
	{
		float x, y; //, z;
		
		public float X
		{
			set { x=value; }
			get	{ return x; }
		}
		public float Y
		{
			set { y=value; }
			get	{ return y; }
		}
		/*public float Z
		{
			set { z=value; }
			get	{ return z; }
		}*/
		
		public Vector2f(float x, float y/*, float z*/)
		{
			this.x=x;
			this.y=y;
			//this.z=z;
		}
	}
	
    public partial class Tools : Form
    {
    	public static PaintWindow paintWindow;
        public Tools()
        {
            InitializeComponent();
            createMenu();
            
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);

            paintWindow=new PaintWindow(PaintWindow.width, PaintWindow.height);
        }

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			paintWindow.shutdown();
		}
	
        /*
         * kynä työkalu 
         */ 
        void Button1Click(object sender, System.EventArgs e)
        {
        	paintWindow.setMode(1);
        }
        
        /**
         * nelikulmio
         */ 
        void Button2Click(object sender, System.EventArgs e)
        {
        	paintWindow.setMode(2);
        }
        
        /**
         * ympyrä 
         */ 
        void Button3Click(object sender, System.EventArgs e)
        {
        	paintWindow.setMode(3);
        }
        
        void chooseColor()
        {
        	colorDialog1=new ColorDialog();
        	colorDialog1.ShowDialog();
        	panel1.BackColor=colorDialog1.Color;
        	paintWindow.setColor(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
        	
        }
        /**
         * värin valinta 
         */ 
        void Button5Click(object sender, System.EventArgs e)
        {
        	chooseColor();
        }

        
        public static ObjList list;
        /**
         * list objects
         * listaa objektit (viivat, ympyrät, nelikulmiot)
         */ 
        void Button4Click(object sender, System.EventArgs e)
        {
        	// jos ikkuna ei ole auki, luo uusi
        	if(ObjList.opened==false) list=new ObjList();

        	updateList();
        	list.Show();
        }
        
        /**
         * päivitä nimet 
         */ 
        public static void updateList()
        {	
        	if(ObjList.opened==false) return;
        	
        	list.clear();
        	
        	// lisää joka tason nimi ja kaikkien objektien nimet tasosta
        	for(int l=0; l<paintWindow.layers.Count; l++)
        	{
        		
        		list.addLayer(  ((Layer)paintWindow.layers[l]).name  );
        		
        		for(int q=0; q<((Layer)paintWindow.layers[l]).objs.Count; q++)
        		{
        			list.add(((baseObject)((Layer)paintWindow.layers[l]).objs[q]).Name, ((Layer)paintWindow.layers[l]).name);
        			
        		}
        		              
        		
        	}
        }
        
        /**
         * valitaan täyttöväri
         */ 
        void Button6Click(object sender, System.EventArgs e)
        {
        	colorDialog1=new ColorDialog();
        	colorDialog1.ShowDialog();
        	panel2.BackColor=colorDialog1.Color;
        
        	paintWindow.setFillColor(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
        	
        }
        /**
         * filled valinta
         */ 
        void CheckBox1CheckedChanged(object sender, System.EventArgs e)
        {
        	paintWindow.setFill(checkBox1.Checked);
        }

        void Panel1Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
        }
        
        /**
         * new layer, luo uuden tason 
         */ 
        void Button7Click(object sender, System.EventArgs e)
        {
        	paintWindow.newLayer();
        	updateList();
        }
        /**
         * closed eli rendataanko niin että viivarykelmä näyttää suljetulta.
         *  eli vika vertex on eka vertex.
         */ 
        void CheckBox2CheckedChanged(object sender, System.EventArgs e)
        {
        	paintWindow.setClosed(checkBox2.Checked);
        }
        
        /**
         * move layer: layeria voi liikuttaa
         */ 
        void Button8Click(object sender, System.EventArgs e)
        {
        	paintWindow.moveLayer();
        }

        
        
        // menut:
        private MainMenu mainMenu;
        public void createMenu()
        {
			mainMenu = new MainMenu();
			MenuItem File = mainMenu.MenuItems.Add("File");
			File.MenuItems.Add(new MenuItem("New", new System.EventHandler(NewToolMenuItemClick)));

			                    
			File.MenuItems.Add(new MenuItem("Save JPEG", new System.EventHandler(SaveJPGToolMenuItemClick)));
			File.MenuItems.Add(new MenuItem("Open", new System.EventHandler(openToolMenuItem_Click)));
			File.MenuItems.Add(new MenuItem("Exit", new System.EventHandler(exitToolMenuItem_Click)));
        
			this.Menu=mainMenu;
			
			MenuItem About = mainMenu.MenuItems.Add("About");
			About.MenuItems.Add(new MenuItem("About", new System.EventHandler(aboutToolMenuItem_Click)));
			this.Menu=mainMenu;
			
			
        }
        
        
        private void aboutToolMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("CSPaint by mjt, 2006-07\nmixut@hotmail.com", "About");
        }

        private void exitToolMenuItem_Click(object sender, EventArgs e)
        {
        	paintWindow.shutdown();
            Application.Exit();
        }
        /**
         * tiedoston avaus dialogi, ei käytössä 
         */ 
        private void openToolMenuItem_Click(object sender, EventArgs e)
        {
        	MessageBox.Show("open not implemented", "Open");
        	
        	/*
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            MessageBox.Show(openFileDialog1.FileName, "Valittu filu: ");
            */            
        }

        /**
         * tallenna jpeg kuva 
         */ 
        void SaveJPGToolMenuItemClick(object sender, System.EventArgs e)
        {
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.ShowDialog();
        	
            // tallenna texture jpeg kuvaksi.
            paintWindow.saveJPG(saveFileDialog1.FileName);
        }
        
        /**
         * uusi kuva
         */ 
        void NewToolMenuItemClick(object sender, EventArgs e)
        {
        	paintWindow.clearAll();

        	checkBox1.Checked=true;
        	checkBox2.Checked=true;
        	
        }
        
    }
}
