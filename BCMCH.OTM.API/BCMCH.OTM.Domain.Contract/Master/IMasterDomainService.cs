using BCMCH.OTM.API.Shared.Master;

namespace BCMCH.OTM.Domain.Contract.Master
{
    public interface IMasterDomainService
    {
        Task<IEnumerable<SpecialEquipments>> SpecialEquipmentsDetails();
    }
}
