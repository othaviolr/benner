using System.Collections.Generic;

namespace SistemaGestao.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Endereco { get; set; }

        public List<Pedido> Pedidos { get; set; }

        public Pessoa()
        {
            Pedidos = new List<Pedido>();
        }
    }
}