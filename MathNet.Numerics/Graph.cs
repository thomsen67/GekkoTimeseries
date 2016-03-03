using System;
using System.Drawing; 
using System.Drawing.Drawing2D; 
using System.Windows.Forms;
namespace WindowsApplication1
{


    /// <summary>
    /// Summary description for Class1
    /// </summary>

    // draws shapes with different brushes

    public partial class DrawShapesForm : Form
    {

        // default constructor

        public DrawShapesForm()
        {

            InitializeComponent();

        } // end constructor



        // draw various shapes on Form

        private void DrawShapesForm_Paint(object sender, PaintEventArgs e)
        {

            // references to object we will use

            Graphics graphicsObject = e.Graphics;



            // ellipse rectangle and gradient brush

            Rectangle drawArea1 = new Rectangle(5, 35, 30, 100);

            LinearGradientBrush linearBrush =

               new LinearGradientBrush(drawArea1, Color.Blue,

                  Color.Yellow, LinearGradientMode.ForwardDiagonal);



            // draw ellipse filled with a blue-yellow gradient

            graphicsObject.FillEllipse(linearBrush, 5, 30, 65, 100);



            // pen and location for red outline rectangle

            Pen thickRedPen = new Pen(Color.Red, 10);

            Rectangle drawArea2 = new Rectangle(80, 30, 65, 100);



            // draw thick rectangle outline in red

            graphicsObject.DrawRectangle(thickRedPen, drawArea2);



            // bitmap texture

            Bitmap textureBitmap = new Bitmap(10, 10);



            // get bitmap graphics

            Graphics graphicsObject2 =

               Graphics.FromImage(textureBitmap);



            // brush and pen used throughout program

            SolidBrush solidColorBrush =

               new SolidBrush(Color.Red);

            Pen coloredPen = new Pen(solidColorBrush);



            // fill textureBitmap with yellow

            solidColorBrush.Color = Color.Yellow;

            graphicsObject2.FillRectangle(solidColorBrush, 0, 0, 10, 10);




            // draw a rounded, dashed yellow line

            coloredPen.Color = Color.Yellow;

            coloredPen.DashCap = DashCap.Round;

            coloredPen.DashStyle = DashStyle.Dash;

            graphicsObject.DrawLine(coloredPen, 320, 30, 395, v150);

        } // end method DrawShapesForm_Paint

    } // end class DrawShapesForm 


}
	


