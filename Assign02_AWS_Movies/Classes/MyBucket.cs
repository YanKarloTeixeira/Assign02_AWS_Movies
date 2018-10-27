using System.Composition;
using Amazon.S3.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Amazon.S3;

namespace Assign02_AWS_Movies.Classes
{
    public class MyBucket
    {
        public static async Task FileDeleteAsync( string KeyName, string BucketName, IAmazonS3 client)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = BucketName,
                    Key = KeyName
                };

                Console.WriteLine("Deleting an object");
                await client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}
