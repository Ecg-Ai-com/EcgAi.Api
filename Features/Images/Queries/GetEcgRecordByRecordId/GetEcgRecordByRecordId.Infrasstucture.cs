// using System.Net.Mime;
// using System.Text;
// using System.Text.Json;
// using EcgAi.Api.Features.Images.Queries.GetEcgImageByRecordId;
//
// // ReSharper disable PositionalPropertyUsedProblem
//
// namespace EcgAi.Api.Features.Images.Queries.GetEcgRecordByRecordId;
//
// /// <summary>
// ///     To call external Azure http function creating an ecg image from ecg data
// /// </summary>
// public class AzureEcgDrawingFunction
// {
//     private readonly HttpClient _httpClient;
//     private readonly ILogger<AzureEcgDrawingFunction> _logger;
//
//
//     public AzureEcgDrawingFunction(ILogger<AzureEcgDrawingFunction> logger, HttpClient httpClient)
//     {
//         _httpClient = httpClient;
//         _logger = logger;
//     }
//
//     /// <summary>
//     /// </summary>
//     /// <param name="ecgPlotRequest"></param>
//     /// <returns></returns>
//     /// <exception cref="HttpRequestException"></exception>
//     /// <exception cref="ArgumentNullException"></exception>
//     public async Task<CreateEcgPlotResponse> CreateEcgImage(EcgPlotRequest ecgPlotRequest)
//     {
//         using var op = Operation.Begin(
//             "Images/GetEcgImageByRecordId azure http function was called with a TransactionId {0} with " +
//             "the RecordId {1}", ecgPlotRequest.TransactionId!, ecgPlotRequest.RecordName!);
//         var request = CreateHttpRequestMessage(ecgPlotRequest);
//         var response = await _httpClient.SendAsync(request);
//
//         if (!response.IsSuccessStatusCode)
//             throw new HttpRequestException("CreateEcgImage has returned a bad status code {response.StatusCode}");
//
//         var ecgPlotResponse = await response.Content.ReadFromJsonAsync<CreateEcgPlotResponse>();
//         if (ecgPlotResponse != null)
//         {
//             op.Complete();
//             return ecgPlotResponse;
//         }
//
//         _logger.LogWarning("CreateEcgImage has not returned a valid json document {0}",
//             response.Content.ReadAsStringAsync());
//         throw new ArgumentException(
//             $@"CreateEcgImage has not returned a valid json document: {response.Content.ReadAsStringAsync()}");
//     }
//
//     private HttpRequestMessage CreateHttpRequestMessage(EcgPlotRequest ecgPlotRequest)
//     {
//         var json = JsonSerializer.Serialize(ecgPlotRequest);
//         var request = new HttpRequestMessage
//         {
//             Method = HttpMethod.Get,
//             RequestUri = _httpClient.BaseAddress,
//             Content = new StringContent(json, Encoding.UTF8,
//                 MediaTypeNames.Application.Json /* or "application/json" in older versions */)
//         };
//         return request;
//     }
// }