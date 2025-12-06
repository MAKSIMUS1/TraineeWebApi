using AutoMapper;
using WebApiTrainingProject.Data.Models;
using WebApiTrainingProject.DTOs.Request;
using WebApiTrainingProject.DTOs.Response;

namespace WebApiTrainingProject.Utils.Mapping
{
    public class NodeGraphProfile : Profile
    {
        public NodeGraphProfile()
        {
            CreateMap<NodeGraph, NodeGraphDto>();
            CreateMap<NodeGraphCreateDto, NodeGraph>();
            CreateMap<NodeGraphUpdateDto, NodeGraph>();
        }
    }
}
