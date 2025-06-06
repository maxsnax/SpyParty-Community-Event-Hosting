IronZIP is a library developed and maintained by Iron Software that helps C# Software Engineers to create, extract, and edit archive and zip files in .NET applications & websites

Visit our website for a quick-start guide at  https://ironsoftware.com/csharp/zip/

Extract Archive C# Code Example
=============================================================
using IronZip;

string file_path = "../archive.zip";
IronZipArchive.ExtractArchiveToDirectory(file_path, "output"); // Extracts files to folder 'unarchived'

Create Archive C# Code Example
=============================================================
using IronZip;

// Create a new Archive
using (var archive = new IronZipArchive())
{
    archive.Add("/assets/doc.pdf");
    archive.Add("/assets/img.png");
    archive.SaveAs("new.zip");
}

// Create a new Archive from an existing Archive
using (var archive = new IronZipArchive("existingArchive.zip"))
{
    archive.Add("/assets/doc.pdf");
    archive.Add("/assets/img.png");

    // Save into existing Archive
    archive.Save();

    // Save as a new Archive
    archive.SaveAs("new.zip");
}

Documentation Links
========================
API Reference : https://ironsoftware.com/csharp/zip/object-reference/api/
Licensing : https://ironsoftware.com/csharp/zip/licensing/
Support : support@ironsoftware.com

Compatibility
========================
* C#, F#, and VB.NET
* .NET 8, .NET 7, .NET 6, .NET 5, Core 2x & 3x, Standard 2.0 & 2.1, and Framework 4.6.2+
* Mobile, Console, Web, and Desktop Application
* Windows 10+, Mac 10.14, Linux (Debian, CentOS, Ubuntu), Android, iOS 12, Docker, Azure, and AWS
* Microsoft Visual Studio or Jetbrains ReSharper & Rider