using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGestao.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int PessoaId { get; set; }
        public DateTime DataVenda { get; set; }
        public string FormaPagamento { get; set; }
        public string Status { get; set; } 

        public List<ItemPedido> Itens { get; set; }

        public decimal ValorTotal
        {
            get
            {
                return Itens?.Sum(i => i.Quantidade * i.ValorUnitario) ?? 0;
            }
        }

        public Pedido()
        {
            Itens = new List<ItemPedido>();
            DataVenda = DateTime.Now;
            Status = "Pendente";
        }
    }
}