using GrpcModels = EcgAi.Data.Api.Features.Physionet.Grpc;
using DiagnosticCode = EcgAi.Data.Api.Features.Physionet.Grpc.DiagnosticCode;
using EcgLead = EcgAi.Data.Api.Features.Physionet.Grpc.EcgLead;

namespace EcgAi.Api.Features.Data.Physionet.Commands.CreateEcgRecord;

// ReSharper disable once UnusedType.Global
public class CreateFromPhysionetMapperConfig:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GrpcModels.GetByIdResponse, EcgRecord>()
            .Map(dest => dest, src => src.Ecg)
            .Map(dest => dest.Leads, src => src.Ecg.Leads)
            .Map(dest => dest.DiagnosticCodes, src => src.Ecg.DiagnosticCode);
            

        config.NewConfig<GrpcModels.EcgLead, EcgLead>() 
            .Map(dest=>dest.Signals,src=>src.Signals);
        
        config.NewConfig<DiagnosticCode, Core.Models.DiagnosticCode>()
            .Map(dest=>dest.Code, src=>src.ScpCode);
    }
}