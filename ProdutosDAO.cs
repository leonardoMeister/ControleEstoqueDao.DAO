using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace ControleEstoqueDao.DAO
{
    public class Produtos
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public int Status { get; set; }
        public int AreaId { get; set; }
        public decimal Valor { get; set; }
        public int MarcaId { get; set; }
        public string Modelo { get; set; }
        public string Descricao { get; set; }
        public byte[] Foto { get; set; }
        public int QuantidadeEstoque { get; set; }

        public Produtos()
        {

        }
        public Produtos(int id, string nome)
        {
            this.Nome = nome;
            this.IdProduto = id;
        }
    }
    public class ProdutosDAO
    {
        private DbProviderFactory factory;

        public ProdutosDAO()
        {
        }

        /// <summary>
        /// Fazendo Select no banco
        /// </summary>
        /// <param name="provider">Qual o banco provedor</param>
        /// <param name="stringConexao">Conexao com o banco</param>
        /// <param name="produtos">Objeto a selecionar</param>
        /// <returns></returns>
        public DataTable SelectDbProvider(string provider, string stringConexao, Produtos produto)
        {
            factory = DbProviderFactories.GetFactory(provider);
            using (var conexao = factory.CreateConnection()) //Cria conexão
            {
                //Atribui a string de conexão
                conexao.ConnectionString = stringConexao;
                using (var comando = factory.CreateCommand()) //Cria comando
                {
                    comando.Connection = conexao; //Atribui conexão
                    conexao.Open();
                    string auxSql = "";

                    if (produto.Nome != null && produto.Nome != "")
                    {
                        var nome = comando.CreateParameter();
                        nome.ParameterName = "@nome";
                        nome.Value = "%" + produto.Nome + "%";
                        comando.Parameters.Add(nome);

                        comando.CommandText = @"SELECT id_produto 'Id Produto', nome 'Produto', status 'Status', area_id 'Id Area Atuacao',
                                                valor 'Valor', marca_id 'Id Marca', modelo 'Modelo', descricao 'Descricao' ,foto 'Foto',
                                                quantidade_estoque 'Quantidade Estoque' FROM tb_produto WHERE nome like @nome";
                    }
                    else
                    {
                        if (produto.IdProduto != 0)
                        {
                            auxSql = $"where id_produto = '{produto.IdProduto}'";
                        }
                        //Criando query
                        comando.CommandText = @"SELECT id_produto 'Id Produto',nome 'Nome',status 'Status',
		                                   bb.area 'Area Atuação',   valor 'Valor',
		                                   cc.marca 'Marca', 
                                           modelo 'Modelo',descricao 'Descricao',quantidade_estoque 'Quantidade Estoque', foto 'Foto', id_area 'Id Area'
                                           FROM tb_produto aa
										   inner join tb_area_atuacao bb
										   on bb.id_area = aa.area_id
										   inner join tb_marca cc
										   on cc.id_marca = aa.marca_id" + " " + auxSql;
                    }
                    //Executa o script na conexão e retorna as linhas afetadas.
                    var sdr = comando.ExecuteReader();
                    DataTable linhas = new DataTable();
                    linhas.Load(sdr);
                    return linhas;
                }
            }
        }
        /// <summary>
        /// Inserindo no banco os produtos
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="stringConexao"></param>
        /// <param name="produtos"></param>
        public void InserirDbProvider(string provider, string stringConexao, Produtos produtos)
        {
            factory = DbProviderFactories.GetFactory(provider);
            using (var conexao = factory.CreateConnection())              //Cria conexão
            {
                //Atribui a string de conexão
                conexao.ConnectionString = stringConexao;
                using (var comando = factory.CreateCommand())            //Cria comando
                {
                    //Atribui conexão
                    comando.Connection = conexao;

                    //Adiciona parâmetro (@campo e valor)
                    var nomeProdto = comando.CreateParameter();
                    nomeProdto.ParameterName = "@nomeProduto";
                    nomeProdto.Value = produtos.Nome;
                    comando.Parameters.Add(nomeProdto);

                    var statusProduto = comando.CreateParameter();
                    statusProduto.ParameterName = "@status";
                    statusProduto.Value = produtos.Status;
                    comando.Parameters.Add(statusProduto);

                    var areaId = comando.CreateParameter();
                    areaId.ParameterName = "@AreaId";
                    areaId.Value = produtos.AreaId;
                    comando.Parameters.Add(areaId);

                    var valor = comando.CreateParameter();
                    valor.ParameterName = "@Valor";
                    valor.Value = produtos.Valor;
                    comando.Parameters.Add(valor);

                    var marcaId = comando.CreateParameter();
                    marcaId.ParameterName = "@MarcaId";
                    marcaId.Value = produtos.MarcaId;
                    comando.Parameters.Add(marcaId);

                    var modelo = comando.CreateParameter();
                    modelo.ParameterName = "@Modelo";
                    modelo.Value = produtos.Modelo;
                    comando.Parameters.Add(modelo);

                    var foto = comando.CreateParameter();
                    foto.ParameterName = "@foto";
                    foto.Value = produtos.Foto;
                    comando.Parameters.Add(foto);

                    var descricao = comando.CreateParameter();
                    descricao.ParameterName = "@Descricao";
                    descricao.Value = produtos.Descricao;
                    comando.Parameters.Add(descricao);

                    var qtdEstoque = comando.CreateParameter();
                    qtdEstoque.ParameterName = "@QTDEstoque";
                    qtdEstoque.Value = produtos.QuantidadeEstoque;
                    comando.Parameters.Add(qtdEstoque);

                    //Abre conexão
                    conexao.Open();

                    if (produtos.IdProduto != 0)
                    {
                        var id = comando.CreateParameter();
                        id.ParameterName = "@id";
                        id.Value = produtos.IdProduto;
                        comando.Parameters.Add(id);

                        comando.CommandText = @"UPDATE tb_produto SET nome= @nomeProduto , status = @status , area_id = @AreaId ,
                                                marca_id = @MarcaId , valor = @Valor ,  modelo = @Modelo , descricao = @Descricao ,
                                                quantidade_estoque = @QTDEstoque , foto = @foto
                                                WHERE id_produto = @Id; ";
                    }
                    else
                    {
                        //Script para inserir com os parâmetros adicionados
                        comando.CommandText = @"INSERT INTO tb_produto    
                                           (nome,status,area_id,valor,marca_id,modelo,descricao,quantidade_estoque,foto)
                                           VALUES (@nomeProduto,@status,@AreaId,@Valor,@MarcaId,@Modelo,@Descricao, @QTDEstoque, @foto)";

                    }
                    //Executa o script na conexão e retorna o número de linhas afetadas.
                    var linhas = comando.ExecuteNonQuery();
                    //fecha conexão
                    conexao.Close();
                }
            }
        }

        public void RemoverDbProvider(string provider, string stringConexao, int id)
        {
            factory = DbProviderFactories.GetFactory(provider);
            using (var conexao = factory.CreateConnection())              //Cria conexão
            {
                //Atribui a string de conexão
                conexao.ConnectionString = stringConexao;
                using (var comando = factory.CreateCommand())            //Cria comando
                {
                    //Atribui conexão
                    comando.Connection = conexao;

                    //Abre conexão
                    conexao.Open();
                    //Script para inserir com os parâmetros adicionados
                    comando.CommandText = $"delete from tb_produto where id_produto = {id}";
                    //Executa o script na conexão e retorna o número de linhas afetadas.
                    var linhas = comando.ExecuteNonQuery();
                    //fecha conexão
                    conexao.Close();
                }
            }
        }
    }
}
