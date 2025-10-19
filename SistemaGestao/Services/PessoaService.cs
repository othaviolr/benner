using SistemaGestao.Models;
using System.Collections.Generic;
using System.Linq;

namespace SistemaGestao.Services
{
    public class PessoaService : BaseService<Pessoa>
    {
        public PessoaService() : base("pessoas.json")
        {
        }

        private int GerarProximoId()
        {
            return Dados.Any() ? Dados.Max(p => p.Id) + 1 : 1;
        }

        public void Adicionar(Pessoa pessoa)
        {
            pessoa.Id = GerarProximoId();
            Dados.Add(pessoa);
            SalvarDados();
        }

        public void Atualizar(Pessoa pessoa)
        {
            var pessoaExistente = Dados.FirstOrDefault(p => p.Id == pessoa.Id);

            if (pessoaExistente != null)
            {
                pessoaExistente.Nome = pessoa.Nome;
                pessoaExistente.CPF = pessoa.CPF;
                pessoaExistente.Endereco = pessoa.Endereco;
                SalvarDados();
            }
        }

        public void Remover(int id)
        {
            var pessoa = Dados.FirstOrDefault(p => p.Id == id);

            if (pessoa != null)
            {
                Dados.Remove(pessoa);
                SalvarDados();
            }
        }

        public Pessoa ObterPorId(int id)
        {
            return Dados.FirstOrDefault(p => p.Id == id);
        }

        public List<Pessoa> BuscarPorNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return ObterTodos();

            return Dados.Where(p => p.Nome.ToLower().Contains(nome.ToLower())).ToList();
        }

        public List<Pessoa> BuscarPorCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return ObterTodos();

            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            return Dados.Where(p => p.CPF.Contains(cpf)).ToList();
        }

        public bool CpfJaExiste(string cpf, int? idExcluir = null)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (idExcluir.HasValue)
            {
                return Dados.Any(p => p.CPF == cpf && p.Id != idExcluir.Value);
            }

            return Dados.Any(p => p.CPF == cpf);
        }
    }
}