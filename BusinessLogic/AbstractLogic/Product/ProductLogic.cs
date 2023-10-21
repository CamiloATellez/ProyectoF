using DataTransferObjets.Configuration;
using DataTransferObjets.Dto;

namespace BusinessLogic.AbstractLogic.Product
{
    internal static class ProductLogic
    {
        public static void CreateDirectoryImages(string FullPath) 
        {
            if(Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);        
        }

        public static void EraseToWrite(string FullPath)
        {
            if (File.Exists(FullPath))
                File.Delete(FullPath);
        }

        public static ResponseImagesDto SavePicture(ImagesDto images)
        {
            string upload = images.UploadPath;
            string filename = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(images.Files[0].FileName);
            string fileFullPath = Path.Combine(upload, filename) + extension;

            using (var filestream = new FileStream(fileFullPath, FileMode.Create))
                images.Files[0].CopyTo(filestream);

            return new ResponseImagesDto()
            {
                SavePath = StaticDefination.ImagenPath + filename +extension,
                RequestResponse = true

            };

        }


    }
}
