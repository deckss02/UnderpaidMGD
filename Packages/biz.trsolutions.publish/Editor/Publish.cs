using System;
using System.IO;
using System.IO.Compression;
using UnityEditor;
using UnityEngine;

namespace UnityEditor.TRSolutions.Publish
{
    public class Publisher : MonoBehaviour {

        [MenuItem("File/Publish")]
        static void Publish() {
            var projectRoot = new DirectoryInfo(Directory.GetCurrentDirectory());
            print($"Publishing {projectRoot.Name}");
            var parent = projectRoot.Parent;
            if (parent == null) {
                print("Unable to get to parent folder of your project!");
                return;
            }
            var target = Path.Combine(parent.ToString(), projectRoot.Name + " Package");
            print(string.Format("Project will be published to {0}", target));
            Directory.CreateDirectory(target);
            // Upload the build (Build subfolder)
            var buildFolder = getBuildFolder(projectRoot);
            if (buildFolder == null)
            {
                Debug.LogWarning("No build folder found! Please save your executable build in the Build folder to include it in the submission!");
            } else
            {
                var buildTarget = Path.Combine(target, "Build.zip");
                using FileStream zipToOpen = new FileStream(buildTarget, FileMode.Create);
                using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create);
                new Compress("Build", archive).CompressFolder("/", buildFolder);
            }
            // Upload the main project
            var projectTarget = Path.Combine(target, projectRoot.Name + ".zip");
            using (FileStream zipToOpen = new FileStream(projectTarget, FileMode.Create)) {
                using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create); var ignoreParser = new IgnoreParser();
                ignoreParser.Initialise();
                //+ print($"IgnoreParser:\n{ignoreParser}");
                new Compress(projectRoot, archive, ignoreParser).CompressRoot();

                // Compress("/", cwd, ignoreParser, archive);
            }
            // Upload the gameplay video (Recording subfolder)
            if (Directory.Exists("Recordings"))
            {
                var recordingsFolder = new DirectoryInfo("Recordings");
                var recordingsTarget = Path.Combine(target, "Recordings.zip");
                using FileStream zipToOpen = new FileStream(recordingsTarget, FileMode.Create);
                using ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create);
                new Compress(recordingsFolder, archive, null).CompressRoot();
            }
            else
            {
                Debug.LogWarning("No Recordings folder found! Please save your recordings in the Recordings folder to include them in the submission!");
            }
            
        }

        private static DirectoryInfo getBuildFolder(DirectoryInfo projectRoot)
        {
            foreach (var fileInfo in projectRoot.EnumerateDirectories())
            {
                switch (fileInfo.Name)
                {
                    case "Build":
                    case "Builds":
                    case "build":
                    case "builds": return fileInfo;
                }
            }
            return null;
        }

        //// TODO: revise this into an instantiated class with common parms, so recursion takes up less stack space
        //// TODO: need also to keep project root so can strip FullName into relative name and relative name with slash
        //public static void Compress(string folderRelativePath, DirectoryInfo directorySelected, Publish.IgnoreParser ignoreParser, ZipArchive archive)
        //{
        //    print($"Compressing relative path {folderRelativePath}");

        //    foreach (var fileInfo in directorySelected.EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly))
        //    {
        //        //var path = fileInfo.FullName;
        //        //print(string.Format("Processing {0}", path));
        //        // Construct the path in a format that our git-like ignore function recognizes:
        //        // - "/" is the root of our project;
        //        // - Directories end in "/"
        //        // - Unix-style file separators
        //        var isDirectory = (File.GetAttributes(fileInfo.FullName) & FileAttributes.Directory) == FileAttributes.Directory;
        //        var fileRelativePath = folderRelativePath + fileInfo.Name + (isDirectory ? "/" : "");
        //        print($"Processing {fileRelativePath}");
        //        if (ignoreParser.IsIgnored(fileRelativePath))
        //        {
        //            print($"Skipping {fileRelativePath}");
        //        } else
        //        {
        //            if (isDirectory)
        //            {
        //                Compress(fileRelativePath, new DirectoryInfo(fileInfo.FullName), ignoreParser, archive);
        //            }
        //            else
        //            {
        //                print($"Compressing {fileRelativePath}");
        //                var zipArchiveEntry = archive.CreateEntry(fileRelativePath /* !!! NEEDS TO BE RELATIVE TO PROJECT! */);
        //                using (var zipArchiveStream = zipArchiveEntry.Open())
        //                {
        //                    using (var fileStream = File.OpenRead(fileInfo.FullName))
        //                    {
        //                        var buffer = new byte[5000];
        //                        int bytesRead;
        //                        do
        //                        {
        //                            bytesRead = fileStream.Read(buffer, 0, buffer.Length);
        //                            zipArchiveStream.Write(buffer, 0, bytesRead);
        //                        } while (bytesRead > 0);
        //                    }
        //                }

        //            }
        //        }
        //    }
        //}

        // Copied from https://github.com/codemix/gitignore-parser/blob/master/lib/index.js:
        // exports.parse = function (content) {
        //   return content.split('\n')
        //   .map(function (line) {
        //     line = line.trim();
        //     return line;
        //   })
        // .filter(function (line) {
        //   return line && line[0] !== '#';
        // })
        // .reduce(function (lists, line) {
        //   var isNegative = line[0] === '!';
        //   if (isNegative) {
        //     line = line.slice(1);
        //   }
        //   if (line[0] === '/')
        //     line = line.slice(1);
        //   if (isNegative) {
        //     lists[1].push(line);
        //   }
        //   else {
        //     lists[0].push(line);
        //   }
        //   return lists;
        // }, [[], []])
        // .map(function (list) {
        //   return list
        //   .sort()
        //   .map(prepareRegexes)
        //   .reduce(function (list, prepared) {
        //     list[0].push(prepared[0]);
        //     list[1].push(prepared[1]);
        //     return list;
        //   }, [[], [], []]);
        // })
        // .map(function (item) {
        //   return [
        //     item[0].length > 0 ? new RegExp('^((' + item[0].join(')|(') + '))') : new RegExp('$^'),
        //     item[1].length > 0 ? new RegExp('^((' + item[1].join(')|(') + '))') : new RegExp('$^')
        //   ]
        // });
        // };
        // function prepareRegexes (pattern) {
        //   return [
        //     // exact regex
        //     prepareRegexPattern(pattern),
        //     // partial regex
        //     preparePartialRegex(pattern)
        //   ];
        // }
        // function prepareRegexPattern (pattern) {
        //   return escapeRegex(pattern).replace('**', '(.+)').replace('*', '([^\\/]+)');
        // }
        // function preparePartialRegex (pattern) {
        //   return pattern
        //   .split('/')
        //   .map(function (item, index) {
        //     if (index)
        //       return '([\\/]?(' + prepareRegexPattern(item) + '\\b|$))';
        //     else
        //       return '(' + prepareRegexPattern(item) + '\\b)';
        //   })
        //   .join('');
        // }
        // function escapeRegex (pattern) {
        //   return pattern.replace(/[\-\[\]\/\{\}\(\)\+\?\.\\\^\$\|]/g, "\\$&");
        // }
    }      
}
