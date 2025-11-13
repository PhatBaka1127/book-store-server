using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookStore.Business.Helper;
using BookStore.Business.Service.Interface;
using BookStore.Data.Helper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BookStore.Business.Service.Implement
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<string> DeleteImageAsync(string url)
        {
            string publicId = ExtractPublicId(url);
            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "BookStore",
                Transformation = new Transformation().Width(800).Height(800).Crop("limit")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                return uploadResult.SecureUrl.ToString();

            throw new FileException("Failed uploading file");
        }

        private string ExtractPublicId(string imageUrl)
        {
            // https://res.cloudinary.com/dx6jgfkio/image/upload/v1763002036/BookStore/va5rt2tttrhekmdslqqb.jpg
            // https://res.cloudinary.com/<cloud_name>/image/upload/v<version>/<folder>/<public_id>.<ext>
            var match = Regex.Match(imageUrl, @"upload\/v\d+\/(.+?)\.[a-zA-Z0-9]+$");
            if (match.Success)
            {
                return match.Groups[1].Value; // "BookStore/va5rt2tttrhekmdslqqb"
            }

            throw new ArgumentException("Invalid Cloudinary image URL format.", nameof(imageUrl));
        }
    }
}