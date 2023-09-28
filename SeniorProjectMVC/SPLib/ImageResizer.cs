using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace SPLib
{
    public class ImageResizer
    {
        public ManualResetEvent complete;
        private Thread? thread;
        public Image img;
        public int size;
        public string path;
        public string extension;

        public ImageResizer(Image img, int size, string path, string extension)
        {
            this.img = img;
            this.size = size;
            this.path = path;
            this.extension = extension;

            complete = new ManualResetEvent(false);
        }

        public void Start()
        {
            thread = new Thread(ThreadProc);
            thread.Start(this);
        }

        private static void ThreadProc(object param)
        {
            ImageResizer resizer = (ImageResizer)param;
            resizer.Go();
        }

        private void Go()
        {
            Image newImg = Resize();

            if (extension == ".png")
            {
                newImg.Save(Helper.CWD + path + extension, ImageFormat.Png);
            }
            else if (extension == ".jpg")
            {
                newImg.Save(Helper.CWD + path + extension, ImageFormat.Jpeg);
            }

            complete.Set();
        }

        private Image Resize()
        {
            if (img.Height > size)
            {
                //If Image is bigger than height:
                //Determine aspect ratio
                float ratio = (float)img.Width / (float)img.Height;

                //Scale width to new height using aspect ratio
                int newWidth = (int)Math.Floor(size * ratio);

                //Return resized image
                return (Image)(new Bitmap(img, new Size(newWidth, size)));
            }
            else
            {
                //If Image is smaller than height:
                return (Image)(new Bitmap(img, new Size(img.Width, img.Height)));
            }
        }
    }
}
