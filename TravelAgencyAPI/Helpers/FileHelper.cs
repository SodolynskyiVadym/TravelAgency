namespace TravelAgencyAPI.Helpers;

public class FileHelper
{
    // private static string _pathBooks = @"../book_store_front/src/assets/bookPhoto";
    // private static string _pathAuthors = @"../book_store_front/src/assets/authorPhoto";
    // private static string _pathPublishers = @"../book_store_front/src/assets/publisherPhoto";
    private static string _path = @"../travel_agency_front\src\assets\images";


    public void RenamePhoto(string? oldName, string newName, int id, string folderName)
    {
        if (oldName != null)
        {
            string oldFileName = oldName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";
            string newFileName = newName.Replace(" ", "").ToLower() + id.ToString() + ".jpg";


            if (File.Exists($"{_path}/{folderName}/{oldFileName}"))
            {
                File.Move($"{_path}/{folderName}/{oldFileName}", $"{_path}/{folderName}/{newFileName}");
            }
        } 
    }


    public void DeletePhoto(string name, int id, string folderName)
    {

        if (File.Exists($"{_path}/{folderName}/{name}")) 
        { 
            File.Delete($"{_path}/{folderName}/{name}");
        }
    }
}