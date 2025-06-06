IronZIP - The Zip Library for .NET
=============================================================
Quickstart:  https://ironsoftware.com/csharp/zip/

Compatibility
=============================================================
Supports applications and websites developed in:
- .NET Framework 4.6.2 (and above) for Windows, Linux, MacOs, Android, and Azure
- .NET Core 2, 3 (and above) Windows, Linux, MacOs, Android, and Azure
- .NET 5 for Windows, Linux, MacOs, Android, and Azure
- .NET 6 for Windows, Linux, MacOs, Android, and Azure
- .NET 7 for Windows, Linux, MacOs, Android, and Azure
- .NET 8 for Windows, Linux, MacOs, Android, and Azure


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


Documentation
=============================================================

- API Reference : https://ironsoftware.com/csharp/zip/object-reference/api/
- Licensing : https://ironsoftware.com/csharp/zip/licensing/
- Support : support@ironsoftware.com