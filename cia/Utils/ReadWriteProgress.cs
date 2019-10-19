using System;
using System.Collections.Generic;
using System.Text;

namespace cia.Utils
{

    public class ReadWriteProgress
    {
        public ReadWriteProgress(long bytesRead, long totalBytes)
        {
            BytesRead = bytesRead;
            TotalBytes = totalBytes;
        }

        public long TotalBytes { get; private set; }

        public long BytesRead { get; private set; }

        public float PercentComplete { get { return (float)BytesRead / TotalBytes; } }

        public bool IsFinished { get { return BytesRead == TotalBytes; } }
    }
}
