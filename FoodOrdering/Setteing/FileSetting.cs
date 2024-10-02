using Microsoft.EntityFrameworkCore.Storage;

namespace FoodOrdering.Setteing
{
    public static class FileSetting
    {
        public const string ImageCateogryPath = "/img/category/";
        public const string ImageProductPath = "/img/products/";
        public const string AllowedExtentions = ".jpg,.png,.jpeg";
        public const int MaxFileSizeInMB = 1;
        public const int MaxFileSizeInBytes = MaxFileSizeInMB *1024*1024 ;
    }
}
