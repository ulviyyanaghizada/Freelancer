using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Freelancer.Utilities.Extension
{
    public static class FileExtension
    {
        public static bool CheckType(this IFormFile file, string type) => file.ContentType.Contains(type);
        public static bool CheckSize(this IFormFile file, int kb) => kb * 1024 > file.Length;
        public static string CheckValidate(this IFormFile file,string type,int kb)
        {
            string result = "";
            if (!file.CheckType(type))
            {
                result += $"{file.FileName} tipi yanlishdir";
            }
            if (!file.CheckSize(kb))
            {
                result += $"{file.FileName} adli faylin hecmi {kb}dan chox olmamalidir";
            }
            return result;

        }
        public static string ChangeFileName(string oldname)
        {
            string extension = oldname.Substring(oldname.LastIndexOf("."));
            if (oldname.Length<32)
            {
                oldname.Substring(oldname.LastIndexOf("."));
            }
            else
            {
                oldname = oldname.Substring(0, 32);
            }
            return Guid.NewGuid().ToString() + extension + oldname;
        }
        public static string SaveFile(this IFormFile file,string path)
        {
            string filename = ChangeFileName(file.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path,filename),FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return filename;
        }

        public static void DeleteFile(this string file,string root,string folder)
        {
            string path = Path.Combine(root, folder, file);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
