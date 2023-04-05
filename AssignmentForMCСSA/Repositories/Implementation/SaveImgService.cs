using AssignmentForMCСSA.Repositories.Abstract;

namespace AssignmentForMCСSA.Repositories.Implementation
{
    public class SaveImgService : ISaveImg
    {
        public async Task<string> SaveImg(IFormFile img)
        {
            if (img == null)
            {
                return string.Empty;
            }

            var fileName = Guid.NewGuid().ToString() + img.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fullPath = Path.Combine(path, fileName);
            var stream = new FileStream(fullPath, FileMode.Create);
            await img.CopyToAsync(stream);
            await stream.FlushAsync();

            return "/img/" + fileName;
        }
    }
}
