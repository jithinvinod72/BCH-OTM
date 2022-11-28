using BCMCH.OTM.API.Shared.Master;

namespace BCMCH.OTM.Data.Contract.Master
{
    public interface IMasterDataAccess
    {
        Task<IEnumerable<SpecialEquipments>> SpecialEquipmentsDetails();
    }
}
