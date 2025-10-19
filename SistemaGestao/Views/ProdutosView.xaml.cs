using SistemaGestao.ViewModels;
using System.Windows;

namespace SistemaGestao.Views
{
    public partial class ProdutosView : Window
    {
        public ProdutosView()
        {
            InitializeComponent();
            DataContext = new ProdutoViewModel();
        }
    }
}