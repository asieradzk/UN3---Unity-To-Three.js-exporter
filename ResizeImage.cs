using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.IO;
using SixLabors.ImageSharp.Processing;

namespace EMS
{


    public static class ResizeImage
    {

        public static byte[] ResizePngToJpg(byte[] pngImage, int width)
        {
            try
            {
                // Use the ImageSharp Image.Load method to create a new Image object from the input byte array
                Image img = Image.Load(pngImage);

                // Calculate the new height of the image based on the aspect ratio of the original image and the desired width
                int height = (int)(width * (img.Height / (double)img.Width));

                // Use the ImageSharp Image.Clone method to create a new image with the desired size
                Image resizedImg = img.Clone(x => x.Resize(width, height));

                // Use the ImageSharp JpegEncoder to encode the resized image as a JPEG and return the resulting byte array
                using (MemoryStream ms = new MemoryStream())
                {
                    resizedImg.SaveAsJpeg(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                // Add code to handle any exceptions that may be thrown
                Console.WriteLine(ex.Message);
                return null;
            }
        }


    }
}