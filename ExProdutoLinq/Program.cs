using System;
using ExProdutoLinq.Entidades;


namespace ExprodutoLinq
{
    class Program
    {
        static void PrintarLista<Produto>(string message, IEnumerable<Produto> produtos)
        {
            Console.WriteLine(message);
            foreach (Produto obj in produtos)
            {
                Console.WriteLine(obj);
            }
            Console.WriteLine();
        }

        static void Main(string[] ars)
        {
            Categoria c1 = new Categoria { ID = 1, Nome = "Tools", Tipo = 2 };
            Categoria c2 = new Categoria { ID = 2, Nome = "Computeres", Tipo = 1 };
            Categoria c3 = new Categoria { ID = 1, Nome = "Eletronics", Tipo = 1 };

            List<Produto> produtos = new List<Produto>()
            {
                new Produto(){Id = 1, Nome = "Computer", Preco = 1100.0, Categoria = c2},
                new Produto(){Id = 2, Nome = "Hammer", Preco = 90.0, Categoria = c1},
                new Produto(){Id = 3, Nome = "TV", Preco = 1700.0, Categoria = c3},
                new Produto(){Id = 4, Nome = "Notebook", Preco = 130.0, Categoria = c2},
                new Produto(){Id = 5, Nome = "Saw", Preco = 80.0, Categoria = c1},
                new Produto(){Id = 6, Nome = "Tablet", Preco = 700.0, Categoria = c2},
                new Produto(){Id = 7, Nome = "Camera", Preco = 700.0, Categoria = c3},
                new Produto(){Id = 8, Nome = "Printer", Preco = 1100.0, Categoria = c3},
                new Produto(){Id = 9, Nome = "MacBook", Preco = 1800.0, Categoria = c2},
                new Produto(){Id = 10, Nome = "Sound Bar", Preco = 700.0, Categoria = c3 },
                new Produto(){Id = 11, Nome = "Level", Preco = 70.0, Categoria = c1}
            };



            // 1- Filtrando produtos Tipo 1 AND Preco < 900:

            //Com function Lambda:  var r1 = produtos.Where(p => p.Categoria.Tipo == 1 && p.Preco < 900.0);
            //Com Sintaxe Similar SQL: 
            var r1 =
                 from T_PRODUTO in produtos
                 where T_PRODUTO.Categoria.Tipo == 1 && T_PRODUTO.Preco < 900.0
                 select T_PRODUTO;
            PrintarLista("Tipo 1 é Preco < 100: ", r1);
            /////////////////////////////////////////////////////////////////////////


            // 2- Filtrar os nome do produto nas categoria Tools

            // Com function lambda: var r2 = produtos.Where(p => p.Categoria.Nome == "Tools").Select(p => p.Nome);
            // Com Sintaxe Similar SQL:
            var r2 =
                from T_PRODUTO in produtos
                where T_PRODUTO.Categoria.Nome == "Tools"
                select T_PRODUTO.Nome;
            PrintarLista("Nome do produto com categoria somente Tools: ", r2);

            /////////////////////////////////////////////////////////////////////////



            // 3- Filtrar os produtos que começa com a letra C e criando um objeto anonimo dentro da minha projeção select

            // com function lambda: var r3 = produtos.Where(p => p.Nome[0] == 'C').Select(p => new { p.Nome, p.Preco, NomeCategoria = p.Categoria.Nome }); //NomeCategoria = Aliás para o atributo da Classe Produto, porq ja estou criando o atributo Nome irá dar ambiguidade.
            // Com Sintaxe Similar SQL:
            var r3 =
            from T_PRODUTO in produtos
            where
               T_PRODUTO.Nome[0] == 'C'
            select new
            {
                T_PRODUTO.Nome,
                T_PRODUTO.Preco,
                NomeCategoria = T_PRODUTO.Categoria.Nome
            };
            PrintarLista("Produtos que começa com a letra C: ", r3);

            /////////////////////////////////////////////////////////////////////////////////




            // 4- Filtrando só as categorias tipo 1 and Ordenando por preço e nome.

            // Function com Lambda: var r4 = produtos.Where(p => p.Categoria.Tipo == 1).OrderBy(p => p.Preco).ThenBy(p => p.Nome);
            //Sintaxe Similar a SQL:
            var r4 =
                from T_PRODUTO in produtos
                where
                    T_PRODUTO.Categoria.Tipo == 1
                orderby
                    T_PRODUTO.Preco
                orderby
                    T_PRODUTO.Nome
                select
                    T_PRODUTO;
            PrintarLista("Categoria tipo por 1 e ordenado por preco e nome: ", r4);

            /////////////////////////////////////////////////////////////////////


            // 5- Filtrando uma base de dado Pulando tal elementos e pegando tal elementos

            // Sintaxe Similar SQL:

            var r5 =
            (from T_PRODUTO in r4
             select
                 T_PRODUTO).Skip(2).Take(4);

            PrintarLista("Pulando dois elementos da minha base de dados r4 e pegando somente 4 elementos dela após o pulo: ", r5);

            /////////////////////////////////////////////////////////////////////


            // 6- Operações que pega os elementos.
            //Sinta Similar ao SQL:
            var r6 =
            (from T_PRODUTO in produtos
             select T_PRODUTO).FirstOrDefault();
            Console.WriteLine("Pegando o primeiro elemento: " + r6); //Pegando o primeiro elemento da minha lista.

            // Function Lambdia: var r7 = produtos.Where(p => p.Preco > 3000.0).FirstOrDefault(); //Erro caso não encontrar valor no first retorna null
            //Sintaxe Similiar SQL:
            var r7 =
            (from T_PRODUTO in produtos
             where
                 T_PRODUTO.Preco > 3000.0
             select
                 T_PRODUTO).FirstOrDefault(); //Encontrando valor que não existe na query e retornando null
            Console.WriteLine("Erro first" + r7);

            Console.WriteLine();

            //Operação de Agrupamento.


            // 9 - Agrupando nome dos Produtos que tem relacionamento a cada Categoria.

            // Function lambda: var r16 = produtos.GroupBy(p => p.Categoria);
            // Sintaxe Similar ao SQL:
            var r16 =
            from T_PRODUTO in produtos
            group
                T_PRODUTO by T_PRODUTO.Categoria;
               
            foreach (IGrouping<Categoria, Produto> categoria in r16)
            {
                Console.WriteLine("Categoria: " + categoria.Key.Nome + ": ");
                foreach (Produto produto in categoria)
                {
                    Console.WriteLine(produto);
                }
                Console.WriteLine();
            }
        }


