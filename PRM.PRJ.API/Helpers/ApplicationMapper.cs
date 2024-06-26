using PRM.PRJ.API.Models.ViewModel;
using PRM.PRJ.API.Models;
using AutoMapper;

namespace PRM.PRJ.API.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            #region User
            CreateMap<User, UserDTO>().ReverseMap();
            #endregion
        }
    }
}
