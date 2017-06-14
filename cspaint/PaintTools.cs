/**
 * @file PaintTools.cs
 * @author mjt, mixut@hotmail.com
 * 
 * @created 22.10.2006
 * @edited 28.6.2007
 * 
 * kynä, ympyrä ja nelikulmio työkalut
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

class baseObject 
{ 
	public static int lines=0, rects=0, circles=0; 

	protected byte[] color=new byte [3] {0,0,0};
	protected byte[] fillColor=new byte [3] {0,0,0};
	protected string name; // objektin nimi 
	protected bool fill=false; // jos true, kuvio täytetään

	public string Name
	{
		get { return name; }
	}
	public void setColor(byte[] color)
	{
		Gl.glColor3ub(color[0], color[1], color[2]);
		
	}
	public void setColor(byte[] col, byte[] fillCol, bool fill)
	{
		color[0]=col[0];
		color[1]=col[1];
		color[2]=col[2];
		fillColor[0]=fillCol[0];
		fillColor[1]=fillCol[1];
		fillColor[2]=fillCol[2];
		this.fill=fill;
	}
	
	// javan vectoria vastaava 
	protected ArrayList array=new ArrayList(); 
	
	public void add(float x, float y) 
	{ 
		Vector2f p=new Vector2f(x, y); 
		array.Add(p); 
	} 
	public Vector2f get(int i) 
	{ 
		return (Vector2f)array[i];
	} 
	
	// ei käytössä
	public virtual void render(bool col)
	{
	}
	
	public int numVert()
	{
		return array.Count;
	}
	
} 

class Line : baseObject 
{ 
	int displayList=-1;

	public Line()
	{ 
		name="Line"+lines++; 
	} 

	public void close()
	{
		// vika vertex ekaks vertexiks, sulkee polyn
		Vector2f p=(Vector2f)array[0];
		add(p.X, p.Y);
	}
	
	float tmpx=-1, tmpy=-1;
	public void setXY(float x, float y)
	{
		tmpx=x; tmpy=y;	
	}
	
	/**
	 * piirrä kynällä piirretyt viivat. jos col==true, käytä sen väriä 
	 */ 
	public override void render(bool col) 
	{ 
		if(displayList>0)
		{
			if(col) setColor(fillColor);
			Gl.glCallList(displayList);
		}
		
		if(col) setColor(color);
		Gl.glBegin(Gl.GL_LINE_STRIP);
		
		for(int q=0; q<array.Count; q++) 
		{ 
           	Vector2f p=(Vector2f)array[q];
           	Gl.glVertex2f(p.X, p.Y);
 		} 
		if(tmpx>0)
		{
			Gl.glVertex2f(tmpx, tmpy); // suora viiva hiiren koordinaatteihin
		}
		Gl.glEnd();
	} 
	
	public void tesselate(bool closed)
	{
		Glu.GLUtesselator tess = Glu.gluNewTess();

		gluTessNew.gluTessCallback(tess, Glu.GLU_TESS_VERTEX, new gluTessNew.TessVertexCallback_2(tessVertex));
		gluTessNew.gluTessCallback(tess, Glu.GLU_TESS_COMBINE, new gluTessNew.TessCombineCallback_2(tessCombine));
	
		Glu.gluTessCallback(tess, Glu.GLU_TESS_BEGIN, new Glu.TessBeginCallback(Begin));
		Glu.gluTessCallback(tess, Glu.GLU_TESS_END, new Glu.TessEndCallback(End));
		//Glu.gluTessCallback(tess, Glu.GLU_TESS_ERROR, new Glu.TessErrorCallback(Error));
		this.displayList=Gl.glGenLists(1);

		Gl.glNewList(displayList, Gl.GL_COMPILE);
		Glu.gluTessBeginPolygon(tess, IntPtr.Zero);
		Glu.gluTessBeginContour(tess); 
		
		double[][] vert=new double [array.Count][];
			
		int q;
		for(q=0; q<array.Count; q++)
		{
			vert[q]=new double [3];
			Vector2f ve=(Vector2f) array[q];
			vert[q][0]=ve.X;
			vert[q][1]=ve.Y;
			vert[q][2]=0;
			Glu.gluTessVertex(tess, vert[q], vert[q]);
		}

		Glu.gluTessEndContour(tess); 
		Glu.gluEndPolygon(tess);
		Gl.glEndList();
	

		if(closed==false) 
		{
			array.Remove(array[array.Count-1]);
			array.Remove(array[array.Count-1]);
		}
		
		render(true);
		
	}
	
	private static void Begin(int prim)
	{ 
		Gl.glBegin(prim); 
	}
	
	
	private static void tessVertex(double[] data, double[] color)
	{ 
		Gl.glVertex3dv(data); 
	}
	private static void End() 
	{ 
		Gl.glEnd(); 
	}
	private static void tessCombine(double[] coords, double[][] vertexData, float[] weight, ref double[] dataOut)
	{
            double[] vertex = new double[6];
            int i;

            vertex[0] = coords[0];
            vertex[1] = coords[1];
            vertex[2] = coords[2];
            
   			for (i = 3; i < 6; i++)
      			vertex[i] = weight[0] * vertexData[0][i] 
                  + weight[1] * vertexData[1][i]
                  + weight[2] * vertexData[2][i] 
                  + weight[3] * vertexData[3][i];
             
            dataOut=vertex;
	}

}

