using SistemaGestao.Models;
using SistemaGestao.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SistemaGestao.ViewModels
{
    public class PedidoViewModel : ViewModelBase
    {
        private readonly PessoaService _pessoaService;
        private readonly ProdutoService _produtoService;
        private readonly PedidoService _pedidoService;

        private Pessoa _pessoaSelecionada;
        public Pessoa PessoaSelecionada
        {
            get => _pessoaSelecionada;
            set => SetProperty(ref _pessoaSelecionada, value);
        }

        private Produto _produtoSelecionado;
        public Produto ProdutoSelecionado
        {
            get => _produtoSelecionado;
            set => SetProperty(ref _produtoSelecionado, value);
        }

        private int _quantidade = 1;
        public int Quantidade
        {
            get => _quantidade;
            set => SetProperty(ref _quantidade, value);
        }

        private string _formaPagamento;
        public string FormaPagamento
        {
            get => _formaPagamento;
            set => SetProperty(ref _formaPagamento, value);
        }

        private decimal _valorTotal;
        public decimal ValorTotal
        {
            get => _valorTotal;
            set => SetProperty(ref _valorTotal, value);
        }

        public ObservableCollection<Pessoa> Pessoas { get; set; }
        public ObservableCollection<Produto> Produtos { get; set; }
        public ObservableCollection<ItemPedido> ItensPedido { get; set; }
        public ObservableCollection<string> FormasPagamento { get; set; }

        private bool _pedidoFinalizado;
        public bool PedidoFinalizado
        {
            get => _pedidoFinalizado;
            set => SetProperty(ref _pedidoFinalizado, value);
        }

        public ICommand AdicionarProdutoCommand { get; }
        public ICommand RemoverProdutoCommand { get; }
        public ICommand FinalizarPedidoCommand { get; }
        public ICommand NovoPedidoCommand { get; }

        public PedidoViewModel()
        {
            _pessoaService = new PessoaService();
            _produtoService = new ProdutoService();
            _pedidoService = new PedidoService();

            Pessoas = new ObservableCollection<Pessoa>();
            Produtos = new ObservableCollection<Produto>();
            ItensPedido = new ObservableCollection<ItemPedido>();
            FormasPagamento = new ObservableCollection<string> { "Dinheiro", "Cartão", "Boleto" };

            AdicionarProdutoCommand = new RelayCommand(AdicionarProduto, CanAdicionarProduto);
            RemoverProdutoCommand = new RelayCommand(RemoverProduto);
            FinalizarPedidoCommand = new RelayCommand(FinalizarPedido, CanFinalizarPedido);
            NovoPedidoCommand = new RelayCommand(NovoPedido);

            CarregarPessoas();
            CarregarProdutos();
        }

        public PedidoViewModel(Pessoa pessoa) : this()
        {
            PessoaSelecionada = pessoa;
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

        private void CarregarProdutos()
        {
            Produtos.Clear();
            var produtos = _produtoService.ObterTodos();

            foreach (var produto in produtos)
            {
                Produtos.Add(produto);
            }
        }

        private void AdicionarProduto(object parameter)
        {
            if (ProdutoSelecionado == null || Quantidade <= 0)
                return;

            var itemExistente = ItensPedido.FirstOrDefault(i => i.ProdutoId == ProdutoSelecionado.Id);

            if (itemExistente != null)
            {
                itemExistente.Quantidade += Quantidade;
            }
            else
            {
                var novoItem = new ItemPedido
                {
                    ProdutoId = ProdutoSelecionado.Id,
                    ProdutoNome = ProdutoSelecionado.Nome,
                    Quantidade = Quantidade,
                    ValorUnitario = ProdutoSelecionado.Valor
                };

                ItensPedido.Add(novoItem);
            }

            CalcularValorTotal();

            ProdutoSelecionado = null;
            Quantidade = 1;
        }

        private bool CanAdicionarProduto(object parameter)
        {
            return ProdutoSelecionado != null && Quantidade > 0 && !PedidoFinalizado;
        }

        private void RemoverProduto(object parameter)
        {
            if (parameter is ItemPedido item)
            {
                ItensPedido.Remove(item);
                CalcularValorTotal();
            }
        }

        private void CalcularValorTotal()
        {
            ValorTotal = ItensPedido.Sum(i => i.Subtotal);
        }

        private void FinalizarPedido(object parameter)
        {
            if (PessoaSelecionada == null)
            {
                MessageBox.Show("Selecione uma pessoa para o pedido!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ItensPedido.Any())
            {
                MessageBox.Show("Adicione pelo menos um produto ao pedido!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(FormaPagamento))
            {
                MessageBox.Show("Selecione a forma de pagamento!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var pedido = new Pedido
            {
                PessoaId = PessoaSelecionada.Id,
                FormaPagamento = FormaPagamento,
                Itens = ItensPedido.ToList()
            };

            _pedidoService.Adicionar(pedido);

            PedidoFinalizado = true;

            MessageBox.Show(
                $"Pedido finalizado com sucesso!\n\nValor Total: {ValorTotal:C}\nStatus: Pendente",
                "Sucesso",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private bool CanFinalizarPedido(object parameter)
        {
            return PessoaSelecionada != null &&
                   ItensPedido.Any() &&
                   !string.IsNullOrWhiteSpace(FormaPagamento) &&
                   !PedidoFinalizado;
        }

        private void NovoPedido(object parameter)
        {
            var resultado = MessageBox.Show(
                "Deseja criar um novo pedido? Os dados atuais serão descartados.",
                "Confirmação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultado == MessageBoxResult.Yes)
            {
                LimparPedido();
            }
        }

        private void LimparPedido()
        {
            PessoaSelecionada = null;
            ProdutoSelecionado = null;
            FormaPagamento = null;
            Quantidade = 1;
            ValorTotal = 0;
            PedidoFinalizado = false;
            ItensPedido.Clear();
        }
    }
}