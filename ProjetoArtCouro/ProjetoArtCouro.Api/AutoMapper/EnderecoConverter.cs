using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Model.Models.Cliente;

namespace ProjetoArtCouro.Api.AutoMapper
{
    public class EnderecoConverter : ITypeConverter<ICollection<Endereco>, EnderecoModel>
    {
        public EnderecoModel Convert(ResolutionContext context)
        {
            var listaEndereco = (ICollection<Endereco>)context.SourceValue;
            return Mapper.Map<EnderecoModel>(listaEndereco.FirstOrDefault(x => x.Principal));
        }
    }
}