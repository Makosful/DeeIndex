using System.Collections.Generic;
using System.IO;

namespace DeepIndex.Hoster.Crawler.Data.Abstractions
{
    public interface IRestAccess
    {
        void SendFilePaths(IEnumerable<FileInfo> fileInfos);
    }
}