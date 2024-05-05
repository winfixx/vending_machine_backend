namespace VendingMachine.Core.Services
{
    public class ImageService(string rootPath)
    {
        private readonly string rootPath = rootPath;

        public async Task<string> Save(Stream img, string fileName)
        {
            if (img == null || string.IsNullOrEmpty(fileName)) 
                throw new ArgumentNullException("Недостаток данных");

            fileName += ".jpg";

            using (var fileStream = new FileStream($"{rootPath}/" + fileName, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public async Task<string> Delete(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) 
                throw new ArgumentNullException("Недостаток данных");

            string path = $"{rootPath}/{fileName}";

            File.Delete(path);

            return path;
        }
    }
}
