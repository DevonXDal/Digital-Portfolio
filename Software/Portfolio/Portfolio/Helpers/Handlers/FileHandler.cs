using Portfolio.Data;
using Portfolio.Models.MainDb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Portfolio.Repositories.Interfaces;

namespace Portfolio.Helpers.Handlers
{
    /// <summary>
    /// Handles the management of the filesystem
    /// </summary>
    public class FileHandler
    {
        private readonly IRepo<RelatedFile> _relatedFileRepo;
        private readonly Env _env;
        private readonly ILogger<FileHandler> _logger;

        public FileHandler(IRepo<RelatedFile> relatedFileRepo, Env env, ILogger<FileHandler> logger)
        {
            _relatedFileRepo = relatedFileRepo;
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// Creates the related file on the file system and makes a database association to it.
        /// </summary>
        /// <param name="file">The file to be created.</param>
        /// <param name="alteredFileName">A different file name to use</param>
        /// <returns>-1 if the file could not be created or the id of the new file.</returns>
        public int CreateRelatedFile(
            IFormFile file, 
            string? alteredFileName = null)
        {
            if (!TryFileCreationValidation(file))
            {
                return -1;
            }

            string filename = Regex.Replace(file.FileName, "/[/A-Za-z0-9-_ ]+\\//g", ""); //Used regexr.com
            if (alteredFileName is not null)
            {
                filename = alteredFileName;
            }

            string fileStoragePath = _env.FileStoragePath;
            string fileExtension = Path.GetExtension(filename); //= Regex.Match(file.FileName, "/\\..*$/g").ToString();
            string filenameGuid = Guid.NewGuid().ToString() + fileExtension;

            // Todays date written using underscores to not mess with the file name (Ex: 5-16-2001)
            string todaysDate = DateTime.UtcNow.Date.ToShortDateString().Replace('/', '_');
            string fullFilePath = Path.Combine(fileStoragePath, todaysDate, filenameGuid);
            string relativePath = $"/filesystem/{todaysDate}/{filenameGuid}";

            FileStream fileStream = null;
            bool writtenSuccessfully = false;
            try
            {
                //https://stackoverflow.com/questions/39322085/how-to-save-iformfile-to-disk
                //https://www.completecsharptutorial.com/basic/c-file-handling-programming-examples-and-practice-question.php
                DirectoryInfo directory = new DirectoryInfo($"{fileStoragePath}/{todaysDate}");
                if (!directory.Exists)
                {
                    directory.Create();
                }

                fileStream = new FileStream(fullFilePath, FileMode.Create);
                file.CopyToAsync(fileStream).Wait();
                writtenSuccessfully = true;

            }
            catch (IOException e)
            {
                _logger.LogError(e, $"IO error writing file: {filename}");
            }
            finally
            {
                if (fileStream is not null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }

            if (!writtenSuccessfully)
            {
                return -1; //File not stored
            }

            RelatedFile entity = new RelatedFile
            {
                FilePath = relativePath,
                FileName = filename,
            };

            _relatedFileRepo.Insert(entity);

            return entity.Id;
        }

        /// <summary>
        /// Determines whether the file is an image.
        /// </summary>
        /// <param name="file">The file to check</param>
        /// <returns>
        ///   <c>true</c> if the file is an image otherwise, <c>false</c>.
        /// </returns>
        public bool IsRelatedFileImage(IFormFile file) => file.ContentType.Contains("image/", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the file and returns its byte array form.
        /// </summary>
        /// <param name="file">The related file mapping entity from the database</param>
        /// <returns>The byte array holding file data</returns>
        public byte[] GetFileAsBytes(RelatedFile file)
        {
            string filePath = GetAbsoluteFilePathOfRelatedFile(file);


            var mimeType = GetMimeType(filePath);

            byte[] fileBytes = null;
            try
            {
                fileBytes = File.ReadAllBytes(Path.GetFullPath(filePath));
                if (!File.Exists(filePath))
                {
                    _logger.LogError($"The file was not found on the system and could not be downloaded, path was: {filePath}");
                }
            }
            catch (IOException e)
            {
                _logger.LogError(e, $"An IOException was thrown attempting to access the file at path: {filePath}");
            }


            return fileBytes;
        }

        /// <summary>
        /// Gets the file an returns it as a file download result.
        /// </summary>
        /// <param name="file">The related file mapping entity from the database</param>
        /// <returns>The result to download a file on the client side, null if not found</returns>
        public FileContentResult? GetFileResult(RelatedFile file)
        {
            string filePath = GetAbsoluteFilePathOfRelatedFile(file);

            var mimeType = GetMimeType(filePath);

            byte[] fileBytes = null;
            try
            {
                fileBytes = File.ReadAllBytes(Path.GetFullPath(filePath));
                if (!File.Exists(filePath))
                {
                    _logger.LogError($"The file was not found on the system and could not be downloaded, path was: {filePath}");
                }
            }
            catch (IOException e)
            {
                _logger.LogError(e, $"An IOException was thrown attempting to access the file at path: {filePath}");
            }


            return new FileContentResult(fileBytes, mimeType);
        }

        /// <summary>
        /// Takes a RelatedFile and deletes its entity in the database and the actual file. 
        /// If it fails to delete the file then the RelatedFile entity is deleted and a log is created.
        /// </summary>
        /// <param name="file">The RelatedFile that should be removed from the system</param>
        public async Task DeleteFile(RelatedFile file)
        {
            try
            {
                string filePath = GetAbsoluteFilePathOfRelatedFile(file);
                if (!filePath.Contains("wwwroot"))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to delete fail for RelatedFile, will leave file in filesystem storage.");
            }
            finally
            {
                _relatedFileRepo.Delete(_relatedFileRepo.GetByID(file.Id));
            }
        }

        //Returns the Mime type of the file (helps identify the type of file)
        private string GetMimeType(string fileName)
        {
            //https://stackoverflow.com/questions/45727856/how-to-download-a-file-in-asp-net-core
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        //Returns the absolute filepath depending on whether the filesystem path or wwwroot
        public String GetAbsoluteFilePathOfRelatedFile(RelatedFile file)
        {
            if (file.FilePath.StartsWith("./") || file.FilePath.StartsWith("/filesystem/"))
            {
                return Path.Combine(ReadFileStoragePath(), file.FilePath.Replace("./", "").Replace("/filesystem/", ""));
            }
            else
            {
                return Path.Combine("./wwwroot", file.FilePath);
            }
        }

        // ===== GENERAL FILE OPERATIONS =====
        
        /// <summary>
        /// Creates a directory using the specified relative path (use './').
        /// Use a path name like: "./parentDirectoryThatMayNotExist/DeepestNeededDirectory/"
        /// It will be mapped to the correct absolute path.
        /// </summary>
        /// <param name="directoryPath">The relative path beginning in "./"</param>
        public void CreateDirectory(string directoryPath)
        {
            try
            { 
                DirectoryInfo directory = new DirectoryInfo(ConvertRelativePathToAbsolute(directoryPath));
                if (!directory.Exists)
                {
                    directory.Create();
                }
            }
            catch (IOException e)
            {
                _logger.LogError(e, $"IO error creating a directory");
            }
        }

        /// <summary>
        /// Creates a file given the provided relative path and byte array.
        /// An error is logged if the file was not created successfully. 
        /// A directory is created if needed.
        /// This overwrites already existing files.
        /// 
        /// The file path should appear like "./path/to/file.png" in order to process correctly.
        /// </summary>
        /// <param name="relativeFileNameAndPath">The relative path starting with "./" and file name/extension</param>
        /// <param name="fileBytes">The data to be stored as a file</param>
        /// <returns>true if the file was created at the path successfully</returns>
        public async Task<bool> CreateFileAtPath(string relativeFileNameAndPath, byte[] fileBytes)
        {
            FileStream fileStream = null;
            bool writtenSuccessfully = false;
            string absolutePath = ConvertRelativePathToAbsolute(relativeFileNameAndPath);
            string directoryPath = DirectoryPathOfFile(absolutePath);

            if (fileBytes.Length == 0 || absolutePath.Length == 0)
            {
                return false; // Needs a file path and data for that path
            }

            try
            {
                //https://stackoverflow.com/questions/39322085/how-to-save-iformfile-to-disk
                //https://www.completecsharptutorial.com/basic/c-file-handling-programming-examples-and-practice-question.php
                DirectoryInfo directory = new DirectoryInfo(directoryPath);
                if (!directory.Exists)
                {
                    directory.Create();
                }

                fileStream = new FileStream(absolutePath, FileMode.Create);
                await fileStream.WriteAsync(fileBytes);
                writtenSuccessfully = true;

            }
            catch (IOException e)
            {
                _logger.LogError(e, $"IO error writing file: {absolutePath}, for non-RelatedFile");
            }
            finally
            {
                if (fileStream is not null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }

            return writtenSuccessfully;
        }

        /// <summary>
        /// USE WITH CAUTION: Takes a relative directory path like "./directory/path/" and 
        /// removes all files and subdirectories from it, leaving the directory empty but still 
        /// existing.
        /// 
        /// If the target directory does not exist, it is simply created and no clearing is done.
        /// 
        /// Built from Andrew Morton's answer: https://stackoverflow.com/questions/1288718/how-to-delete-all-files-and-folders-in-a-directory?page=1&tab=scoredesc#tab-top
        /// </summary>
        /// <param name="relativeDirectoryPath">The relative path to the directory to target</param>
        /// <returns>true if all subdirectories and files are deleted</returns>
        public async Task<bool> ClearRelativeDirectoryOfFilesOrMakeNew(string relativeDirectoryPath)
        {
            DirectoryInfo di = new DirectoryInfo(ConvertRelativePathToAbsolute(relativeDirectoryPath));

            try
            {
                if (!di.Exists)
                {
                    Directory.CreateDirectory(di.FullName);
                    return true;
                }

                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    if (await ClearRelativeDirectoryOfFilesOrMakeNew(dir.FullName))
                    {
                        dir.Delete(true);
                    } else return false;
                }
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
            } catch (IOException e)
            {
                _logger.LogError(e, $"Could not remove all files from specfied directory: {di.FullName}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Copies all files and subdirectories from one directory path to another.
        /// If the directory path has "./" it will be replaced with the absolute file path
        /// given to this Web server for client file storage.
        /// 
        /// From ShloEmi's answer at: https://stackoverflow.com/questions/7146021/copy-all-files-in-directory
        /// </summary>
        /// <param name="fromDirectoryPath">The path to pull files and directories from</param>
        /// <param name="toDirectoryPath">The path to push files and directories to</param>
        /// <returns>true if the process was completed successfully</returns>
        public async Task<bool> CopyFilesBetweenDirectories(string fromDirectoryPath, string toDirectoryPath)
        {
            fromDirectoryPath = ConvertRelativePathToAbsolute(fromDirectoryPath);
            toDirectoryPath = ConvertRelativePathToAbsolute(toDirectoryPath);

            try
            {
                FileSystem.CopyDirectory(fromDirectoryPath, toDirectoryPath);
                return true;
            } catch (Exception e)
            {
                _logger.LogError(e, "Could not successfully copy from one directory: " +
                    $"{fromDirectoryPath} to another: {toDirectoryPath}");
            }
            return false;
            
        }

        /// <summary>
        /// Copies a file from one directory to another
        /// If a file path has "./" it will be replaced with the absolute file path
        /// given to this Web server for client file storage.
        /// 
        /// </summary>
        /// <param name="fromFilePath">The path to pull the file from</param>
        /// <param name="toFilePath">The path to copy the file to</param>
        /// <returns>true if the process was completed successfully</returns>
        public async Task<bool> CopyFile(string fromFilePath, string toFilePath)
        {
            fromFilePath = ConvertRelativePathToAbsolute(fromFilePath);
            toFilePath = ConvertRelativePathToAbsolute(toFilePath);

            try
            {
                FileSystem.CopyFile(fromFilePath, toFilePath, overwrite: true);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not successfully copy a file from: " +
                    $"{fromFilePath} to: {toFilePath}");
            }
            return false;

        }

        /// <summary>
        /// Reads a txt-type file to a string and returns the string.
        /// 
        /// "./" is converted to point at the relative path's true absolute path for
        /// Web server stored user files.
        /// </summary>
        /// <param name="fileNameAndPath"></param>
        /// <returns>A string with the file contents or null if the read was unsuccessful</returns>
        public async Task<string?> ReadTextFileToString(string fileNameAndPath)
        {
            fileNameAndPath = ConvertRelativePathToAbsolute(fileNameAndPath);
            string? fileTextContents = null;

            try
            {
                
                if (!File.Exists(fileNameAndPath))
                {
                    _logger.LogError($"The file was not found on the system and could not be read to string, path was: {fileNameAndPath}");
                }
                fileTextContents = await File.ReadAllTextAsync(fileNameAndPath);
            } catch (IOException e)
            {
                _logger.LogError(e, $"An IOException was thrown attempting to read the file to a string at path: {fileNameAndPath}");
            }

            return fileTextContents;
        }

        /// <summary>
        /// Provides the file storage path that should used to store user files
        /// </summary>
        /// <returns>The file path that user/runtime files should be stored to</returns>
        public string ReadFileStoragePath()
        {
            return _env.FileStoragePath;
        }
        
        /// <summary>
        /// Takes an array of bytes representing a file and converts it into Base64 form.
        /// </summary>
        /// <returns>The Base64 form of the file</returns>
        public string ToBase64(byte[] fileBytes)
        {
            return Convert.ToBase64String(fileBytes);
        }

        //Uses the arguments for the parameters to do similar validation. Returns false if validation failed.
        private bool TryFileCreationValidation(Object file)
        {
            if (file is null)
            {
                _logger.LogWarning("No file sent to be manipulated");
                return false; //No file
            } else return true;
        }

        /// <summary>
        /// Converts /filesystem and ./ to their correct absolute path for the given
        /// relative path.
        /// 
        /// This also ensures the path is correct for the host OS.
        /// </summary>
        /// <param name="relativePath">The relative path to convert</param>
        /// <returns>The absolute path</returns>
        public string ConvertRelativePathToAbsolute(string relativePath)
        {
            return Path.GetFullPath(
                relativePath
                .Replace("\\", "/")
                .Replace("/filesystem/", ReadFileStoragePath())
                .Replace("./", ReadFileStoragePath()));
        }

        // Gets the directory from a path that points to a file. Returns root relative path if no directory given
        private string DirectoryPathOfFile(string pathWithFileName)
        {
            pathWithFileName = pathWithFileName.Replace("\\", "/"); // Prevent windows path from causing issues.
            for (int i = pathWithFileName.Length - 1; i >= 0; i--)
            {
                if (pathWithFileName[i] == '/')
                {
                    return pathWithFileName.Substring(0, i + 1);
                }
            }

            return "./"; // Return root relative directory as fail safe
        }
    }
}
