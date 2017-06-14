/**
 * @file PaintWindow.cs
 * @author mjt, mixut@hotmail.com
 * 
 * @created 22.10.2006
 * @edited 28.6.2007
 * 
 * luo opengl ikkuna johon piirto tapahtuu.
 * 
 */

/*
 * ohjelma toimii seuraavasti:
 *  alussa luodaan Layer0 johon piirto tapahtuu.
 * uusia tasoja voi luoda mielin määrin, myös poistaa niitä.
 * valittuun tasoon piirretään. 
 * tasoja voi siirtää ylös/alas. mitä alempana, sitä "lähempänä" ruutua,
 * koska alemmat piirretään viimeisenä ja ylemmät ekana.
 * tasoja voi myös liikutella.
 * ctrl nappi pohjassa saa kynällä suoran viivan ja circlellä ellipsin 
 *
 * kuvan voi tallentaa .jpg tiedostoon (hidas).
 * 
 */ 
using System;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using Tao.OpenGl;
using Tao.FreeGlut;

namespace cspaint
{
	/**
	 * tason nimi ja objektilista 
	 */ 
	class Layer
	{
		public static int layers;
		public int sx=0, sy=0; // tason aloituskohta
		
		public string name; 
		public ArrayList objs=new ArrayList(); 
		public Layer()
		{
			this.name="Layer"+layers++;
			
		}
		public void reset()
		{
			layers=0;
		}
		public void setXY(int x, int y)
		{
			sx=x; sy=y;
		}
		
	}
	
	/**
	 * piirto tapahtuu opengl ikkunaan 
	 */ 
	public class PaintWindow
	{
		public static int width=400, height=400; // ikkunan koko
        static bool running=true;
		
		int mode=1;  // aseta kynätoiminto alussa
		bool fill=true; // jos true, objektit myös täytetään, muuten vaan ääriviivat
		bool closed=true; // jos true, suljetaan polygoni
		baseObject tmpObj=null;
		static int selectedObj=-1;
		public static int SelectedObject
		{
			set { selectedObj=value; }
		}
		
		public ArrayList layers=new ArrayList(); // tasot
		int selectedLayer=-1; // valittu taso
		public int SelectedLayer
		{
			get { return selectedLayer; }
		}
		
		/**
		 * tallenna jpeg 
		 * 
		 * tämä tallennusfunktio bugaa pahan kerran. voi kaataa
		 * ohjelman, on todella hidas, välillä jpeg tulee tiedostoon
		 * miten sattuu!
		 */ 
		public void saveJPG(string name)
		{
			if(name!="")
			{
				if(!name.EndsWith(".jpg") && !name.EndsWith(".jpeg"))
					name=name+".jpg";
				
				Console.WriteLine("Saving "+name+" ...please wait.");
				// nappaa kuva byte taulukkoon
				byte[] data=new byte [width*height*3];
				Gl.glReadPixels(0,0, width, height, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, data);
				
				// aseta joka pikseli
				// HIDAS!!!
				// FIXME joku nopeempi tapa
				Bitmap bitmap=new Bitmap(width, height);
				Color col=new Color();
				int ind=0;
				
				Console.Write("Set pixels");
				for(int h=height-1; h>0; h--)
				{
					for(int w=0; w< (width/4)*4 ; w++)
					{
						col=Color.FromArgb(255, data[ind++],data[ind++],data[ind++]);
						bitmap.SetPixel(w, h, col);
					}
					
					Console.Write("."); // näyttää vaan ettei ohjelma ole kaatunut
				}
				data=null;
				
				// tallenna
				bitmap.Save(name);
				bitmap.Dispose();
				
				Console.WriteLine("OK.");
			}
		
		}
		
		/**
		 * asettaa ohjelman alkutilaan, eli voi aloittaa
		 * piirtämisen puhtaalta pöydältä
		 */ 
		public void clearAll()
		{
			selectedObj=-1;
			selectedLayer=-1;
			fill=true; // jos true, objektit myös täytetään, muuten vaan ääriviivat
			closed=true; // jos true, suljetaan polygoni
			tmpObj=null;
			layers.Clear();

			Gl.glClearColor(1,1,1,1);
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
			
			Layer.layers=0;
			// luo yksi taso
            newLayer();
            Tools.updateList();

			updateAll();
		}
		
		/**
		 * layerin liikutus
		 */ 
		public void moveLayer()
		{
			setMode(4); // liikutus
		}
		
