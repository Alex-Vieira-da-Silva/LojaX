using LojaX.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LojaX.API.Controllers
{
    [Route("api/lojax")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private List<Produto> produtos;
        private List<Pedido> _pedidos;

        public PedidosController()
        {
            produtos = new List<Produto>
            {
                new Produto { Id = 1, Nome = "Teclado", Preco = 150.00m },
                new Produto { Id = 2, Nome = "Mouse", Preco = 60.00m },
                new Produto { Id = 3, Nome = "Monitor", Preco = 1000.00m },
                new Produto { Id = 4, Nome = "Desktop", Preco = 1500.00m },
                new Produto { Id = 5, Nome = "Notebook", Preco = 3000.00m },
                new Produto { Id = 6, Nome = "Pendrive", Preco = 30.00m },
                new Produto { Id = 7, Nome = "Fonte", Preco = 70.00m },
                new Produto { Id = 8, Nome = "Cabo", Preco = 10.00m },
                new Produto { Id = 9, Nome = "Hd", Preco = 80.90m },
                new Produto { Id = 10, Nome = "Ssd", Preco = 120.00m },
            };

            // Criea alguns pedidos com entre 3 e 7 produtos aleatórios
            var random = new Random();
            _pedidos = new List<Pedido>();

            for (int i = 1; i <= 3; i++)
            {
                var pedido = new Pedido { Id = i, Produtos = new List<Produto>() };
                int numProdutos = random.Next(3, 8); //Entre 3 e 7 produtos
                for (int j = 0; j < numProdutos; j++)
                {
                    int indexProduto = random.Next(produtos.Count);
                    pedido.Produtos.Add(produtos[indexProduto]);
                }

                _pedidos.Add(pedido);
            }
        }

        // GET: api/produtos
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Produto>> GetProdutos(decimal? ValorMaximo)
        {
            if (ValorMaximo.HasValue)
            {
                return produtos.Where(p => p.Preco <= ValorMaximo.Value).ToList();
            }
            else
            {
                return Ok(produtos);
            }
        }

        // GET: api/pedidos/produtos
        [HttpGet("idPedido")]
        public ActionResult<IEnumerable<Produto>> GetProdutoNoPedidos(int idPedido)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == idPedido);
            if (pedido == null)
            {
                return NotFound("Pedido não encontrado.");
            }

            return Ok(pedido.Produtos);
        }

        // GET: api/pedidos
        [HttpGet("pedidos")]
        public ActionResult<IEnumerable<Pedido>> GetPedidos()
        {
            if (_pedidos == null || !_pedidos.Any())
            {
                return NotFound("Nenhum pedido encontrado.");
            }

            return Ok(_pedidos);
        }

        // POST: api/pedidos/produtos
        [HttpPost("produtos")]
        public ActionResult<IEnumerable<Produto>> AdicionarProduto([FromBody] Produto novoProduto)
        {
            // Verifica se o novoProduto é nulo
            if (novoProduto == null)
            {
                return BadRequest("Produto inválido.");
            }

            // Verifica se os campos do novoProduto são nulos ou em branco
            if (string.IsNullOrWhiteSpace(novoProduto.Nome) || novoProduto.Preco <= 0)
            {
                return BadRequest("Os campos do produto não podem ser nulos ou em branco.");
            }

            // Gera um novo Id baseado no maior Id existente na lista de produtos
            int novoId = produtos.Max(p => p.Id) + 1;
            novoProduto.Id = novoId;

            produtos.Add(novoProduto);

            // Retorna a lista de produtos
            return Ok(produtos);
        }




    }
}
