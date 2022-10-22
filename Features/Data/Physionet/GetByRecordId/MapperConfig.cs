using EcgAi.Data.Api.Features.Physionet.Grpc;
using DiagnosticCode = EcgAi.Data.Api.Features.Physionet.Grpc.DiagnosticCode;
using EcgLead = EcgAi.Data.Api.Features.Physionet.Grpc.EcgLead;

namespace EcgAi.Api.Features.Data.Physionet.GetByRecordId;

// ReSharper disable once UnusedType.Global
public class MapperConfig:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig.GlobalSettings.Default.EnumMappingStrategy(EnumMappingStrategy.ByValue);
        config.NewConfig<Request, GetByIdRequest>();

        config.NewConfig<GetByIdResponse, Response>()
            .Map(dest => dest.EcgRecord, src => src.Ecg)
            .Map(dest => dest.EcgRecord!.Leads, src => src.Ecg.Leads)
            .Map(dest => dest.EcgRecord!.DiagnosticCodes, src => src.Ecg.DiagnosticCode);
        
            

        config.NewConfig<EcgLead, Core.Models.EcgLead>() 
            .Map(dest=>dest.Signal,src=>src.Signals);

        config.NewConfig<DiagnosticCode, Core.Models.DiagnosticCode>()
            .Map(dest=>dest.Code, src=>src.ScpCode);

    }
}