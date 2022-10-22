// // ReSharper disable UnusedAutoPropertyAccessor.Global
//
// // ReSharper disable ClassNeverInstantiated.Global
// // ReSharper disable UnusedMember.Global
//
// using EcgAi.Api.Features.Images.Queries.GetEcgImageByRecordId;
//
// namespace EcgAi.Api.Features.Images.Queries.GetEcgRecordByRecordId;
//
// public class GetEcgRecordByRecordIdRequest :IRequest<CreateEcgPlotResponse>
// {
//     public string TransactionId { get; init; } = Guid.NewGuid().ToString();
//     public int RecordId { get; init; }
//     public string? DatabaseName { get; init; }
//     public int SampleRate { get; init; }
//
// }
//
//
// public class GetEcgRecordByRecordIdResponse
// {
//     public string? TransactionId { get; set; }
//     public string? RecordName { get; set; }
//     public string? FileName { get; set; }
//     public string? FileExtension { get; set; }
//     public string? Image { get; set; }
// }
//
