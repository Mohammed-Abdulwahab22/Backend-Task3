using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Globalization;
using Task3.Models;

namespace Task3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
       

        private static List<f1> allFilesInformation = new List<f1>();


        [HttpPost("operations")]
        public IActionResult Main([FromForm] FileContent model)
        {


            var fileExtension = model.File != null && model.File.ContentType == "image/jpeg" ? "jpg" : "mp4";

            switch (model.OperationType)
            {
                case QueryType.Create:
                    if (model.File == null || model.FileName == null || model.Owner == null || model.OperationType == null)
                    {
                        return BadRequest("all fields required");
                    }
                    else
                    {
                        bool FileExistsc(string fileName, string fileExtension)
                        {

                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", $"{fileName}.{fileExtension}");
                            return System.IO.File.Exists(filePath);
                        }
                        if (FileExistsc(model.FileName, fileExtension))
                        {
                            return BadRequest("File already exist.");
                        }
                        if (model.File.ContentType != "image/jpeg" && model.File.ContentType != "video/mp4")
                        {
                            return BadRequest("File type not supported. Please upload a jpg or mp4 file.");
                        }


                        var filePath = Path.Combine("Files", $"{model.FileName}.{fileExtension}");
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            model.File.CopyTo(stream);
                        }

                        
                        var jsonContent = new
                        {

                            Filename = $"{model.FileName}.{fileExtension}",
                            Owner = model.Owner,
                            Description = model.Description,
                            

                        };

                        var jsonFilePath = Path.Combine("Files", $"{model.FileName}.json");
                        System.IO.File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(jsonContent));


                        allFilesInformation.Add(new f1
                        {
                            FileName = $"{model.FileName}.{fileExtension}", 
                            UserName = model.Owner,
                            CreationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            ModificationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),

                              
                        });

                        var allFilesJsonFilePath = Path.Combine("Files", "allFiles.json");

                        System.IO.File.WriteAllText(allFilesJsonFilePath, JsonConvert.SerializeObject(allFilesInformation));

                        return Ok(new
                        {
                            Message = "File created successfully.",
                            FileName = $"{model.FileName}.{fileExtension}",
                            Owner = model.Owner,
                            Description = model.Description
                        });


                    }




                case QueryType.Update:
                    var jsonFilePathToUpdate = Path.Combine("Files", $"{model.FileName}.json");
                    if (System.IO.File.Exists(jsonFilePathToUpdate))
                    {
                        var existingJsonContent = System.IO.File.ReadAllText(jsonFilePathToUpdate);
                        var existingMetadata = JsonConvert.DeserializeObject<FileContent>(existingJsonContent);






                        try
                        {


                            UpdateModificationDate(existingMetadata.FileName); 


                            if (model.File != null && model.Description == null)
                            {


                                var filePathu = Path.Combine("Files", $"{model.FileName}.{fileExtension}");
                                using (var stream = new FileStream(filePathu, FileMode.Create))
                                {
                                    model.File.CopyTo(stream);
                                }

                                var existingFileExtension = existingMetadata.FileName.Split('.').Last();
                                var newFileExtension = model.File.FileName.Split('.').Last();


                                var oldFilePath = Path.Combine("Files", $"{model.FileName}.{existingFileExtension}");
                                if (System.IO.File.Exists(oldFilePath))
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }

                                existingMetadata.FileName = $"{model.FileName}.{fileExtension}";

                            

                                System.IO.File.WriteAllText(jsonFilePathToUpdate, JsonConvert.SerializeObject(existingMetadata));
                            }
                            else if (model.File == null && model.Description != null)
                            {

                                existingMetadata.Description = model.Description;

                                System.IO.File.WriteAllText(jsonFilePathToUpdate, JsonConvert.SerializeObject(existingMetadata));
                            }
                            else if (model.File != null && model.Description != null)
                            {

                                var filePathu = Path.Combine("Files", $"{model.FileName}.{fileExtension}");
                                using (var stream = new FileStream(filePathu, FileMode.Create))
                                {
                                    model.File.CopyTo(stream);
                                }

                                var existingFileExtension = existingMetadata.FileName.Split('.').Last();
                                var newFileExtension = model.File.FileName.Split('.').Last();


                                var oldFilePath = Path.Combine("Files", $"{model.FileName}.{existingFileExtension}");
                                if (System.IO.File.Exists(oldFilePath))
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }

                                existingMetadata.FileName = $"{model.FileName}.{fileExtension}";
                                existingMetadata.Description = model.Description;

                                System.IO.File.WriteAllText(jsonFilePathToUpdate, JsonConvert.SerializeObject(existingMetadata));
                            }
                            else
                            {
                                return Ok("choose to change description or file.");
                            }
                        }


                        catch (Exception)
                        {

                            throw;
                        }

                    }
                    else
                    {
                        return Ok("file doesn't exist");
                    }




                    return Ok("File Updated");

                case QueryType.Delete:
                    var jsonFilePathTodelete = Path.Combine("Files", $"{model.FileName}.json");
                    if (System.IO.File.Exists(jsonFilePathTodelete))
                    {
                        if (model.File == null && model.Description == null)
                        {
                            var jsonFilePathTod1 = Path.Combine("Files", $"{model.FileName}.json");
                            var existingJsond1 = System.IO.File.ReadAllText(jsonFilePathTod1);
                            var existingMetad1 = JsonConvert.DeserializeObject<FileContent>(existingJsond1);

                            var fileToRemove = allFilesInformation.FirstOrDefault(f => f.FileName == existingMetad1.FileName);
                            if (fileToRemove != null)
                            {
                                allFilesInformation.Remove(fileToRemove);
                            }

                            var fileToDelete = Path.Combine("Files", $"{model.FileName}.{existingMetad1.FileName.Split('.').Last()}");
                            var jsonToDelete = Path.Combine("Files", $"{model.FileName}.json");
                            System.IO.File.Delete(fileToDelete);
                            System.IO.File.Delete(jsonToDelete);

                            UpdateAllFilesJson();

                            return Ok("File deleted successfully.");

                        }
                        else
                        {
                            return Ok("only file name and owner");
                        }

                    }
                    else
                    {
                        return Ok("file doesn't exist");
                    }



                case QueryType.Retrieve:

                    var jsonFilePathToret = Path.Combine("Files", $"{model.FileName}.json");
                    if (System.IO.File.Exists(jsonFilePathToret))
                    {
                        UpdateModificationDate(model.FileName); 

                        if (model.File == null && model.Description == null)
                        {

                            var jsonFilePathTor = Path.Combine("Files", $"{model.FileName}.json");
                            var existingJsonr = System.IO.File.ReadAllText(jsonFilePathTor);
                            var existingMetad = JsonConvert.DeserializeObject<FileContent>(existingJsonr);
                            var dd = existingMetad.FileName.Split('.').Last();

                            var filePath1 = Path.Combine("Files", $"{model.FileName}.{dd}");
                            var jsonFilePath1 = Path.Combine("Files", $"{model.FileName}.json");

                            if (!System.IO.File.Exists(filePath1) || !System.IO.File.Exists(jsonFilePath1))
                            {
                                return BadRequest("File or metadata not found.");
                            }

                            var fileContent = System.IO.File.ReadAllBytes(filePath1);
                            var jsonContent1 = System.IO.File.ReadAllText(jsonFilePath1);

                            var fileStream = new FileStream(filePath1, FileMode.Open, FileAccess.Read);



                            var fileResponse = new
                            {
                                FileContent = Convert.ToBase64String(fileContent),
                                FileName = model.FileName,
                                FileOwner = model.Owner,
                                Description = JsonConvert.DeserializeObject<FileContent>(jsonContent1).Description
                            };

                          

                            Response.Headers.Add("File Name", $"{model.FileName}.{dd}");
                            Response.Headers.Add("File-Owner", model.Owner);
                            Response.Headers.Add("File-Description", JsonConvert.DeserializeObject<FileContent>(jsonContent1).Description);
                            return Ok(fileStream);
                        }
                        else
                        {
                            return Ok("only filname and owner");
                        }

                       

                    }
                    else
                    {
                        return Ok("File doesn't exist");
                    }

                default:
                    return Ok("Invalid operation type.");
            }

        }
        [HttpPost("filter")]
        public IActionResult filter([FromForm] filter1 filterCriteria)
        {
            try
            {
                if (filterCriteria.startdate == null || filterCriteria.enddate == null)
                {
                    return BadRequest("Start Date and End Date are required");
                }
                // Specify the expected date format
                string dateFormat = "yyyy-MM-dd HH:mm:ss";


                var filteredFiles = allFilesInformation
                    .Where(file =>
                    {
                        if (DateTime.TryParse(file.CreationDate, out var creationDate))
                        {
                            return creationDate >= filterCriteria.startdate && creationDate <= filterCriteria.enddate;
                        }
                        return false;
                    });

                /*.OrderBy(file =>
                    filterCriteria.sorttype == SortType.ascending
                        ? DateTime.ParseExact(file.CreationDate, dateFormat, CultureInfo.InvariantCulture) 
                        : DateTime.ParseExact(file.CreationDate, dateFormat, CultureInfo.InvariantCulture).AddYears(-1000))
                .Select(file => new
                {
                    FileName = file.FileName,
                    CreationDate = file.CreationDate,
                    ModificationDate = file.ModificationDate
                });*/


                if (filterCriteria.sort_by_creation_date == SortTypeByCreationDate.ascendingc)
                {
                    filteredFiles = filteredFiles.OrderBy(file => DateTime.ParseExact(file.CreationDate, dateFormat, CultureInfo.InvariantCulture));

                }
                else if(filterCriteria.sort_by_creation_date == SortTypeByCreationDate.descendingc)
                {
                    filteredFiles = filteredFiles.OrderByDescending(file => DateTime.ParseExact(file.CreationDate, dateFormat, CultureInfo.InvariantCulture));

                }
                else if(filterCriteria.sort_by_modification_date == SortTypeByModificationDate.ascendingm)
                {
                    filteredFiles = filteredFiles.OrderBy(file => DateTime.ParseExact(file.ModificationDate, dateFormat, CultureInfo.InvariantCulture));

                }
                else if(filterCriteria.sort_by_modification_date == SortTypeByModificationDate.descendingm)
                {
                    filteredFiles = filteredFiles.OrderByDescending(file => DateTime.ParseExact(file.ModificationDate, dateFormat, CultureInfo.InvariantCulture));

                }

                var result = filteredFiles.Select(file => new
                {
                    FileName = file.FileName,
                    CreationDate = file.CreationDate,
                    ModificationDate = file.ModificationDate
                });

                return Ok(result);


            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
       
        private void UpdateAllFilesJson()
        {
            var allFilesJsonFilePath = Path.Combine("Files", "allFiles.json");
            System.IO.File.WriteAllText(allFilesJsonFilePath, JsonConvert.SerializeObject(allFilesInformation));
        }

        private void UpdateModificationDate(string fileName)
        {
            var fileToUpdate = allFilesInformation.FirstOrDefault(f => f.FileName == fileName);
            if (fileToUpdate != null)
            {
                fileToUpdate.ModificationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }

            UpdateAllFilesJson(); 
        }



        [HttpPost("FilterByUsers")]
        public IActionResult filterUsers([FromForm] FilterByUserRequest filterByUserRequest)
        {
            if (filterByUserRequest.Usernames != null )
            {
                var filteredFiles = allFilesInformation
                .Where(file =>
                {
                    filterByUserRequest.Usernames.Contains(file.UserName);
                    if (DateTime.TryParse(file.CreationDate, out var creationDate))
                    {
                        return creationDate >= filterByUserRequest.startdate && creationDate <= filterByUserRequest.enddate;
                    }
                    return false;


                });

               

                string dateFormat = "yyyy-MM-dd HH:mm:ss";


                if (filterByUserRequest.sort_creation_date == SortTypeByCreationDateuser.ascendinguser1)
                {
                    filteredFiles = filteredFiles.OrderBy(file => DateTime.ParseExact(file.CreationDate, dateFormat, CultureInfo.InvariantCulture));

                }
                else if (filterByUserRequest.sort_creation_date == SortTypeByCreationDateuser.descendinguser1)
                {
                    filteredFiles = filteredFiles.OrderByDescending(file => DateTime.ParseExact(file.CreationDate, dateFormat, CultureInfo.InvariantCulture));

                }
                else if (filterByUserRequest.sort_modification_date == SortTypeByModificationDateuser.ascendingm1)
                {
                    filteredFiles = filteredFiles.OrderBy(file => DateTime.ParseExact(file.ModificationDate, dateFormat, CultureInfo.InvariantCulture));

                }
                else if (filterByUserRequest.sort_modification_date == SortTypeByModificationDateuser.descendingm1)
                {
                    filteredFiles = filteredFiles.OrderByDescending(file => DateTime.ParseExact(file.ModificationDate, dateFormat, CultureInfo.InvariantCulture));

                }

                var result = filteredFiles.Select(file => new
                {
                    FileName = file.FileName,
                    Username = file.UserName,
                    //Description = JsonConvert.DeserializeObject<FileContent>(System.IO.File.ReadAllText(Path.Combine("Files", $"{file.FileName}.json"))).Description,
                    CreationDate = file.CreationDate,
                    ModificationDate = file.ModificationDate
                });

                return Ok(result);
            }
            else
            {
                return BadRequest("Please Enter atleast one user name");
            }
            


        }

    }
}