class Rect : baseObject
{ 
	public Rect()
	{ 
		name="Rect"+rects++; 
	} 
	
	// nelikulmion leveys ja korkeus
	float tmpx=-1, tmpy=-1; 
	/**
	 * piirrä nelikulmio. jos col==true, käytä sen väriä 
	 */ 
	public override void render(bool col) 
	{ 
		//if(tmpx==-1 && tmpy==-1) return;
		Vector2f p=(Vector2f)array[0];

		if(fill) // täyttö
		{
			if(col) setColor(fillColor);
			Gl.glBegin(Gl.GL_QUADS);
			Gl.glVertex2f(p.X, p.Y); 
			Gl.glVertex2f(p.X+tmpx, p.Y); 
			Gl.glVertex2f(p.X+tmpx, p.Y+tmpy); 
			Gl.glVertex2f(p.X, p.Y+tmpy);
			Gl.glEnd();
		}

		// ääriviivat
		if(col) setColor(color);
		Gl.glBegin(Gl.GL_LINE_STRIP);
		Gl.glVertex2f(p.X, p.Y); 
		Gl.glVertex2f(p.X+tmpx, p.Y); 
		Gl.glVertex2f(p.X+tmpx, p.Y+tmpy); 
		Gl.glVertex2f(p.X, p.Y+tmpy); 
		Gl.glVertex2f(p.X, p.Y); 
			
		Gl.glEnd();
	} 
	
	public void calcWH(float x, float y) 
	{ 
		Vector2f p=(Vector2f)array[0];
		tmpx=x-p.X;
		tmpy=y-p.Y;
	} 
} 
class Circle : baseObject 
{ 
	public Circle()
	{ 
		name="Circle"+circles++; 
	} 
	
	float r1=0, r2=0; 
	public void calcR(float x, float y) 
	{ 
		// vähennetään keskipiste, saadaan säde 
		Vector2f p=(Vector2f)array[0];
		x-=p.X;
		y-=p.Y;
	
		// laske säteen pituus 
		r1=(float)Math.Sqrt( x*x + y*y ); 
		r2=r1;
	} 
	
	public void calcXY(float x, float y)
	{
		// vähennetään keskipiste
		Vector2f p=(Vector2f)array[0];
		x-=p.X;
		y-=p.Y;
					
		r1=x;
		r2=y;
	}
	
	/**
	 * piirrä ympyrä. jos col==true, käytä sen väriä 
	 */ 
	public override void render(bool col) 
	{ 
		float x, y; 
		Vector2f p=(Vector2f)array[0];
		float ADD=(float)(2*Math.PI)/50;

		if(fill)
		{
			Gl.glTranslatef(p.X, p.Y, 0);
			if(col) setColor(fillColor);
			
			Gl.glBegin(Gl.GL_TRIANGLE_FAN);
			for(float angle=0.0f;angle<=2*Math.PI+ADD;angle+=ADD)
			{
				x=(float)Math.Sin((float)angle)*r1;
				y=(float)Math.Cos((float)angle)*r2;
				Gl.glVertex2f(x, y);
			}
    			
			Gl.glEnd();
			Gl.glLoadIdentity();
			
		}
			
		if(col) setColor(color);
		Gl.glTranslatef(p.X, p.Y, 0);
		Gl.glBegin(Gl.GL_LINE_STRIP);
	
		
		for(float q=0; q<=2*Math.PI+ADD; q+=ADD)
		{ 
			x=(float)Math.Sin((float)q)*r1;
			y=(float)Math.Cos((float)q)*r2;
						
			Gl.glVertex2f(x, y); 
		} 
		
		Gl.glEnd();
		Gl.glLoadIdentity();
	} 
} 



}
