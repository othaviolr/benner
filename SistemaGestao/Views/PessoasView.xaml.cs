using SistemaGestao.ViewModels;
using System.Windows;

namespace SistemaGestao.Views
{
    public partial class PessoasView : Window
    {
        private PessoaViewModel _viewModel;

        public PessoasView()
        {
            InitializeComponent();
            _viewModel = new PessoaViewModel();
            DataContext = _viewModel;
        }

        private void BtnIncluirPedido_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.PessoaSelecionada == null)
            {
                MessageBox.Show("Selecione uma pessoa primeiro!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var telaPedido = new PedidosView(_viewModel.PessoaSelecionada);
            telaPedido.ShowDialog();

            _viewModel.PessoaSelecionada = _viewModel.PessoaSelecionada;
        }

        private void BtnTodosPedidos_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.FiltrarTodosPedidos();
        }

        private void BtnPedidosEntregues_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.FiltrarPedidosEntregues();
        }

        private void BtnPedidosPagos_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.FiltrarPedidosPagos();
        }

        private void BtnPedidosPendentes_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.FiltrarPedidosPendentes();
        }
    }
}