using AutoMapper;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Application.Projects.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Projects;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Profiles
{
    internal class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<LanguageCode, string>().ConstructUsing(x => x.Value);
            CreateMap<ProjectId, string>().ConstructUsing(src => src.Value);
            CreateMap<ProjectInfo, ProjectInfoView>();
            CreateMap<Project, ProjectView>();
        }
    }
}
