// using EcgAi.Api.Data.Database.Cosmos;
// using EcgAi.Api.Features.Images.Queries.GetEcgImageByRecordId;
// using Microsoft.EntityFrameworkCore;
//
// // ReSharper disable ConvertToUsingDeclaration
// // ReSharper disable once UnusedType.Global
// // ReSharper disable PositionalPropertyUsedProblem
//
//
// namespace EcgAi.Api.Features.Images.Queries.GetEcgRecordByRecordId;
//
// public class GetEcgImageByRecordIdHandler : IRequestHandler<GetEcgImageByRecordIdRequest, CreateEcgPlotResponse>
// {
//     private readonly ILogger<GetEcgImageByRecordIdHandler> _logger;
//     // private readonly IMapper _mapper;
//
//     private readonly TrainingDbContext _dbContext;
//
//     private readonly AzureEcgDrawingFunction _azureEcgDrawing;
//     // private readonly IServiceScopeFactory _scopeFactory;
//
//     public GetEcgImageByRecordIdHandler(ILogger<GetEcgImageByRecordIdHandler> logger,
//         TrainingDbContext dbContext,
//         AzureEcgDrawingFunction azureEcgDrawing)
//     {
//         _logger = logger;
//         _dbContext = dbContext;
//         _azureEcgDrawing = azureEcgDrawing;
//     }
//
//     public async Task<CreateEcgPlotResponse> Handle(GetEcgImageByRecordIdRequest request,
//         CancellationToken cancellationToken)
//     {
//         using (var op = Operation.Begin(
//                    "Images/GetEcgImageByRecordId handler was called with a TransactionId {0} with " +
//                    "the RecordId {1} from the Database {2}", request.TransactionId, request.RecordId,
//                    request.DatabaseName!))
//         {
//             var ecgRecord = await GetEcgRecord(request, cancellationToken);
//             var plotRequest = (ecgRecord, request).Adapt<EcgPlotRequest>();
//             var plotResponse = await _azureEcgDrawing.CreateEcgImage(plotRequest);
//             op.Complete();
//             return plotResponse;
//         }
//     }
//
//     private async Task<EcgRecord?> GetEcgRecord(GetEcgImageByRecordIdRequest request,
//         CancellationToken cancellationToken)
//     {
//         using var op = Operation.Begin(
//             "Images/GetEcgImageByRecordId database was called with a TransactionId {0} with " +
//             "the RecordId {1} from the Database {2}", request.TransactionId, request.RecordId,
//             request.DatabaseName!);
//         var ecgRecord = await _dbContext.EcgRecords
//             .Where(c => c.RecordId == request.RecordId && c.DatabaseName == request.DatabaseName)
//             .AsNoTracking()
//             .SingleOrDefaultAsync(cancellationToken: cancellationToken);
//
//         if (ecgRecord is null)
//         {
//             _logger.LogWarning("RecordId {0} from database {1} could not be found", request.RecordId,
//                 request.DatabaseName);
//             throw new ArgumentException(
//                 $"RecordId {request.RecordId} from database {request.DatabaseName} could not be found");
//         }
//
//         op.Complete();
//         return ecgRecord;
//     }
//
//     // private async Task WriteToFile(CreateEcgPlotResponse response)
//     // {
//     //
//     //     var bytes = Convert.FromBase64String(response.Image);
//     //     // var image = Cv2.ImDecode(bytes, ImreadModes.Unchanged);
//     //     // var size = new Size(200, 700);
//     //     Mat img = Mat.FromImageData(bytes, ImreadModes.Unchanged);
//     //     // var imageNew = new Mat();
//     //     // Cv2.Canny(bytes, imageNew, 50, 200);
//     //     // var ddd = new Mat(size,MatType.CV_64F,bytes);
//     //     Cv2.ImWrite("test.png",img);
//     //     var g = "sdsadad";
//     // }
//
//     // private async Task StreamWriteAsync(object obj, string fileName)
//     // {
//     //     JsonSerializerOptions serializerOptions = new()
//     //         {DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull};
//     //     var options = new JsonSerializerOptions(serializerOptions)
//     //     {
//     //         WriteIndented = true
//     //     };
//     //     await using var fileStream = File.Create(fileName);
//     //     await JsonSerializer.SerializeAsync(fileStream, obj, options);
//     // }
// }