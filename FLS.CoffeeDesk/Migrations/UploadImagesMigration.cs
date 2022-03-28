using System;
using System.IO;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework.Blobs;
using EPiServer.Security;
using FLS.CoffeeDesk.Content;
using J2N.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace FLS.CoffeeDesk.Migrations
{
    internal class UploadImagesMigration
    {
        private readonly IContentRepository _contentRepository;

        private readonly IBlobFactory _blobFactory;

        private readonly IConfiguration _configuration;

        public UploadImagesMigration(IContentRepository contentRepository,
            IBlobFactory blobFactory, IConfiguration configuration)
        {
            _contentRepository = contentRepository;
            _blobFactory = blobFactory;
            _configuration = configuration;
        }

        public ImageFile[] UploadImages(ContentReference assetsRoot)
        {
            var tempDirectory = _configuration["TestImagesFolderPath"];
            if (!Directory.Exists(tempDirectory))
            {
                throw new NotSupportedException("Migration could not be done without test images");
            }
            
            string[] mainFiles = Directory.GetFiles(tempDirectory, "*.jpg")
                .Concat(Directory.GetFiles(tempDirectory, "*.png")).ToArray();
            var images = new List<ImageFile>();
            foreach (var file in mainFiles)
            {
                images.Add(UploadImage(file, assetsRoot));
            }

            return images.ToArray();
        }

        private ImageFile UploadImage(string filePath, ContentReference assetsRoot)
        {
            var imageExtension = Path.GetExtension(filePath);

            var newImage = _contentRepository.GetDefault<ImageFile>(assetsRoot);

            var byteArrayData = File.ReadAllBytes(filePath);

            var blob = _blobFactory.CreateBlob(newImage.BinaryDataContainer, imageExtension);
            using (var s = blob.OpenWrite())
            {
                var w = new StreamWriter(s);
                w.BaseStream.Write(byteArrayData, 0, byteArrayData.Length);
                w.Flush();
            }

            newImage.BinaryData = blob;
            newImage.Name = Path.GetFileName(filePath);
            _contentRepository.Save(newImage, SaveAction.Publish, AccessLevel.NoAccess);
            return newImage;
        }
    }
}