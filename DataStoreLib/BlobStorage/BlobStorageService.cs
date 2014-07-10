﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using DataStoreLib.Utils;
using System.IO;

namespace DataStoreLib.BlobStorage
{
    public class BlobStorageService
    {
        public const string Blob_ImageContainer = "posters";
        public const string Blob_XMLFileContainer = "crawlfiles";
        public const string Blob_NewsImages = "newsimages";

        #region Private Methods
        private CloudBlobContainer GetCloudBlobContainer(string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("StorageTableConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobCantainer = blobClient.GetContainerReference(containerName);

            if (blobCantainer.CreateIfNotExists())
            {
                blobCantainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            return blobCantainer;
        }

        public List<string> GetUploadedFileFromBlob(string containerName)
        {
            try
            {
                CloudBlobContainer blobContainer = GetCloudBlobContainer(containerName);
                List<string> blobs = new List<string>();

                foreach (var blobItem in blobContainer.ListBlobs())
                {
                    blobs.Add(blobItem.Uri.ToString());
                }

                return blobs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public string UploadImageFileOnBlob(string containerName, string fileName, Stream stream)
        {
            try
            {
                if (stream.Length > 0)
                {
                    stream.Position = 0;
                    CloudBlobContainer blobContainer = GetCloudBlobContainer(containerName);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);
                    blob.UploadFromStream(stream);

                    return GetSinglFile(containerName, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "";
        }

        public int GetImageFileCount(string containerName, string pattern)
        {
            try
            {
                List<string> blobFiles = GetUploadedFileFromBlob(containerName);

                var filtersFiles = blobFiles.FindAll(m => m.Contains(pattern));

                return filtersFiles.Count + 1;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return 0;
            }            
        }

        public string GetSinglFile(string containerName, string fileName)
        {
            try
            {
                List<string> blobFiles = GetUploadedFileFromBlob(containerName);

                var file = blobFiles.Find(m => m.Contains(fileName));

                return file;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UploadXMLFileOnBlob(string containerName, string fileName, string fileText)
        {
            try
            {
                if (fileText.Length > 0)
                {
                    //fileText.Position = 0;
                    CloudBlobContainer blobContainer = GetCloudBlobContainer(containerName);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);
                    blob.UploadText(fileText);

                    return GetSinglFile(containerName, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "";
        }

        public string GetUploadeXMLFileContent(string containerName, string fileName)
        {
            try
            {
                CloudBlobContainer blobContainer = GetCloudBlobContainer(containerName);
                var blob = blobContainer.GetBlockBlobReference(fileName);

                var xmlData = blob.DownloadText();

                return xmlData;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return "";
            }
        }

        public bool DeleteFileFromBlob(string containerName, string fileName)
        {
            try
            {
                CloudBlobContainer blobContainer = GetCloudBlobContainer(containerName);
                var blob = blobContainer.GetBlockBlobReference(fileName);

                if (blob != null)
                    blob.Delete();

                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }
    }
}