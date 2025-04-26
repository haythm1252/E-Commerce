using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public static class FileConstants
    {
        public const string CategoryFolderPath = "assets/images/Categories";
        public const string ProductFolderPath = "assets/images/Products";
        public const string AllowedExtensions = ".jpg,.png,.jpeg";
        public const int MaxFileSizeInMP = 1;
        public const int MaxFileSizeInBytes = MaxFileSizeInMP * 1024 * 1024;
    }
}
