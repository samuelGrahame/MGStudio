using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.Design
{
    public class Sprite : ProjectItem
    {        
        public int Width { get; set; }
        public int Height { get; set; }

        public int OriginX { get; set; }
        public int OriginY { get; set; }

        public bool PreciseCollisionChecking { get; set; }
        public bool SeperateCollisionMasks { get; set; }

        public List<string> RawImages = new List<string>();

        public List<Bitmap> GetImages()
        {
            List<Bitmap> images = new List<Bitmap>();

            foreach (var rawImage in RawImages)
            {
                using (MemoryStream streamBitmap = new System.IO.MemoryStream(Convert.FromBase64String(rawImage)))
                {
                    images.Add((Bitmap)Image.FromStream(streamBitmap));
                }                
            }
            return images;
        }

        public void SetImages(List<Bitmap> Bitmaps, bool Dispose = false)
        {
            List<string> rawImages = new List<string>();

            foreach (var bitmap in Bitmaps)
            {
                using (var ms = new MemoryStream())
                {
                    using (var bmp = new Bitmap(bitmap))
                    {
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        rawImages.Add(Convert.ToBase64String(ms.GetBuffer()));
                    }
                    if(Dispose)
                    {
                        bitmap.Dispose();
                    }
                }                
            }

            RawImages = rawImages;
        }
    }
}
