using System.IO;
using System.Linq;
using Core;

namespace Control_Software.Getters
{
    public static class DirectoryGetter
    {
        public static string GetBaseDirectory(string directory)
        {
            var baseDirectory = Directory.GetParent(directory).FullName;
            var strings = baseDirectory.Split('\\');

            if (!strings.Last().Contains(Constants.ProgramBaseFolderName))
            {
                return GetBaseDirectory(baseDirectory);
            }
            return baseDirectory;
        }
    }
}
