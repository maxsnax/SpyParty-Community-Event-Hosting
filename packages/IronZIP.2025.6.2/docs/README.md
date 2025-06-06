![Nuget](https://img.shields.io/nuget/v/IronZIP?color=informational&label=latest)  ![Installs](https://img.shields.io/nuget/dt/IronZIP?color=informational&label=installs&logo=nuget)  ![Passed](https://img.shields.io/badge/build-%20%E2%9C%93%20382%20tests%20passed%20(0%20failed)%20-107C10?logo=visualstudio) ![windows](https://img.shields.io/badge/%E2%80%8E%20-%20%E2%9C%93-107C10?logo=windows) ![linux](https://img.shields.io/badge/%E2%80%8E%20-%20%E2%9C%93-107C10?logo=linux&logoColor=white) ![macOS](https://img.shields.io/badge/%E2%80%8E%20-%20%E2%9C%93-107C10?logo=apple) ![microsoftazure](https://img.shields.io/badge/%E2%80%8E%20-%20%E2%9C%93-107C10?logo=microsoftazure) ![docker](https://img.shields.io/badge/%E2%80%8E%20-%20%E2%9C%93-107C10?logo=docker&logoColor=white) ![aws](https://img.shields.io/badge/%E2%80%8E%20-%20%E2%9C%93-107C10?logo=amazonaws) [![livechat](https://img.shields.io/badge/Live%20Chat-8%20Engineers%20Active%20Today-purple?logo=googlechat&logoColor=white)](https://ironsoftware.com/csharp/zip/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=topshield#helpscout-support)

# IronZIP - The C# ZIP Archive Library

[![IronZIP NuGet Trial Banner Image](https://raw.githubusercontent.com/iron-software/iron-nuget-assets/main/IronZIP-readme/nuget-trial-banner.png)](https://ironsoftware.com/csharp/zip/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=topbanner#trial-license)

IronZIP is a library developed and maintained by Iron Software that helps C# Software Engineers to create, edit, and save Archives easily.

### IronZIP excels at:
- Archive various formats: Images (jpg, png, gif, tiff, svg, bmp), Text Files, Documents (PDFs, DOCX, XLSX), Audio (MP3, WAV), and even nested ZIP archives.
- Safeguard your archive with passwords: Supports Traditional, AES128, and AES256 encryption.
- Open password-protected ZIP archives.
- Extract the contents of ZIP, TAR, GZIP, and BZIP2 archives.
- Choose from 9 compression levels.
- Add files to existing archives.

And many more! *Visit [our website](https://ironsoftware.com/csharp/zip/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=featureslist) to see all our [code examples](https://ironsoftware.com/csharp/zip/examples/create-zip/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=featureslist) and a [full list of our features](https://ironsoftware.com/csharp/zip/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=featureslist)*

### IronZIP has cross platform support compatibility with:
- .NET 8, .NET 7, .NET 6 and .NET 5, Core 2x & 3x, Standard 2, and Framework 4.6.2+
- Windows, macOS, Linux, Docker, Azure, and AWS

[![IronZIP Cross Platform Compatibility Support Image](https://raw.githubusercontent.com/iron-software/iron-nuget-assets/main/IronZIP-readme/cross-platform-compatibility.png)](https://ironsoftware.com/csharp/zip/docs/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=crossplatformbanner)

Additionally, our [full licensing information](https://ironsoftware.com/csharp/zip/licensing/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=supportanddocs#trial-license) can easily be found on our website.

## Using IronZIP

Installing the IronZIP NuGet package is quick and easy, please install the package like this:
```
PM> Install-Package IronZIP
```
Once installed, you can get started by adding `using IronZip;` to the top of your C# code. Here are examples to get started:
```
using IronZip;

string file_path = "./archive.zip";
IronZipArchive.ExtractArchiveToDirectory(file_path, "unarchived"); // Extracts files to folder 'unarchived'

// Create a new ZIP archive
using (var archive = new IronZipArchive())
{
    // Add entries
    archive.Add("./assets/doc.pdf");
    archive.Add("./assets/img.png");

    // Export ZIP file
    archive.SaveAs("./output/archive.zip");
}
```

## Features

#### Building Archives from Files
- Images (JPG, PNG, GIF, TIFF, SVG, BMP)
- Text files
- Documents (PDF, DOCX, XLSX)
- Audio (MP3, WAV)
- Other Archives

#### Opening Archives
- ZIP
- TAR
- GZIP
- BZIP2

#### Export Archives
- ZIP
- TAR
- GZIP
- BZIP2

## Licensing & Support available
For code examples, tutorials and documentation visit https://ironsoftware.com/csharp/zip/

For support please email us at support@ironsoftware.com 

## Documentation Links
- How-To Guides : [https://ironsoftware.com/csharp/zip/how-to/](https://ironsoftware.com/csharp/zip/how-to/license-keys/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=supportanddocs)
- Code Examples : [https://ironsoftware.com/csharp/zip/examples/](https://ironsoftware.com/csharp/zip/examples/create-zip/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=supportanddocs)
- Tutorials : [https://ironsoftware.com/csharp/zip/tutorials/](https://ironsoftware.com/csharp/zip/tutorials/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=supportanddocs)
- Licensing : [https://ironsoftware.com/csharp/zip/licensing/](https://ironsoftware.com/csharp/zip/licensing/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=supportanddocs)
- Live Chat Support : [https://ironsoftware.com/csharp/zip/#helpscout-support](https://ironsoftware.com/csharp/zip/?utm_source=nuget&utm_medium=organic&utm_campaign=readme&utm_content=supportanddocs#helpscout-support)
