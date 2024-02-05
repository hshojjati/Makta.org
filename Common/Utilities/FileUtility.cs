using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class FileUtility
    {
        private const string BaseUploadPath = "upload";
        private const string BaseFullUploadPath = "wwwroot\\upload";
        public static readonly string[] AllowedExtentions = { "txt", "pdf", "doc", "docx", "xls", "xlsx", "png", "jpg", "jpeg", "gif", "csv", "mp3", "mp4" };

        public static bool? FileExists(IFormFile file, out string fileName)
        {
            fileName = "";
            if (file != null)
            {
                var uploadPath = GetUploadDestination(PrepareFileName(file.FileName));
                fileName = uploadPath;
                var retValue = File.Exists(uploadPath);
                return retValue;
            }

            return null;
        }

        public static async Task<string> Upload(IFormFile file, string newName = null)
        {
            if (file != null && file.Length > 0 && AllowedExtentions.Contains(GetFileExtension(file.FileName)))
            {
                var uploadPath = GetUploadDestination(PrepareFileName(file.FileName, newName));
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream).ConfigureAwait(false);
                }

                var retValue = GetRelativeUrl(uploadPath);
                return retValue;
            }

            return null;
        }

        public static async Task<FormFile> Download(string filePath, string newName = null)
        {
            if (filePath.HasValue())
            {

                var path = GetPublicUrl(filePath);

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                var name = newName ?? GetFileName(filePath);
                return new FormFile(memory, 0, memory.Length, name, name);
            }

            return null;
        }

        private static string GetUploadDestination(string fileName)
        {
            var yearPath = Path.Combine(BaseFullUploadPath, DateTime.UtcNow.Year.ToString());
            if (!Directory.Exists(yearPath))
            {
                Directory.CreateDirectory(yearPath);
            }

            var monthPath = Path.Combine(yearPath, DateTime.UtcNow.Month.ToString());
            if (!Directory.Exists(monthPath))
            {
                Directory.CreateDirectory(monthPath);
            }

            var dayPath = Path.Combine(monthPath, DateTime.UtcNow.Day.ToString());
            if (!Directory.Exists(dayPath))
            {
                Directory.CreateDirectory(dayPath);
            }

            var retValue = Path.Combine(dayPath, $"{GetFileName(fileName)}_{DateTime.UtcNow.Ticks.ToString().Substring(0, 5)}.{GetFileExtension(fileName)}");
            return retValue;
        }

        public static long GetFileSize(string filePath)
        {
            if (filePath.HasValue())
            {
                if (File.Exists(filePath))
                    return new FileInfo(GetStaticUrl(filePath)).Length;
            }

            return 0;
        }

        public static string GetFileSizeToString(string filePath)
        {
            var size = GetFileSize(filePath);

            if (size > 0)
            {
                int iteration;
                for (iteration = 0; iteration < 3 && size > 1024; iteration++)
                {
                    size /= 1024;
                }

                switch (iteration)
                {
                    case 0:
                        return $"{size} B";
                    case 1:
                        return $"{size} KB";
                    case 2:
                        return $"{size} MB";
                    case 3:
                        return $"{size} GB";
                    default:
                        return $"{size} B";
                }
            }

            return null;
        }

        public static string GetPublicUrl(string filePath)
        {
            if (filePath.HasValue())
            {
                return $"/{BaseUploadPath}{filePath.Replace("/", @"\")}";
            }

            return null;
        }

        public static string GetStaticUrl(string filePath)
        {
            if (filePath.HasValue())
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), BaseFullUploadPath);
                return $"{path}{filePath.Replace("/", @"\")}";
            }

            return null;
        }

        public static string GetRelativeUrl(string uploadPath)
        {
            if (uploadPath.HasValue())
            {
                return uploadPath.Replace(BaseFullUploadPath, "").Replace(@"\", "/");
            }

            return null;
        }

        private static string PrepareFileName(string fileName, string newName = null)
        {
            if (fileName.HasValue())
            {
                var name = newName ?? GetFileName(fileName);
                var ext = GetFileExtension(fileName);
                var retvalue = $"{name.FixFileName()}.{ext}";
                return retvalue;
            }

            return null;
        }

        public static string GetFileExtension(string fileName)
        {
            if (fileName.HasValue())
            {
                return fileName.Split('.').LastOrDefault().ToLower();
            }

            return null;
        }

        public static string GetFileName(string filePath)
        {
            if (filePath.HasValue())
            {
                return string.Join(' ', filePath.Split('/').LastOrDefault().Split('.')[0..^1]);
            }

            return null;
        }

        public static string FixFileName(this string input)
        {
            return input.Trim().RemoveWhiteSpaces().ToLower()
                .Replace(' ', '-')
                .Replace('%', '-')
                .Replace(':', '-')
                .Replace('~', '-')
                .Replace('/', '-')
                .Replace('!', '-')
                .Replace('@', '-')
                .Replace('#', '-')
                .Replace('$', '-')
                .Replace('^', '-')
                .Replace('&', '-')
                .Replace('*', '-')
                .Replace('(', '-')
                .Replace(')', '-')
                .Replace('+', '-')
                .Replace('=', '-')
                .Replace('[', '-')
                .Replace(']', '-')
                .Replace('{', '-')
                .Replace('}', '-')
                .Replace(',', '-')
                .Replace(';', '-')
                .Replace('`', '-')
                .Replace('<', '-')
                .Replace('>', '-')
                .Replace('|', '-')
                .Replace('/', '-')
                .Replace('\\', '-');
        }

        public static string GetRandomSerialNumber()
        {
            var rndSerialNumber = new Random().Next(100000, 999999).ToString();
            return rndSerialNumber;
        }

        public static async Task UploadHCPicture(IFormFile file, string fileName)
        {
            if (file != null && file.Length > 0 && AllowedExtentions.Contains(GetFileExtension(file.FileName)))
            {
                var dir = Path.Combine(BaseFullUploadPath, "HCPictures");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var uploadPath = Path.Combine(dir, fileName);
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream).ConfigureAwait(false);
                }
            }
        }

        public static bool? FileExists(IFormFile file, double timeDifference)
        {
            if (file != null)
            {
                var uploadPath = GetUploadDestination(PrepareFileName(file.FileName));
                return File.Exists(uploadPath);
            }

            return null;
        }

        public static string GetHCPictureStaticUrl(int clientId)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), BaseFullUploadPath);
            path = Path.Combine(path, "HCPictures");

            var dir = new DirectoryInfo(path);
            var files = dir.GetFiles();
            foreach (var item in files)
            {
                if (item.Name == clientId.ToString() + item.Extension)
                {
                    var itemPath = Path.Combine(BaseUploadPath, "HCPictures");
                    itemPath = Path.Combine(itemPath, item.Name);
                    itemPath = itemPath.Replace(@"\", "/");
                    return itemPath;
                }
            }

            return "";
        }

        public static bool HasHCPicture(int clientId)
        {
            try
            {
                var path = Path.Combine(BaseFullUploadPath, "HCPictures");
                var dir = new DirectoryInfo(path);
                var files = dir.GetFiles();
                foreach (var item in files)
                {
                    if (item.Name == clientId.ToString() + item.Extension)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public static void Compress(Bitmap srcBitMap, string destFile, long level)
        {
            Stream s = new FileStream(destFile, FileMode.Create); //create FileStream,this will finally be used to create the new image 
            Compress(srcBitMap, s, level);  //main progress to compress image
            s.Close();
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private static void Compress(Bitmap srcBitmap, Stream destStream, long level)
        {
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            myImageCodecInfo = GetEncoderInfo("image/jpeg");
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;
            srcBitmap.Save(destStream, myImageCodecInfo, myEncoderParameters);
        }
    }
}