        /////////////////////////////////////////////////////////////////////////////

            // Aluns outros Metodos do Linq

             /*Operação que pega somente um registro ID.
            var r8 = produtos.Where(p => p.Id == 3).SingleOrDefault();//Pega um simples elemento , funciona so se o where pega UM resultado.
            Console.WriteLine("Pegando somente um Registro: " + r8);
            var r9 = produtos.Where(p => p.Id == 30).SingleOrDefault();
            Console.WriteLine("Pegando um registro ID que não existe: " + r9);

            Console.WriteLine();

             8- Operação de Agregação
            var r10 = produtos.Max(p => p.Preco); //Pegando o max do Preço
            Console.WriteLine("Pegando o maximo de preço na minha list: " + r10);
            var r11 = produtos.Min(p => p.Preco); //Pegando o minimo da minha lista
            Console.WriteLine("Pegando o minimo da minha lista: " + r11);
            var r12 = produtos.Where(p => p.Categoria.ID == 1).Sum(p => p.Preco);//Fazendo a soma de valores
            Console.WriteLine("Pegando todos ID 1 da classe Categoria e fazendo a soma dos preço dos produtos: " + r12);
            var r13 = produtos.Where(p => p.Categoria.ID == 1).Average(p => p.Preco); //Fazendo a media de tal valores.
            Console.WriteLine("Pegando a média geral do dos produtos que tem a categoria ID 1: " + r13);

            Fazendo media de preço de um produto mais se não existir uma categoria nele, fazer não da divisão por zero.
            var r14 = produtos.Where(p => p.Categoria.ID == 5).Select(p => p.Preco).DefaultIfEmpty(0.0).Average();
            Console.WriteLine("Pegando um Conjunto que está relacionado a categoria 5, mais nao existe: " + r14);


            Fazendo agregação personalizada (Ex: Fazendo soma de Preços).

            var r15 = produtos.Where(p => p.Categoria.ID == 1).Select(p => p.Preco).Aggregate(0.0/*Valor inicial caso for zero*/,(x, y) => x + y);
            //Console.WriteLine("Soma com Agregação personalizada: " + r15);

        }
}