		/**
		 * poistaa valitun tason, myös kaikki viivat mitä tasossa on 
		 */ 
		public void removeLayer()
		{
			layers.Remove(layers[selectedLayer]);
		}
		
		/**
		 * luo uusi taso ja lisää se layers-arraylistiin 
		 * piirto tapahtuu sitten siihen tasoon
		 */ 
		public void newLayer()
		{
			Layer layer=new Layer();
			layers.Add(layer);
			selectedLayer++;
		}
		
		/**
		 * valitse taso nimen perusteella
		 */
		public void selectLayer(string name)
		{
			for(int q=0; q<layers.Count; q++)
			{
				if( ((Layer)layers[q]).name==name )
				{
					selectedLayer=q;
					selectedObj=-1;
					return;
				}
			}

		}
		
		/**
		 * move up : siirtää valitun tason ylemmäs
		 */ 
		public void moveUp(string name)
		{
			if(name.Contains("Layer"))
			{
				if(selectedLayer>0)
				{
					Layer l1=(Layer)layers[selectedLayer-1];
					layers[selectedLayer-1]=layers[selectedLayer];
					layers[selectedLayer]=l1;
					updateAll();
				}
				
			}
		}
		/**
		 * move down : siirtää valitun tason alemmas
		 */ 
		public void moveDown(string name)
		{
			if(name.Contains("Layer"))
			{
				if(selectedLayer<layers.Count-1)
				{
					Layer l1=(Layer)layers[selectedLayer+1];
					layers[selectedLayer+1]=layers[selectedLayer];
					layers[selectedLayer]=l1;
					updateAll();
				}
				
			}
		}
		
		/**
		 * valitse objekti tai taso 
		 */ 
		public void select(string name)
		{
			if(name.Contains("Layer"))
			{
				selectLayer(name);
			}
			else
				selectObject(name);
			
		}
		
		/**
		 * etsi objekti nimen perusteella, aseta sen index selectObj muuttujaan 
		 */ 
		public void selectObject(string name)
		{
			selectedObj=-1;
			// käy kaikki tasot läpi
			for(int l=0; l<layers.Count; l++)
			{
			
				// etsi objekteista name nimistä
				for(int q=0; q<((Layer)layers[l]).objs.Count; q++)
				{
					baseObject tmp=(baseObject)((Layer)layers[l]).objs[q];
					if(tmp.Name==name)
					{
						selectedObj=q;
						selectedLayer=l;
						break;					
					}
				}
			}
			
		}
		/**
		 * poista valittu objekti 
		 */ 
		public void removeObject()
		{
			((Layer)layers[selectedLayer]).objs.Remove( ((Layer)layers[selectedLayer]).objs[selectedObj] );
		}
		
		/**
		 * suljetaanko polygoni 
		 */ 
		public void setClosed(bool closed)
		{
			this.closed=closed;	
		}
		
		/**
		 * täytetäänkö kuviot, true/false 
		 */ 
		public void setFill(bool fill)
		{
			this.fill=fill;
		}
		
		/**
		 * poista valittu objekti/layer
		 */ 
		public void remove(string name)
		{
			// poista taso
			if(name.Contains("Layer"))
			{
				selectLayer(name); // valitse se taso
				removeLayer(); // poista se	
				selectedObj=-1;
				selectedLayer=0;
			}
			else
			{
				selectObject(name); // valitse objekti
				removeObject(); // poista se
				selectedObj=-1;
			}
		}
		
		
		
		byte[] color=new byte [3] {0,0,0};
		byte[] fillColor=new byte [3] {255,255,255};
		/*
		 * aseta color taulukko 
		 */ 
		public void setColor(byte r, byte g, byte b)
		{
			color[0]=r;
			color[1]=g;
			color[2]=b;
		}
		/*
		 * aseta fillColor taulukko 
		 */ 
		public void setFillColor(byte r, byte g, byte b)
		{
			fillColor[0]=r;
			fillColor[1]=g;
			fillColor[2]=b;
		}
		
		/**
		 * valittu työkalu
		 */ 
		public void setMode(int mode)
		{
			this.mode=mode;
		}
		
		/**
		 * luo toinen säie ja aseta loop() metodi siihen
		 */ 
        public PaintWindow(int width, int height)
        {
        	create(width, height);
        	
        	// loopataan eri threadissa
        	Thread thread=new Thread(loop);
        	thread.Start();
        	
        }

