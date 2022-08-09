using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace limiredo_backend.Services
{
    public static class SoundService
    {
        public static async Task<byte[]> GetIntervalFile(string url, string sasValue, List<Models.Sound> sounds)
        {
            List<byte> resultList = new List<byte>();

            foreach (var sound in sounds)
            {
                var file = await ReadBlobWithSasAsync(new Uri($"{url}{sound.File}?{sasValue}"));


                MemoryStream ms = new MemoryStream();
                file.Content.CopyTo(ms);
                var soundByteArray = ms.ToArray();
                resultList.AddRange(soundByteArray.Skip(44).ToList());
            }
            var memoryStreamWithHeader = new MemoryStream(new byte[44 + resultList.Count]);
            var memoryStreamWithoutHeader = new MemoryStream(resultList.ToArray());
            memoryStreamWithHeader.Position = 44;
            memoryStreamWithoutHeader.CopyTo(memoryStreamWithHeader);

            return CreateWavHeader(memoryStreamWithHeader);
        }

        private static async Task<BlobDownloadInfo> ReadBlobWithSasAsync(Uri sasUri)
        {
            BlobClient blobClient = new BlobClient(sasUri, null);
            try
            {
                BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
                return blobDownloadInfo;
            }
            catch (RequestFailedException e)
            {
                 throw;
            }
        }

        private static byte[] CreateWavHeader(MemoryStream stream)
        {
            stream.Position = 0;
            stream.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4);
            stream.Position = 4;
            stream.Write(BitConverter.GetBytes(0), 0, 4);
            stream.Position = 8;
            stream.Write(Encoding.ASCII.GetBytes("WAVE"), 0, 4);
            stream.Position = 12;
            stream.Write(Encoding.ASCII.GetBytes("fmt "), 0, 4);
            stream.Position = 16;
            stream.Write(BitConverter.GetBytes(16), 0, 4);
            stream.Position = 20;
            stream.Write(BitConverter.GetBytes(1), 0, 2);
            stream.Position = 22;
            stream.Write(BitConverter.GetBytes(2), 0, 2);
            stream.Position = 24;
            stream.Write(BitConverter.GetBytes(44100), 0, 4);
            stream.Position = 28;
            stream.Write(BitConverter.GetBytes(176400), 0, 4);
            stream.Position = 32;
            stream.Write(BitConverter.GetBytes(4), 0, 2);
            stream.Position = 34;
            stream.Write(BitConverter.GetBytes(16), 0, 2);
            stream.Position = 36;
            stream.Write(Encoding.ASCII.GetBytes("data"), 0, 4);
            stream.Position = 40;
            stream.Write(BitConverter.GetBytes(0), 0, 4);

            stream.Position = 4;
            stream.Write(BitConverter.GetBytes(stream.Length), 0, 4);
            stream.Position = 40;
            stream.Write(BitConverter.GetBytes(stream.Length - 40), 0, 4);
            return stream.ToArray();
        }
    }
}
