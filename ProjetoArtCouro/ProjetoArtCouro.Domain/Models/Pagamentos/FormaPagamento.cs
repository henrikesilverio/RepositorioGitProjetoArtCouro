﻿using System;
using System.Collections.Generic;
using ProjetoArtCouro.Domain.Models.Compras;
using ProjetoArtCouro.Domain.Models.Vendas;
using ProjetoArtCouro.Resource.Resources;
using ProjetoArtCouro.Resource.Validation;

namespace ProjetoArtCouro.Domain.Models.Pagamentos
{
    public class FormaPagamento
    {
        public Guid FormaPagamentoId { get; set; }
        public int FormaPagamentoCodigo { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public virtual ICollection<Venda> Venda { get; set; }
        public virtual ICollection<Compra> Compra { get; set; }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotNull(Descricao, string.Format(Erros.NullParameter, "Descricao"));
            AssertionConcern.AssertArgumentNotEmpty(Descricao, string.Format(Erros.EmptyParameter, "Descricao"));
        }
    }
}
