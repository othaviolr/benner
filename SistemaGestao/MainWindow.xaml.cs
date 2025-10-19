using SistemaGestao.Views;
using System.Windows;

namespace SistemaGestao
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnPessoas_Click(object sender, RoutedEventArgs e)
        {
            var telaPessoas = new PessoasView();
            telaPessoas.ShowDialog();
        }

        private void BtnProdutos_Click(object sender, RoutedEventArgs e)
        {
            var telaProdutos = new ProdutosView();
            telaProdutos.ShowDialog();
        }

        private void BtnPedidos_Click(object sender, RoutedEventArgs e)
        {
            var telaPedidos = new PedidosView();
            telaPedidos.ShowDialog();
        }
    }
}