using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.IO;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;

namespace AWSSecrets
{
    public class Helpers
    {
    /*
    *	Use this code snippet in your app.
    *	If you need more information about configurations or implementing the sample code, visit the AWS docs:
    *	https://aws.amazon.com/developers/getting-started/net/
    *	
    *	Make sure to include the following packages in your code.
    *	
    *	using System;
    *	using System.IO;
    *
    *	using Amazon;
    *	using Amazon.SecretsManager;
    *	using Amazon.SecretsManager.Model;
    *
    */

    /*
    * AWSSDK.SecretsManager version="3.3.0" targetFramework="net45"
    */
        private static string GetSecret(String secretName)
        {
            Console.WriteLine($"Reaching for secret: {secretName}");
            string region = "us-east-2";
            string secret = "";

            MemoryStream memoryStream = new MemoryStream();

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;
            request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

            GetSecretValueResponse response = null;

            // In this sample we only handle the specific exceptions for the 'GetSecretValue' API.
            // See https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            // We rethrow the exception by default.

            try
            {
                response = client.GetSecretValueAsync(request).Result;
            }
            catch (DecryptionFailureException e)
            {
                // Secrets Manager can't decrypt the protected secret text using the provided KMS key.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (InternalServiceErrorException e)
            {
                // An error occurred on the server side.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (InvalidParameterException e)
            {
                // You provided an invalid value for a parameter.
                // Deal with the exception here, and/or rethrow at your discretion
                throw;
            }
            catch (InvalidRequestException e)
            {
                // You provided a parameter value that is not valid for the current state of the resource.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (ResourceNotFoundException e)
            {
                // We can't find the resource that you asked for.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }
            catch (System.AggregateException ae)
            {
                // More than one of the above exceptions were triggered.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw;
            }

            // Decrypts secret using the associated KMS CMK.
            // Depending on whether the secret is a string or binary, one of these fields will be populated.
            if (response.SecretString != null)
            {
                secret = response.SecretString;
            }
            else
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }

            // Your code goes here.
            return secret;
        } 

        public static string GetConnectionString() {
            String secret = GetSecret("ywa-tracc-db-login");
            var connInfo = JsonConvert.DeserializeObject<dynamic>(secret);
            String host = connInfo.host;
            String db = connInfo.dbInstanceIdentifier;
            String username = connInfo.username;
            String password = connInfo.password;

            return "Host=" + host +
                    ";Database=" + db +
                    ";User ID=" + username +
                    ";Password=" + password + ";";
        }

        public static string GetYTAPIKey() {
            String secret = GetSecret("youtube-api-key");
            var secretObj = JsonConvert.DeserializeObject<dynamic>(secret);
            String apikey = secretObj.YT_API_KEY;
            return apikey;
        }
    }
}