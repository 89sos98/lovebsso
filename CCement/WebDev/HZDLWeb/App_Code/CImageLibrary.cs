using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


public class CImageLibrary
{
    public enum ValidateImageResult { OK, InvalidFileSize, InvalidImageSize }

    //检查图片大小
    public static ValidateImageResult ValidateImage(string file, int MAX_FILE_SIZE, int MAX_WIDTH, int MAX_HEIGHT)
    {
        byte[] bs = File.ReadAllBytes(file);

        double size = (bs.Length / 1024);
        //大于50KB
        if (size > MAX_FILE_SIZE) return ValidateImageResult.InvalidFileSize;
        Image img = Image.FromFile(file);
        if (img.Width > MAX_WIDTH || img.Height > MAX_HEIGHT) return ValidateImageResult.InvalidImageSize;
        return ValidateImageResult.OK;
    }

    //按宽度比例缩小图片
    public static Image GetOutputSizeImage(Image imgSource, int MAX_WIDTH)
    {
        Image imgOutput = imgSource;

        Size size = new Size(imgSource.Width, imgSource.Height);
        if (imgSource.Width <= 3 || imgSource.Height <= 3) return imgSource; //3X3大小的图片不转换

        if (imgSource.Width > MAX_WIDTH || imgSource.Height > MAX_WIDTH)
        {
            double rate = MAX_WIDTH / (double)imgSource.Width;

            if (imgSource.Height * rate > MAX_WIDTH)
                rate = MAX_WIDTH / (double)imgSource.Height;

            size.Width = Convert.ToInt32(imgSource.Width * rate);
            size.Height = Convert.ToInt32(imgSource.Height * rate);

            imgOutput = imgSource.GetThumbnailImage(size.Width, size.Height, null, IntPtr.Zero);
        }

        return imgOutput;
    }

    //按比例缩小图片
    public static Image GetOutputSizeImage(Image imgSource, Size outSize)
    {
        Image imgOutput = imgSource.GetThumbnailImage(outSize.Width, outSize.Height, null, IntPtr.Zero);
        return imgOutput;
    }

    public static byte[] GetImageBytes(string imageFileName)
    {
        Image img = Image.FromFile(imageFileName);
        return GetImageBytes(img);
    }

    public static byte[] GetImageBytes(Image img)
    {
        if (img == null) return null;
        try
        {
            System.IO.MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            byte[] bs = ms.ToArray();
            ms.Close();
            return bs;
        }
        catch { return null; }
    }

    public static Image FromBytes(byte[] bs)
    {
        if (bs == null) return null;
        try
        {
            MemoryStream ms = new MemoryStream(bs);
            Image returnImage = Image.FromStream(ms);
            ms.Close();
            return returnImage;

        }
        catch { return null; }
    }

}