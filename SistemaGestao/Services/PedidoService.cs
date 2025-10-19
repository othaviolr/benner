using SistemaGestao.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGestao.Services
{
    public class PedidoService : BaseService<Pedido>
    {
        public PedidoService() : base("pedidos.json")
        {
        }

        private int GerarProximoId()
        {
            return Dados.Any() ? Dados.Max(p => p.Id) + 1 : 1;
        }

        public void Adicionar(Pedido pedido)
        {
            pedido.Id = GerarProximoId();
            pedido.DataVenda = DateTime.Now;
            pedido.Status = "Pendente";
            Dados.Add(pedido);
            SalvarDados();
        }

        public void AtualizarStatus(int id, string novoStatus)
        {
            var pedido = Dados.FirstOrDefault(p => p.Id == id);

            if (pedido != null)
            {
                pedido.Status = novoStatus;
                SalvarDados();
            }
        }

        public Pedido ObterPorId(int id)
        {
            return Dados.FirstOrDefault(p => p.Id == id);
        }

        public List<Pedido> ObterPorPessoa(int pessoaId)
        {
            return Dados.Where(p => p.PessoaId == pessoaId)
                       .OrderByDescending(p => p.DataVenda) 
                       .ToList();
        }

        public List<Pedido> ObterPorStatus(int pessoaId, string status)
        {
            return Dados.Where(p => p.PessoaId == pessoaId && p.Status == status)
                       .OrderByDescending(p => p.DataVenda)
                       .ToList();
        }

        public List<Pedido> ObterEntregues(int pessoaId)
        {
            return Dados.Where(p => p.PessoaId == pessoaId && p.Status == "Recebido")
                       .OrderByDescending(p => p.DataVenda)
                       .ToList();
        }

        public List<Pedido> ObterPagos(int pessoaId)
        {
            return Dados.Where(p => p.PessoaId == pessoaId &&
                              (p.Status == "Pago" || p.Status == "Enviado" || p.Status == "Recebido"))
                       .OrderByDescending(p => p.DataVenda)
                       .ToList();
        }

        public List<Pedido> ObterPendentesPagamento(int pessoaId)
        {
            return Dados.Where(p => p.PessoaId == pessoaId && p.Status == "Pendente")
                       .OrderByDescending(p => p.DataVenda)
                       .ToList();
        }

        public decimal CalcularTotalPorPessoa(int pessoaId)
        {
            return Dados.Where(p => p.PessoaId == pessoaId)
                       .Sum(p => p.ValorTotal);
        }
    }
}