        /**
         * loppulätinät 
         */ 
        public void shutdown()
        {
        	running=false;
            Glut.glutMouseFunc(null);
            Glut.glutMotionFunc(null);
            Glut.glutCloseFunc(null);
            Glut.glutReshapeFunc(null); 
        }
        

        
        bool ctrl=false;
        /*private void keyboard(byte key, int x, int y)
        {
        	int mod = Glut.glutGetModifiers();
			if(mod==Glut.GLUT_ACTIVE_CTRL) ctrl=true;
			else ctrl=false;
        }*/
        
        /**
         * luo glut ikkuna, aseta callbackit ja luo tyhjä texture 
         */ 
		void create(int width, int height)
		{
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_RGB);
            Glut.glutInitWindowSize(width, height);
            Glut.glutInitWindowPosition(100, 100);
            Glut.glutCreateWindow("CSPaint");

            Glut.glutDisplayFunc(new Glut.DisplayCallback(display));
            Glut.glutMouseFunc(new Glut.MouseCallback(mouseButton));
            Glut.glutMotionFunc(new Glut.MotionCallback(mouseMotion));
            Glut.glutCloseFunc(new Glut.CloseCallback(closeWindow));
            Glut.glutReshapeFunc(new Glut.ReshapeCallback(reshape)); 
            //Glut.glutKeyboardFunc(new Glut.KeyboardCallback(keyboard));
            
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glClearColor(1,1,1,1);
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            Gl.glDisable(Gl.GL_CULL_FACE);

			Gl.glDisable(Gl.GL_DEPTH_TEST);
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Gl.glOrtho(0, width, height, 0, -1, 1);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
            
            // luo yksi taso
            newLayer();
		}

		void closeWindow()
		{
			running=false;
		}
		
		/**
		 * loop metodi menee eri threadiin
		 */ 
		void loop()
		{
			while(running)
			{
				Glut.glutMainLoopEvent();
        		Glut.glutPostRedisplay(); // kamat ruudulle
				
				Thread.Sleep(1);
			}
		}

		/**
		 * jos suoritettu undo tai poistettu objekti, tyhjennetään
		 *  ruutu ja piirretään kaikki uudelleen (paitsi poistettu)
		 */ 
		public void updateAll()
		{
			Gl.glLoadIdentity();
			Gl.glClearColor(1,1,1,1);
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            // renderoi kaikilta tasoilta
            for(int l=0; l<layers.Count; l++)
            {
	            for(int i=0; i<((Layer)layers[l]).objs.Count; i++)
				{
					Gl.glTranslatef(((Layer)layers[l]).sx, ((Layer)layers[l]).sy, 0);
					
	            	baseObject o=(baseObject)((Layer)layers[l]).objs[i];
					if(o!=null) 
					{
						o.render(true);
					}
					Gl.glLoadIdentity();
				}
			}
            
		}
		
		float sin=0;
		/**
		 * renderoi grafiikat 
		 */ 
        private void display() 
		{
        	if(!running) return;
        	
        	Gl.glLoadIdentity();
        
        	// valittu objekti välkkymään
        	if(selectedObj!=-1)
        	{
				// jos tasoa liikutettu, liikuta
				Gl.glTranslatef(((Layer)layers[selectedLayer]).sx, ((Layer)layers[selectedLayer]).sy, 0);
				
        		baseObject o=(baseObject)((Layer)layers[selectedLayer]).objs[selectedObj];
				sin+=0.1f;
				Gl.glColor3f((float)Math.Sin((float)sin),0,0);
				o.render(false);
				Gl.glLoadIdentity();

        	}
        	
			if(tmpObj!=null) 
			{
				Gl.glTranslatef(((Layer)layers[selectedLayer]).sx, ((Layer)layers[selectedLayer]).sy, 0);
				
				if(!invert) 
				{
					tmpObj.render(true);
					invert=!invert;
				}
				
				Gl.glLoadIdentity();
			}
			
			Gl.glFlush();
            Glut.glutSwapBuffers();
        }
                
