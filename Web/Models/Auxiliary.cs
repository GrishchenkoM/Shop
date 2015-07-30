using System.IO;
using System.Web;
using Domain.Entities.Interfaces;

namespace Web.Models
{
    public static class Auxiliary
    {
        /// <summary>
        /// Read Image file
        /// </summary>
        /// <param name="uploadImage">Image file for upload</param>
        /// <param name="item">Item wich has to contain the image file</param>
        public static void ReadImage(HttpPostedFileBase uploadImage, object item)
        {
            if(item is IProduct)
                (item as IProduct).Image = ReadImage(uploadImage);
            else if(item is CreateProduct)
                (item as CreateProduct).Image = ReadImage(uploadImage);
        }
        
        private static byte[] ReadImage(HttpPostedFileBase uploadImage)
        {
            byte[] imageData;
            using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
            return imageData;
        }

        public enum Result
        {
            Error = -1, AdditionSuccess, OperationSuccess
        }
    }
}