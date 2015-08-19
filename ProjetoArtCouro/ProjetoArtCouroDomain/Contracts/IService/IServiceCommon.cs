using ProjetoArtCouroDomain.GeneralUse;
using ProjetoArtCouroDomain.Registers;
using System.Collections.Generic;

namespace ProjetoArtCouroDataBase.IService
{
    public interface IServiceCommon
    {
        List<MaritalStatus> GetListMaritalStatus();
        List<States> GetListStates();
        void CreatePerson(PhysicalPerson model);
        void CreatePerson(LegalPerson model);
    }
}
