using SistemaGestao.Models;
using SistemaGestao.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SistemaGestao.ViewModels
{
    public class ProdutoViewModel : ViewModelBase
    {
        private readonly ProdutoService _produtoService;

        private Produto _produtoSelecionado;
        public Produto ProdutoSelecionado
        {
            get => _produtoSelecionado;
            set => SetProperty(ref _produtoSelecionado, value);
        }

        private int _produtoId;
        public int ProdutoId
        {
            get => _produtoId;
            set => SetProperty(ref _produtoId, value);
        }

        private string _nome;
        public string Nome
        {
            get => _nome;
            set => SetProperty(ref _nome, value);
        }

        private string _codigo;
        public string Codigo
        {
            get => _codigo;
            set => SetProperty(ref _codigo, value);
        }

        private decimal _valor;
        public decimal Valor
        {
            get => _valor;
            set => SetProperty(ref _valor, value);
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

        private string _filtroCodigo;
        public string FiltroCodigo
        {
            get => _filtroCodigo;
            set
            {
                SetProperty(ref _filtroCodigo, value);
                AplicarFiltros();
            }
        }

        private string _filtroValorMinimo;
        public string FiltroValorMinimo
        {
            get => _filtroValorMinimo;
            set
            {
                SetProperty(ref _filtroValorMinimo, value);
                AplicarFiltros();
            }
        }

        private string _filtroValorMaximo;
        public string FiltroValorMaximo
        {
            get => _filtroValorMaximo;
            set
            {
                SetProperty(ref _filtroValorMaximo, value);
                AplicarFiltros();
            }
        }

        public ObservableCollection<Produto> Produtos { get; set; }

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

        public ProdutoViewModel()
        {
            _produtoService = new ProdutoService();
            Produtos = new ObservableCollection<Produto>();

            IncluirCommand = new RelayCommand(Incluir);
            EditarCommand = new RelayCommand(Editar, CanEditarOuExcluir);
            SalvarCommand = new RelayCommand(Salvar, CanSalvar);
            ExcluirCommand = new RelayCommand(Excluir, CanEditarOuExcluir);
            CancelarCommand = new RelayCommand(Cancelar);

            CarregarProdutos();
        }

        private void CarregarProdutos()
        {
            Produtos.Clear();
            var produtos = _produtoService.ObterTodos();

            foreach (var produto in produtos)
            {
                Produtos.Add(produto);
            }
        }

        private void AplicarFiltros()
        {
            Produtos.Clear();
            var produtos = _produtoService.ObterTodos();

            if (!string.IsNullOrWhiteSpace(FiltroNome))
            {
                produtos = produtos.Where(p => p.Nome.ToLower().Contains(FiltroNome.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(FiltroCodigo))
            {
                produtos = produtos.Where(p => p.Codigo.ToLower().Contains(FiltroCodigo.ToLower())).ToList();
            }

            decimal? valorMin = null;
            decimal? valorMax = null;

            if (decimal.TryParse(FiltroValorMinimo, out decimal min))
                valorMin = min;

            if (decimal.TryParse(FiltroValorMaximo, out decimal max))
                valorMax = max;

            if (valorMin.HasValue)
            {
                produtos = produtos.Where(p => p.Valor >= valorMin.Value).ToList();
            }

            if (valorMax.HasValue)
            {
                produtos = produtos.Where(p => p.Valor <= valorMax.Value).ToList();
            }

            foreach (var produto in produtos)
            {
                Produtos.Add(produto);
            }
        }

        private void Incluir(object parameter)
        {
            LimparCampos();
            ModoEdicao = true;
        }

        private void Editar(object parameter)
        {
            if (ProdutoSelecionado == null) return;

            ProdutoId = ProdutoSelecionado.Id;
            Nome = ProdutoSelecionado.Nome;
            Codigo = ProdutoSelecionado.Codigo;
            Valor = ProdutoSelecionado.Valor;
            ModoEdicao = true;
        }

        private bool CanEditarOuExcluir(object parameter)
        {
            return ProdutoSelecionado != null;
        }

        private void Salvar(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Nome))
            {
                MessageBox.Show("O campo Nome é obrigatório!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Codigo))
            {
                MessageBox.Show("O campo Código é obrigatório!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Valor <= 0)
            {
                MessageBox.Show("O campo Valor deve ser maior que zero!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_produtoService.CodigoJaExiste(Codigo, ProdutoId == 0 ? (int?)null : ProdutoId))
            {
                MessageBox.Show("Este código já está cadastrado!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var produto = new Produto
            {
                Id = ProdutoId,
                Nome = Nome,
                Codigo = Codigo,
                Valor = Valor
            };

            if (ProdutoId == 0)
            {
                _produtoService.Adicionar(produto);
                MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _produtoService.Atualizar(produto);
                MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ModoEdicao = false;
            LimparCampos();
            CarregarProdutos();
        }

        private bool CanSalvar(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Nome) &&
                   !string.IsNullOrWhiteSpace(Codigo) &&
                   Valor > 0;
        }

        private void Excluir(object parameter)
        {
            if (ProdutoSelecionado == null) return;

            var resultado = MessageBox.Show(
                $"Deseja realmente excluir o produto '{ProdutoSelecionado.Nome}'?",
                "Confirmação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                _produtoService.Remover(ProdutoSelecionado.Id);
                MessageBox.Show("Produto excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                CarregarProdutos();
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
            ProdutoId = 0;
            Nome = string.Empty;
            Codigo = string.Empty;
            Valor = 0;
            ProdutoSelecionado = null;
        }
    }
}