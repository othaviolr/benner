using SistemaGestao.Models;
using SistemaGestao.ViewModels;
using System.Windows;

namespace SistemaGestao.Views
{
    public partial class PedidosView : Window
    {
        public PedidosView()
        {
            InitializeComponent();
            DataContext = new PedidoViewModel();
        }

        public PedidosView(Pessoa pessoa)
        {
            InitializeComponent();
            DataContext = new PedidoViewModel(pessoa);
        }
    }
}