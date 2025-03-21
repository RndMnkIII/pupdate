using Pannella.Helpers;
using Pannella.Models.Settings;
using ArchiveFile = Pannella.Models.Archive.File;

namespace Pannella;

internal static partial class Program
{
    private static void DownloadPockLibraryImages()
    {
        const string fileName = "Library_Image_Set_v1.0.zip";
        Archive archive = ServiceHelper.ArchiveService.GetArchive();
        ArchiveFile archiveFile = ServiceHelper.ArchiveService.GetArchiveFile(fileName);

        if (archiveFile != null)
        {
            string localFile = Path.Combine(ServiceHelper.TempDirectory, fileName);
            string extractPath = Path.Combine(ServiceHelper.TempDirectory, "temp");

            try
            {
                Console.WriteLine("Downloading library images...");
                ServiceHelper.ArchiveService.DownloadArchiveFile(archive, archiveFile, ServiceHelper.TempDirectory);
                Console.WriteLine("Installing library images...");

                if (Directory.Exists(extractPath))
                    Directory.Delete(extractPath, true);

                ZipHelper.ExtractToDirectory(localFile, extractPath);
                File.Delete(localFile);
                Util.CopyDirectory(extractPath, ServiceHelper.UpdateDirectory, true, true);

                Directory.Delete(extractPath, true);
                Console.WriteLine("Complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something happened while trying to install the asset files...");
                Console.WriteLine(ServiceHelper.SettingsService.Debug.show_stack_traces
                    ? ex
                    : Util.GetExceptionMessage(ex));
            }
        }
        else
        {
            Console.WriteLine("Pocket Library Images not found in the archive.");
        }
    }
}
