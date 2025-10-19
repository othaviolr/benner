namespace SistemaGestao.Models
{
    public class ItemPedido
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public decimal Subtotal
        {
            get
            {
                return Quantidade * ValorUnitario;
            }
        }
    }
}