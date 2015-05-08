﻿using System;
using System.Collections.Generic;
using System.IO;

namespace ScriptCs.Dnx.Contracts
{
    public interface IFilePreProcessor : IFileParser
    {
        FilePreProcessorResult ProcessFile(string path);

        FilePreProcessorResult ProcessScript(string script);
    }

    public interface IFileParser
    {
        void ParseFile(string path, FileParserContext context);

        void ParseScript(List<string> scriptLines, FileParserContext context);
    }

    public interface ILineProcessor
    {
        bool ProcessLine(IFileParser parser, FileParserContext context, string line, bool isBeforeCode);
    }

    public interface IDirectiveLineProcessor : ILineProcessor
    {
        bool Matches(string line);
    }

    public interface IFileSystem
    {
        //IEnumerable<string> EnumerateFiles(
        //    string dir, string search, SearchOption searchOption = SearchOption.AllDirectories);

        //IEnumerable<string> EnumerateDirectories(
        //    string dir, string searchPattern, SearchOption searchOption = SearchOption.AllDirectories);

        //IEnumerable<string> EnumerateFilesAndDirectories(
        //    string dir, string searchPattern, SearchOption searchOption = SearchOption.AllDirectories);

        void Copy(string source, string dest, bool overwrite);

        void CopyDirectory(string source, string dest, bool overwrite);

        bool DirectoryExists(string path);

        void CreateDirectory(string path, bool hidden = false);

        void DeleteDirectory(string path);

        string ReadFile(string path);

        string[] ReadFileLines(string path);

        DateTime GetLastWriteTime(string file);

        bool IsPathRooted(string path);

        string GetFullPath(string path);

        string CurrentDirectory { get; set; }

        string NewLine { get; }

        string GetWorkingDirectory(string path);

        void Move(string source, string dest);

        void MoveDirectory(string source, string dest);

        bool FileExists(string path);

        void FileDelete(string path);

        IEnumerable<string> SplitLines(string value);

        void WriteToFile(string path, string text);

        //Stream CreateFileStream(string filePath, FileMode mode);

        void WriteAllBytes(string filePath, byte[] bytes);

        //string GlobalFolder { get; }

        //string HostBin { get; }

        string BinFolder { get; }

        string DllCacheFolder { get; }

        string PackagesFile { get; }

        string PackagesFolder { get; }

        string NugetFile { get; }

        //string GlobalOptsFile { get; }
    }
}