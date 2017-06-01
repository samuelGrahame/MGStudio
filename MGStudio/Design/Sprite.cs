using MGStudio.BaseObjects;
using MGStudio.RunTime;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGStudio.Design
{
    public class Sprite : BaseSprite
    {                
        public List<string> RawImages = new List<string>();

        public SpriteM Parse(GraphicsDevice device)
        {
            var sprM = new SpriteM() { Name = this.Name, Height = this.Height, Width = this.Width, OriginX = this.OriginX, OriginY = this.OriginY, PreciseCollisionChecking = this.PreciseCollisionChecking, SeperateCollisionMasks = this.SeperateCollisionMasks };

            foreach (var bmp in GetImages())
            {
                if(bmp == null)
                {
                    sprM.Textures.Add(null);
                }
                else
                {
                    var lockBits = new LockBitmap(bmp);
                    sprM.Textures.Add(lockBits.GetTexture2D(device));
                    bmp.Dispose();
                }
            }

            return sprM;
        }

        public List<Bitmap> GetImages()
        {
            List<Bitmap> images = new List<Bitmap>();

            foreach (var rawImage in RawImages)
            {
                if(string.IsNullOrWhiteSpace(rawImage))
                {
                    images.Add(null);
                }
                else
                {
                    using (MemoryStream streamBitmap = new System.IO.MemoryStream(Convert.FromBase64String(rawImage)))
                    {
                        images.Add((Bitmap)Image.FromStream(streamBitmap));
                    }
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
