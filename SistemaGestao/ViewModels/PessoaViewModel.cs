using SistemaGestao.Helpers;
using SistemaGestao.Models;
using SistemaGestao.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SistemaGestao.ViewModels
{
    public class PessoaViewModel : ViewModelBase
    {
        private readonly PessoaService _pessoaService;
        private readonly PedidoService _pedidoService;

        private Pessoa _pessoaSelecionada;
        public Pessoa PessoaSelecionada
        {
            get => _pessoaSelecionada;
            set
            {
                SetProperty(ref _pessoaSelecionada, value);
                CarregarPedidosDaPessoa();
            }
        }

        private int _pessoaId;
        public int PessoaId
        {
            get => _pessoaId;
            set => SetProperty(ref _pessoaId, value);
        }

        private string _nome;
        public string Nome
        {
            get => _nome;
            set => SetProperty(ref _nome, value);
        }

        private string _cpf;
        public string CPF
        {
            get => _cpf;
            set => SetProperty(ref _cpf, value);
        }

        private string _endereco;
        public string Endereco
        {
            get => _endereco;
            set => SetProperty(ref _endereco, value);
        }

        private string _filtroNome;
        public string FiltroNome
        {
            get => _filtroNome;
            set
            {
                SetProperty(ref _filtroNome, value);
                AplicarFiltros();
            }
        }

        private string _filtroCpf;
        public string FiltroCpf
        {
            get => _filtroCpf;
            set
            {
                SetProperty(ref _filtroCpf, value);
                AplicarFiltros();
            }
        }

        public ObservableCollection<Pessoa> Pessoas { get; set; }
        public ObservableCollection<Pedido> PedidosDaPessoa { get; set; }

        private bool _modoEdicao;
        public bool ModoEdicao
        {
            get => _modoEdicao;
            set => SetProperty(ref _modoEdicao, value);
        }

        public ICommand IncluirCommand { get; }
        public ICommand EditarCommand { get; }
        public ICommand SalvarCommand { get; }
        public ICommand ExcluirCommand { get; }
        public ICommand CancelarCommand { get; }
        public ICommand MarcarComoPagoCommand { get; }
        public ICommand MarcarComoEnviadoCommand { get; }
        public ICommand MarcarComoRecebidoCommand { get; }

        public PessoaViewModel()
        {
            _pessoaService = new PessoaService();
            _pedidoService = new PedidoService();

            Pessoas = new ObservableCollection<Pessoa>();
            PedidosDaPessoa = new ObservableCollection<Pedido>();

            IncluirCommand = new RelayCommand(Incluir);
            EditarCommand = new RelayCommand(Editar, CanEditarOuExcluir);
            SalvarCommand = new RelayCommand(Salvar, CanSalvar);
            ExcluirCommand = new RelayCommand(Excluir, CanEditarOuExcluir);
            CancelarCommand = new RelayCommand(Cancelar);
            MarcarComoPagoCommand = new RelayCommand(MarcarComoPago);
            MarcarComoEnviadoCommand = new RelayCommand(MarcarComoEnviado);
            MarcarComoRecebidoCommand = new RelayCommand(MarcarComoRecebido);

            CarregarPessoas();
        }

        private void CarregarPessoas()
        {
            Pessoas.Clear();
            var pessoas = _pessoaService.ObterTodos();

            foreach (var pessoa in pessoas)
            {
                Pessoas.Add(pessoa);
            }
        }

        private void AplicarFiltros()
        {
            Pessoas.Clear();
            var pessoas = _pessoaService.ObterTodos();

            if (!string.IsNullOrWhiteSpace(FiltroNome))
            {
                pessoas = pessoas.Where(p => p.Nome.ToLower().Contains(FiltroNome.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(FiltroCpf))
            {
                var cpfLimpo = CpfValidator.RemoverFormatacao(FiltroCpf);
                pessoas = pessoas.Where(p => p.CPF.Contains(cpfLimpo)).ToList();
            }

            foreach (var pessoa in pessoas)
            {
                Pessoas.Add(pessoa);
            }
        }

        private void Incluir(object parameter)
        {
            LimparCampos();
            ModoEdicao = true;
        }

        private void Editar(object parameter)
        {
            if (PessoaSelecionada == null) return;

            PessoaId = PessoaSelecionada.Id;
            Nome = PessoaSelecionada.Nome;
            CPF = PessoaSelecionada.CPF;
            Endereco = PessoaSelecionada.Endereco;
            ModoEdicao = true;
        }

        private bool CanEditarOuExcluir(object parameter)
        {
            return PessoaSelecionada != null;
        }

        private void Salvar(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Nome))
            {
                MessageBox.Show("O campo Nome é obrigatório!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CPF))
            {
                MessageBox.Show("O campo CPF é obrigatório!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!CpfValidator.Validar(CPF))
            {
                MessageBox.Show("CPF inválido!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var cpfLimpo = CpfValidator.RemoverFormatacao(CPF);
            if (_pessoaService.CpfJaExiste(cpfLimpo, PessoaId == 0 ? (int?)null : PessoaId))
            {
                MessageBox.Show("Este CPF já está cadastrado!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var pessoa = new Pessoa
            {
                Id = PessoaId,
                Nome = Nome,
                CPF = cpfLimpo,
                Endereco = Endereco
            };

            if (PessoaId == 0)
            {
                _pessoaService.Adicionar(pessoa);
                MessageBox.Show("Pessoa cadastrada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _pessoaService.Atualizar(pessoa);
                MessageBox.Show("Pessoa atualizada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ModoEdicao = false;
            LimparCampos();
            CarregarPessoas();
        }

        private bool CanSalvar(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Nome) && !string.IsNullOrWhiteSpace(CPF);
        }

        private void Excluir(object parameter)
        {
            if (PessoaSelecionada == null) return;

            var resultado = MessageBox.Show(
                $"Deseja realmente excluir a pessoa '{PessoaSelecionada.Nome}'?",
                "Confirmação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                _pessoaService.Remover(PessoaSelecionada.Id);
                MessageBox.Show("Pessoa excluída com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                CarregarPessoas();
                LimparCampos();
            }
        }

        private void Cancelar(object parameter)
        {
            ModoEdicao = false;
            LimparCampos();
        }

        private void LimparCampos()
        {
            PessoaId = 0;
            Nome = string.Empty;
            CPF = string.Empty;
            Endereco = string.Empty;
            PessoaSelecionada = null;
        }

        private void CarregarPedidosDaPessoa()
        {
            PedidosDaPessoa.Clear();

            if (PessoaSelecionada != null)
            {
                var pedidos = _pedidoService.ObterPorPessoa(PessoaSelecionada.Id);

                foreach (var pedido in pedidos)
                {
                    PedidosDaPessoa.Add(pedido);
                }
            }
        }

        private void MarcarComoPago(object parameter)
        {
            if (parameter is Pedido pedido)
            {
                _pedidoService.AtualizarStatus(pedido.Id, "Pago");
                CarregarPedidosDaPessoa();
                MessageBox.Show("Pedido marcado como Pago!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MarcarComoEnviado(object parameter)
        {
            if (parameter is Pedido pedido)
            {
                _pedidoService.AtualizarStatus(pedido.Id, "Enviado");
                CarregarPedidosDaPessoa();
                MessageBox.Show("Pedido marcado como Enviado!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MarcarComoRecebido(object parameter)
        {
            if (parameter is Pedido pedido)
            {
                _pedidoService.AtualizarStatus(pedido.Id, "Recebido");
                CarregarPedidosDaPessoa();
                MessageBox.Show("Pedido marcado como Recebido!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void FiltrarTodosPedidos()
        {
            if (PessoaSelecionada == null)
            {
                MessageBox.Show("Selecione uma pessoa primeiro!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CarregarPedidosDaPessoa();
        }

        public void FiltrarPedidosEntregues()
        {
            if (PessoaSelecionada == null)
            {
                MessageBox.Show("Selecione uma pessoa primeiro!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PedidosDaPessoa.Clear();
            var pedidos = _pedidoService.ObterEntregues(PessoaSelecionada.Id);

            foreach (var pedido in pedidos)
            {
                PedidosDaPessoa.Add(pedido);
            }
        }

        public void FiltrarPedidosPagos()
        {
            if (PessoaSelecionada == null)
            {
                MessageBox.Show("Selecione uma pessoa primeiro!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PedidosDaPessoa.Clear();
            var pedidos = _pedidoService.ObterPagos(PessoaSelecionada.Id);

            foreach (var pedido in pedidos)
            {
                PedidosDaPessoa.Add(pedido);
            }
        }

        public void FiltrarPedidosPendentes()
        {
            if (PessoaSelecionada == null)
            {
                MessageBox.Show("Selecione uma pessoa primeiro!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PedidosDaPessoa.Clear();
            var pedidos = _pedidoService.ObterPendentesPagamento(PessoaSelecionada.Id);

            foreach (var pedido in pedidos)
            {
                PedidosDaPessoa.Add(pedido);
            }
        }
    }
}