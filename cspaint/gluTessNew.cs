/**
 * @file gluTessNew.cs
 * 
 * @created 23.10.2006
 * 
 * code from
 * http://www.taoframework.com/forum/viewtopic.php?p=429&sid=e9f7864423d356321050967094673761
 * 
 * thanks tao_user :)
 * 
 */
 
using System;
using System.Runtime.InteropServices;
using System.Security;
using Tao.OpenGl;

namespace cspaint
{

class gluTessNew
{
	private const CallingConvention CALLING_CONVENTION = CallingConvention.Winapi;

	// following code from http://www.taoframework.com/forum/viewtopic.php?p=429&sid=e9f7864423d356321050967094673761
	#region +++++ TEST - NEW VERTEX CALLBACK +++++ 
      //external delegate 
      public delegate void TessVertexCallback_2([In] double[] vertexData, [In] double[] colorData); 
      private static TessVertexCallback_2 newTessVertexCallback; 

      //private delegate 
      private unsafe delegate void IntTessVertCB(void *data); 
      private static IntTessVertCB internalVertCB; 

      //internal callback function 
      private static unsafe void InternalTessVertexCallback(void *data) 
      { 
         double [] vertArray = new double[3]; 
         System.Runtime.InteropServices.Marshal.Copy(new IntPtr((void *)(double *)data), vertArray, 0, 3); 

         double [] colArray = new double[3]; 
         System.Runtime.InteropServices.Marshal.Copy(new IntPtr((void *)((double *)data + 3)), colArray, 0, 3); 

         //call external delegate 
         newTessVertexCallback(vertArray, colArray); 
      } 

      //function to set callback 
      public static void gluTessCallback([In] Glu.GLUtesselator tess, int which, [In] TessVertexCallback_2 func) 
      { 
         unsafe 
         { 
            newTessVertexCallback = func; 
            internalVertCB = new IntTessVertCB(InternalTessVertexCallback); 
            __gluTessCallback(tess, which, internalVertCB); 
         } 
      } 

      //imported function (uses INTERNAL callback) 
      [DllImport("glu32.dll", CallingConvention=CALLING_CONVENTION, EntryPoint="gluTessCallback"), SuppressUnmanagedCodeSecurity] 
      private static extern unsafe void __gluTessCallback(Glu.GLUtesselator tess, int which, IntTessVertCB cb); 

      #endregion +++++ TEST - NEW VERTEX CALLBACK +++++ 


      #region +++++ TEST - NEW COMBINE CALLBACK +++++ 

      //1. private callback delegate 
      //   It matches the original OpenGL Combine Callback, that used raw pointers 
      private unsafe delegate void IntTessCombCB(double *coords, double **vertex_data, float *weight, void **dataOut); 
      private static IntTessCombCB internalCombCB; //for GC 

      //2. public external callback delegate 
      //   Managed version of previous one, with arrays instead of pointers 
      public delegate void TessCombineCallback_2([In] double[] coords, [In] double[][] vertexData, [In] float[] weight, ref double[] dataOut); 
      private static TessCombineCallback_2 newTessCombineCallback; //for GC 

      //3. imported OpenGL function 
      //   Takes 1. (instead of 2.) as a paramater 
      [DllImport("glu32.dll", CallingConvention=CALLING_CONVENTION, EntryPoint="gluTessCallback"), SuppressUnmanagedCodeSecurity] 
      private static extern unsafe void __gluTessCallback(Glu.GLUtesselator tess, int which, IntTessCombCB cb); 


      //4. function to set callback 
      //   stores the address of external callback delegate, but gives to OpenGL the address of the internal one 
      public static void gluTessCallback([In] Glu.GLUtesselator tess, int which, [In] TessCombineCallback_2 func) 
      { 
         unsafe 
         { 
            newTessCombineCallback = func; 
            internalCombCB = new IntTessCombCB(InternalTessCombineCallback); 
            __gluTessCallback(tess, which, internalCombCB); 
         } 
      } 

      //5. internal callback function 
      //   the 4. sets *this* as callback function: when the related callback is fired, this function takes the pointer 
      //   parameters, marshals them, and fires the external delegate, that receives the marshalled data 
      //   In this case, the callback functions must also take data from the external callback (with the REF parameter) 
      //   and pass them as an output 
      private static unsafe void InternalTessCombineCallback(double *coords, double **vertex_data, float *weight, void **dataOut) 
      { 
         //coordinates 
         double[] coordArray = new double[3]; 
         System.Runtime.InteropServices.Marshal.Copy(new IntPtr((void *)coords), coordArray, 0, 3); 
          
         //vertex data 
         double[][] vertexDataArray = new double[4][]; 
         for(int i = 0; i < 4; i++) 
         { 
            vertexDataArray[i] = new double[6]; 

            
// added 23.10 mjt
if(vertex_data[i]==null) 
{
	//Console.WriteLine("null "+i);
	return;
}

            System.Runtime.InteropServices.Marshal.Copy(new IntPtr((void *)vertex_data[i]), vertexDataArray[i], 0, 6); 
         } 
          
         //weights 
         float[] wtArray = new float[4]; 
         System.Runtime.InteropServices.Marshal.Copy(new IntPtr((void *)weight), wtArray, 0, 4); 
          
         //out data (empty, filled by the external callback) 
         double[] outDataArray = new double[6]; 
          
         //external callback (signature 2., address taken by 4.) 
         newTessCombineCallback(coordArray, vertexDataArray, wtArray, ref outDataArray); 
          
         //fill dataOut for final output 
         IntPtr ip = System.Runtime.InteropServices.Marshal.AllocHGlobal(6 * System.Runtime.InteropServices.Marshal.SizeOf(typeof(double))); 
         System.Runtime.InteropServices.Marshal.Copy(outDataArray, 0, ip, 6); 
         *dataOut = (double *)(void *)ip; 
      } 
	#endregion +++++ TEST - NEW COMBINE CALLBACK +++++

	}
}
