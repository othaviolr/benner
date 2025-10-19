using SistemaGestao.Models;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGestao.Services
{
    public class ProdutoService : BaseService<Produto>
    {
        public ProdutoService() : base("produtos.json")
        {
        }

        private int GerarProximoId()
        {
            return Dados.Any() ? Dados.Max(p => p.Id) + 1 : 1;
        }

        public void Adicionar(Produto produto)
        {
            produto.Id = GerarProximoId();
            Dados.Add(produto);
            SalvarDados();
        }

        public void Atualizar(Produto produto)
        {
            var produtoExistente = Dados.FirstOrDefault(p => p.Id == produto.Id);

            if (produtoExistente != null)
            {
                produtoExistente.Nome = produto.Nome;
                produtoExistente.Codigo = produto.Codigo;
                produtoExistente.Valor = produto.Valor;
                SalvarDados();
            }
        }

        public void Remover(int id)
        {
            var produto = Dados.FirstOrDefault(p => p.Id == id);

            if (produto != null)
            {
                Dados.Remove(produto);
                SalvarDados();
            }
        }

        public Produto ObterPorId(int id)
        {
            return Dados.FirstOrDefault(p => p.Id == id);
        }

        public List<Produto> BuscarPorNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return ObterTodos();

            return Dados.Where(p => p.Nome.ToLower().Contains(nome.ToLower())).ToList();
        }

        public List<Produto> BuscarPorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return ObterTodos();

            return Dados.Where(p => p.Codigo.ToLower().Contains(codigo.ToLower())).ToList();
        }

        public List<Produto> BuscarPorFaixaValor(decimal? valorMinimo, decimal? valorMaximo)
        {
            var query = Dados.AsQueryable();

            if (valorMinimo.HasValue)
            {
                query = query.Where(p => p.Valor >= valorMinimo.Value);
            }

            if (valorMaximo.HasValue)
            {
                query = query.Where(p => p.Valor <= valorMaximo.Value);
            }

            return query.ToList();
        }

        public bool CodigoJaExiste(string codigo, int? idExcluir = null)
        {
            if (idExcluir.HasValue)
            {
                return Dados.Any(p => p.Codigo == codigo && p.Id != idExcluir.Value); 
            }

            return Dados.Any(p => p.Codigo == codigo); 
        }
    }
}