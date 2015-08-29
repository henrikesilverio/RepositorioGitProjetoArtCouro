using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ProjetoArtCouro.Domain.Models.Enums;
using ProjetoArtCouro.Domain.Models.Pessoas;
using ProjetoArtCouro.Model.Models.Common;

namespace ProjetoArtCouro.Api.AutoMapper
{
    public class MeioComunicacaoConverter : ITypeConverter<ICollection<MeioComunicacao>, MeioComunicacaoModel>
    {
        public MeioComunicacaoModel Convert(ResolutionContext context)
        {
            var listaMeioComunicacao = (ICollection<MeioComunicacao>)context.SourceValue;
            var meioComunicacaoModel = new MeioComunicacaoModel();
            foreach (var item in listaMeioComunicacao.Where(x => x.Principal).ToList())
            {
                if (item.TipoComunicacao.Equals(TipoComunicacaoEnum.Telefone))
                {
                    meioComunicacaoModel.TelefoneId = item.MeioComunicacaoCodigo;
                    meioComunicacaoModel.Telefone = item.MeioComunicacaoNome;
                }
                else if (item.TipoComunicacao.Equals(TipoComunicacaoEnum.Celular))
                {
                    meioComunicacaoModel.CelularId = item.MeioComunicacaoCodigo;
                    meioComunicacaoModel.Celular = item.MeioComunicacaoNome;
                }
                else if (item.TipoComunicacao.Equals(TipoComunicacaoEnum.Email))
                {
                    meioComunicacaoModel.EmailId = item.MeioComunicacaoCodigo;
                    meioComunicacaoModel.Email = item.MeioComunicacaoNome;
                }
            }
            return meioComunicacaoModel;
        }
    }
}