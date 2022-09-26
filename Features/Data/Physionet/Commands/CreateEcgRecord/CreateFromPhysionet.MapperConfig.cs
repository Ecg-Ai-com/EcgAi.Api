using GrpcModels = EcgAi.Data.Api.Features.Physionet.Grpc;

namespace EcgAi.Api.Features.Data.Physionet.Commands.CreateEcgRecord;

public class CreateFromPhysionetMapperConfig:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GrpcModels.GetByIdResponse, EcgRecord>()
            .Map(dest => dest, src => src.Ecg)
            .Map(dest => dest.Leads, src => src.Ecg.Leads);
            

        config.NewConfig<GrpcModels.EcgLead, EcgLead>() 
            .Map(dest=>dest.Signal,src=>src.Signals);
    }
}