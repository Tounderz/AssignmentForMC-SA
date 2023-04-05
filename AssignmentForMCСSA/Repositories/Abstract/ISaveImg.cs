namespace AssignmentForMCСSA.Repositories.Abstract
{
    public interface ISaveImg
    {
        Task<string> SaveImg(IFormFile img);
    }
}
