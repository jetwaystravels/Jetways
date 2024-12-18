using DomainLayer.Model;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

namespace OnionConsumeWebAPI.Comman
{
    public class CredentialService
    {
        public async Task PopulateCredentialsAsync(HttpResponseMessage response, _credentials credentialsObj, int flightCode)
        {
            if (response == null) throw new ArgumentNullException(nameof(response), "Response cannot be null.");
            if (credentialsObj == null) throw new ArgumentNullException(nameof(credentialsObj), "Credentials object cannot be null.");

            var results = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(results))
                throw new InvalidOperationException("Response content is empty.");

            var jsonObject = JsonConvert.DeserializeObject<List<_credentials>>(results);

            if (jsonObject == null || jsonObject.Count == 0)
                throw new InvalidOperationException("Deserialized object is null or empty.");

            // Find the matching credentials based on flightCode
            var matchingCredential = jsonObject.FirstOrDefault(cred => cred?.FlightCode == flightCode);

            if (matchingCredential != null)
            {
                credentialsObj.username = matchingCredential.username ?? string.Empty;
                credentialsObj.password = matchingCredential.password ?? string.Empty;
                credentialsObj.domain = matchingCredential.domain ?? string.Empty;
                credentialsObj.Image = matchingCredential.Image ?? string.Empty;
            }
            else
            {
                throw new KeyNotFoundException($"No credentials found for FlightCode: {flightCode}.");
            }
        }


        //public async Task<Sessionmanager.LogonRequest> PopulateLogonRequestAsync(HttpResponseMessage response, int flightCode)
        //{
        //    if (response == null)
        //        throw new ArgumentNullException(nameof(response), "Response cannot be null.");

        //    if (!response.IsSuccessStatusCode)
        //        throw new InvalidOperationException("Response indicates a failure.");

        //    var results = await response.Content.ReadAsStringAsync();

        //    if (string.IsNullOrWhiteSpace(results))
        //        throw new InvalidOperationException("Response content is empty.");

        //    var JsonObject = JsonConvert.DeserializeObject<List<_credentials>>(results);
        //    //FOR SPICEJET

        //    if (JsonObject == null || JsonObject.Count == 0)
        //        throw new InvalidOperationException("Deserialized object is null or empty.");

        //    // Find the matching credentials based on flightCode
        //    var matchingCredential = JsonObject.FirstOrDefault(cred => cred?.FlightCode == flightCode);

        //    if (matchingCredential != null)
        //    {
        //        var logonRequestData = new Sessionmanager.LogonRequestData
        //        {


        //            AgentName = matchingCredential.username ?? string.Empty,
        //            Password = matchingCredential.password ?? string.Empty,
        //            DomainCode = matchingCredential.domain ?? string.Empty


        //        };

        //        var logonRequest = new Sessionmanager.LogonRequest
        //        {
        //            ContractVersion = 420,
        //            logonRequestData = logonRequestData
        //        };

        //        return logonRequest;
        //    }

        //    throw new InvalidOperationException("Invalid or insufficient credentials data.");
        //}
    }

}
