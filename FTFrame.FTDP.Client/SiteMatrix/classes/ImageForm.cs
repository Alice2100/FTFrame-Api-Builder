using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace FTDPClient.ImageForm
{
	/// <summary>
	/// ImageForm 的摘要说明。
	/// </summary>
	public class ImageForm : System.Windows.Forms.Form
	{
		public string text=null;
		public Bitmap bitmap=null;
		
		public ImageForm()
		{
			InitializeComponent();
		}

		#region Windows 窗体设计器生成的代码
		private void InitializeComponent()
		{
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(16, 16);
			this.Left=-1280;
			this.Top=-1000;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form2";
			this.Text = "Form2";
			this.Load += new System.EventHandler(this.Form2_Load);

		}
		#endregion

		public void Form2_Load(object sender, System.EventArgs e)
		{
            this.Width = 16;
            this.Height = 16;
            if (text == null || bitmap == null) return;
            ReInit();
		}
        public void ReInit()
        {
            Rectangle rect = this.ClientRectangle;
            Bitmap b = new Bitmap(this.Width, this.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.FillRectangle(SystemBrushes.Window, rect);
                Rectangle bmpRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                g.DrawImage(bitmap, bmpRect);
                //Rectangle textRect = new Rectangle(bitmap.Width + 4, 0, rect.Width - bitmap.Width - 4, rect.Height);
                //g.DrawString(text, this.Font, SystemBrushes.ControlDarkDark, textRect);
                Region r = GetRegion(b, SystemColors.Window);
                this.BackgroundImage = b;
                this.Region = r;
            }
        }
		#region get the region about a image
		private Region GetRegion(Bitmap _img, Color color) 
		{ 

			Color _matchColor=Color.FromArgb(color.R,color.G,color.B); 

			System.Drawing.Region rgn= new Region(); 

			rgn.MakeEmpty(); 

			Rectangle rc=new Rectangle(0,0,0,0); 

			bool inimage=false; 

			for(int y=0; y<_img.Height;y++) 
			{ 
				for(int x=0;x<_img.Width;x++) 
				{ 
					if(!inimage) 
					{ 
						if(_img.GetPixel(x,y)!=_matchColor) 
						{ 
							inimage=true; 
							rc.X=x; 
							rc.Y=y; 
							rc.Height=1; 
						} 
					} 
					else 
					{ 
						if(_img.GetPixel(x,y)==_matchColor) 
						{ 
							inimage=false; 
							rc.Width=x-rc.X; 
							rgn.Union(rc);    
						} 
					} 
				} 
				if(inimage) 
				{ 
					inimage=false; 
					rc.Width=_img.Width-rc.X; 
					rgn.Union(rc);    
				} 
			} 
			return rgn; 
		}
		#endregion
	}

}
