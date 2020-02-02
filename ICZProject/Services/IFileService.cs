using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICZProject.Services
{
    public interface IFileService
    {
        string Read(string filePath);

        T Deserialize<T>(string input) where T : class;

        string Serialize<T>(T objectToSerialize);
    }
}