namespace EcgAi.Api.Features.Images.Queries.GetEcgImageByRecordId;

// ReSharper disable once UnusedType.Global
public class GetEcgImageByRecordIdMapperConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(EcgRecord ecgRecord, GetEcgImageByRecordIdRequest request), EcgPlotRequest>()
            .Map(dest => dest.TransactionId, src => src.request.TransactionId)
            .Map(dest => dest.RecordName, src => src.ecgRecord.RecordName)
            .Map(dest => dest.SampleRate, src => src.ecgRecord.SampleRate)
            .Map(dest => dest.ColorStyle, src => src.request.ColorStyle)
            .Map(dest => dest.ShowGrid, src => src.request.ShowGrid)
            .Map(dest => dest.Artifact, src => src.request.Artifact)
            .Map(dest => dest.EcgLeads, src => src.ecgRecord.Leads)
            .Map(dest => dest.FileName, src => src.request.FileName);
    }
}