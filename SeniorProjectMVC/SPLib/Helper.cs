using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using Image = System.Drawing.Image;

namespace SPLib
{
    public class Helper
    {
        public static string CWD { get => Directory.GetCurrentDirectory(); }
        public static string[] ImageExtensions { get => new string[] { ".jpg", ".png" }; }

        #region GeneralFiles
        /// <summary>
        /// Returns the contents of folder 'directory' as a list of DirectoryInfo. 
        /// The 'directory' path must start at the root of the project directory.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns>
        /// 
        /// </returns>
        public static List<FileInfo> GetFiles(string directory)
        {
            var dirInfo = new DirectoryInfo(CWD + directory);
            var extensions = ImageExtensions;

            //Get all image files in folder
            return dirInfo.GetFiles().Where(f => extensions.Contains(
                f.Extension.ToLower())).ToList();
        }

        /// <summary>
        /// Returns the contents of folder 'directory' as a list of DirectoryInfo.
        /// All returned files will have names that contain 'searchString'.
        /// The 'directory' path must start at the root of the project directory.
        /// </summary>
        public static List<FileInfo> GetFiles(string directory, string searchString)
        {
            var dirInfo = new DirectoryInfo(CWD + directory);
            Regex regex = new Regex(searchString, RegexOptions.IgnoreCase);
            var extensions = ImageExtensions;

            //Get all image files in folder that match search string
            return dirInfo.GetFiles().Where(f => extensions.Contains(
                f.Extension.ToLower()) && regex.IsMatch(f.Name)).ToList();
        }

        #endregion

        #region Images

        public static Image CreateThumbnail(Image image)
        {
            int height = 200;

            if (image.Height > height)
            {
                //If Image is bigger than height:
                //Determine aspect ratio
                float ratio = (float)image.Width / (float)image.Height;

                //Scale width to new height using aspect ratio
                int newWidth = (int)Math.Floor(height * ratio);

                //Return resized image
                return (Image)(new Bitmap(image, new Size(newWidth, height)));
            }
            else
            {
                //If Image is smaller than height:
                return (Image)(new Bitmap(image, new Size(image.Width, image.Height)));
            }
        }

        public static Image CreateMobileThumbnail(Image image)
        {
            int height = 50;

            if (image.Height > height)
            {
                //If Image is bigger than height:
                //Determine aspect ratio
                float ratio = (float)image.Width / (float)image.Height;

                //Scale width to new height using aspect ratio
                int newWidth = (int)Math.Floor(height * ratio);

                //Return resized image
                return (Image)(new Bitmap(image, new Size(newWidth, height)));
            }
            else
            {
                //If Image is smaller than height:
                return (Image)(new Bitmap(image, new Size(image.Width, image.Height)));
            }
        }

        /// <summary>
        /// Resizes the image to have a width of 'width' pixels, maintaining the aspect ratio
        /// </summary>
        public static Image ResizeImage(Image image, int width)
        {
            if (image.Width > width)
            {
                //If Image is bigger than height:
                //Determine aspect ratio
                float ratio = (float)image.Height / (float)image.Width;

                //Scale width to new height using aspect ratio
                int newHeight = (int)Math.Floor(width * ratio);

                //Return resized image
                return (Image)(new Bitmap(image, new Size(width, newHeight)));
            }
            else
            {
                //If Image is smaller than height:
                return (Image)(new Bitmap(image, new Size(image.Width, image.Height)));
            }
        }

        /// <summary>
        /// Saves the image at the specified 'path', with the specified 'extension'.
        /// Path must start at the working directory and must include the file's name.
        /// </summary>
        public static void SaveImage(Image img, string path, string extension)
        {
            if (extension == ".png")
            {
                img.Save(CWD + path + extension, ImageFormat.Png);
            }
            else if (extension == ".jpg")
            {
                img.Save(CWD + path + extension, ImageFormat.Jpeg);
            }
        }

        /// <summary>
        /// Saves multiple resized versions of the image, using the specified 'name' and 'extension'.
        /// </summary>
        public static bool SaveAllSizes(Image img, string name, string extension)
        {
            string path1 = "/wwwroot/Images/" + name;
            string path2 = "/wwwroot/Images/Resized/" + name;

            //var watch = System.Diagnostics.Stopwatch.StartNew();

            #region Threading Attempt
            //SaveImage(img, path1, extension);

            //WaitHandle[] waitHandles = new WaitHandle[6];
            //ImageResizer[] resizers = new ImageResizer[6];
            //int[] sizes = new int[] { 200, 400, 600, 800, 1000, 1200 };
            //string[] names = new string[] { "_thumb", "_400", "_600", "_800", "_1000", "_1200" };

            //for (int i = 0; i < 6; i++)
            //{
            //    resizers[i] = new ImageResizer(img, sizes[i], path2 + names[i], extension);
            //    waitHandles[i] = resizers[i].complete;
            //    resizers[i].Start();
            //}

            //WaitHandle.WaitAll(waitHandles);
            #endregion

            SaveImage(img, path1, extension);

            var newFile = new FileInfo(CWD + path1 + extension);
            
            if (newFile.Length > 10485760)
            {
                return false;
            }

            SaveImage(CreateThumbnail(img), path2 + "_thumb", extension);
            SaveImage(CreateMobileThumbnail(img), path2 + "_Mthumb", extension);
            SaveImage(ResizeImage(img, 400), path2 + "_400", extension);
            SaveImage(ResizeImage(img, 600), path2 + "_600", extension);
            SaveImage(ResizeImage(img, 800), path2 + "_800", extension);
            SaveImage(ResizeImage(img, 1000), path2 + "_1000", extension);
            SaveImage(ResizeImage(img, 1200), path2 + "_1200", extension);

            return true;

            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
        }

        //https://stackoverflow.com/questions/35027878/asp-net-core-image-upload-and-resize
        /// <summary>
        /// Returns a FormFile as an Image
        /// </summary>
        public static Image FormFileToImage(IFormFile file)
        {
            try
            {
                return Image.FromStream(file.OpenReadStream(), true, true);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot convert to Image!");
            }
        }

        /// <summary>
        /// Return the image at the specified 'path' as an Image
        /// </summary>
        public static Image LoadImage(string path)
        {
            return Image.FromFile(CWD + path);
        }

        /// <summary>
        /// Identifies the type of an image represented by a FormFile.
        /// </summary>
        public static string GetImageType(IFormFile file)
        {
            if (file.ContentType == "image/png")
            {
                return ".png";
            }
            else if (file.ContentType == "image/jpeg")
            {
                return ".jpg";
            }
            else
            {
                return "error";
            }
        }

        #endregion

        /// <summary>
        /// Returns the 'input' string, formatted to work with HTML
        /// </summary>
        public static string FormatHtmlString(string input)
        {
            return input.Replace(" ", "%20");
        }

        public static void DeleteImage(string image)
        {
            string name = Path.GetFileNameWithoutExtension(image);
            string extension = Path.GetExtension(image);
            string path1 = CWD + "/wwwroot/Images/";
            string path2 = CWD + "/wwwroot/Images/Resized/";

            try
            {
                File.Delete(path1 + name + extension);
                File.Delete(path2 + name + "_Mthumb" + extension);
                File.Delete(path2 + name + "_thumb" + extension);
                File.Delete(path2 + name + "_400" + extension);
                File.Delete(path2 + name + "_600" + extension);
                File.Delete(path2 + name + "_800" + extension);
                File.Delete(path2 + name + "_1000" + extension);
                File.Delete(path2 + name + "_1200" + extension);
            }
            catch
            {

            }
        }
    }
}
