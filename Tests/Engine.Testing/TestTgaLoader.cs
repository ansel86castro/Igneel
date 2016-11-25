using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3D9Testing
{
    [Test]
    public class TestTgaLoader
    {
        [TestMethod]
        public void LoadSave()
        {
            //using (OpenFileDialog d = new OpenFileDialog())
            //{
                //if (d.ShowDialog() == DialogResult.OK)
                //{
                    TargaImage image = new TargaImage(@"F:\3DContents\Modelos\Lightning\c001_01.tga");
                    MemoryStream ms = new MemoryStream(image.Image.Width * image.Image.Height * 32);

                    var decoders = ImageCodecInfo.GetImageDecoders();
                    image.Image.Save(ms, ImageFormat.Bmp);
                  
                    File.WriteAllBytes("temp.bmp", ms.ToArray());

                    var bmpData = image.Image.LockBits(new System.Drawing.Rectangle(0, 0, image.Image.Width, image.Image.Height), 
                        ImageLockMode.ReadWrite, image.Image.PixelFormat);

                    // Get the address of the first line.
                    IntPtr ptr = bmpData.Scan0;

                    // Declare an array to hold the bytes of the bitmap. 
                    int bytes = Math.Abs(bmpData.Stride) * image.Image.Height;
                    byte[] rgbValues = new byte[bytes];

                    //Copy the RGB values into the array.
                    //System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                    // Set every third value to 255. A 24bpp bitmap will look red.   
                    //32bpp - BGRA
                    for (int counter = 0; counter < rgbValues.Length; counter += 3)
                    {
                        rgbValues[counter] = 255;
                    }

                    // Copy the RGB values back to the bitmap
                    System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

                    // Unlock the bits.
                    image.Image.UnlockBits(bmpData);

                    image.Image.Save("temp1.bmp", ImageFormat.Bmp);

                    Process.Start("temp1.bmp");
                    //var g = ((TestApplication)(TestApplication.CurrentApplication)).Form.CreateGraphics();
                    //g.DrawImage(image.Image, 0, 0);
                //}
            //}
        }
    }
}