        int mx=0, my=0; // hiiren koordinaatit jos layeria liikutetaan
        bool invert=false;
        /**
         * kutsutaan jos hiiren nappi pohjassa liikutetaan hiirtä 
         */ 
        private void mouseMotion(int x, int y)
        {
        	if(mode==0) return;
        	if(mode!=4 && tmpObj==null) return;
	
			if(mode!=4)
			{
				Gl.glTranslatef(((Layer)layers[selectedLayer]).sx, ((Layer)layers[selectedLayer]).sy, 0);
		        x-=(int)((Layer)layers[selectedLayer]).sx;
		        y-=(int)((Layer)layers[selectedLayer]).sy;
			}

			if(tmpObj!=null) tmpObj.render(true);
	
        	int mod = Glut.glutGetModifiers();
			if(mod==Glut.GLUT_ACTIVE_CTRL) ctrl=true;
			else ctrl=false;
			
			
			switch(mode) 
			{ 
				case 1: //drawLine:
				if(ctrl) // jos ctrl pohjassa, piirrä suora viiva, muuten lisää piste
				{
					((Line)tmpObj).setXY(x, y); // viivan loppukoordinaatit on hiiren koordinaatit
				}
				else
				{
					tmpObj.add(x, y);
					((Line)tmpObj).setXY(-1, -1);
				}
				break; 
				
				// x1y1 -> x2y2 
				case 2: //drawRect:
					((Rect)tmpObj).calcWH(x, y);
					
				break; 
				
				// keskipiste ja säde 
				case 3: //drawCircle:
				if(ctrl) // jos ctrl pohjassa, piirrä ellipsi, muuten ympyrä
				{
					((Circle)tmpObj).calcXY(x, y);
				}
				else
				{
					((Circle)tmpObj).calcR(x, y); // laske säde
				}
				break; 
			
				case 4: // layerin liikutus
					int xx=mx-x;
					int yy=my-y;
					((Layer)layers[selectedLayer]).setXY(-xx, -yy);
					updateAll();					
				break;
			} 
			
			invert=!invert;
        }

        /**
         * kutsutaan jos hiiren nappia painetaan 
         */ 
        private void mouseButton(int button, int state, int x, int y)
        {
        	if(mode==0) return;
        	if(selectedObj!=-1)
        	{
        		selectedObj=-1;
        		updateAll();
        	}

        	if(mode!=4)
        	{
				Gl.glTranslatef(((Layer)layers[selectedLayer]).sx, ((Layer)layers[selectedLayer]).sy, 0);
		        x-=(int)((Layer)layers[selectedLayer]).sx;
		        y-=(int)((Layer)layers[selectedLayer]).sy;
			}
        	
	        switch(button)
            {
        			// jos painetaan hiiren vasenta nappia
                case Glut.GLUT_LEFT_BUTTON:
        			
        			// kun painetaan pohjaan
                    if(state == Glut.GLUT_DOWN)
	                {
                    	if(mode!=4)
                    	{
                    		invert=false;
                    		Gl.glEnable(Gl.GL_COLOR_LOGIC_OP);
                    		Gl.glLogicOp(Gl.GL_INVERT);
                    	}
                    	
                    	switch(mode)
                    	{
                    		case 1:
                    			tmpObj=new Line();
                    			break;
                    		case 2:
                    			tmpObj=new Rect();
                    			break;
                    		case 3:
                    			tmpObj=new Circle();
                    			break;
                    		case 4:
                    			mx=x; my=y; // layerin liikutus
                    			return;
                    	}
                    	tmpObj.setColor(color, fillColor, true);
    	            	tmpObj.add((float)x, (float)y);
                    }
                    else // irrotetaan napista
                    {
                    	if(tmpObj==null) return;
                    	tmpObj.add((float)x, (float)y);

						if(mode==1) // jos kynä
						{
							if(fill || closed) // jos täyttö tai suljettu
							{
								// vika vertex ekaks vertexiks, sulkee polyn
								((Line)tmpObj).close();
							}

							if(fill) // alue pitää täyttää
                    		{
                    			((Line)tmpObj).tesselate(closed);
							}
                    	}
						if(layers.Count==0) 
						{
							selectedLayer=-1;
							newLayer();
						}
						
						Gl.glDisable(Gl.GL_COLOR_LOGIC_OP);
						tmpObj.render(true);
						((Layer)layers[selectedLayer]).objs.Add(tmpObj);
    	            	tmpObj=null;
                    	
						Tools.updateList();
                    }
                    break;
                    
                case Glut.GLUT_MIDDLE_BUTTON:
                case Glut.GLUT_RIGHT_BUTTON:
                    break;
                    
                default:
                    break;
            }
        }

        /**
         * kutsutaan jos ikkunan koko muuttuu 
         */ 
        private void reshape(int width, int height)
        {
			PaintWindow.width=width;
			PaintWindow.height=height;

			Gl.glViewport(0,0, width, height);

			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Gl.glOrtho(0, width, height, 0, -1, 1);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();

			Gl.glClearColor(1,1,1,1);
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
			updateAll();
        }
   
    }
}
