using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace ControleEstoqueDao.DAO
{
    public class Marca
    {
        public Marca(int id, string nomeM)
        {
            IdMarca = id;
            Nome = nomeM;
        }

        public Marca()
        {
        }
        public override string ToString()
        {
            return $"Marca: {Nome}, Id: {IdMarca}";
        }

        public int IdMarca { get; set; }
        public string Nome { get; set; }


    }
    /*
     id_marca INT IDENTITY(1,1) NOT NULL,
	marca VARCHAR(100)  NOT NULL,
	PRIMARY KEY(id_marca),
     */
    public class MarcaDAO
    {
        private DbProviderFactory factory;
        public MarcaDAO()
        {

        }

        /// <summary>
        /// Fazendo Select no banco
        /// </summary>
        /// <param name="provider">Qual o banco provedor</param>
        /// <param name="stringConexao">Conexao com o banco</param>
        /// <param name="marca">Objeto a selecionar</param>
        /// <returns></returns>
        public DataTable SelectDbProvider(string provider, string stringConexao, Marca marca)
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

                    //verificando se precisa de uma condicao
                    if (marca.Nome != "" && marca.Nome != null)
                    {
                        var nome = comando.CreateParameter();
                        nome.ParameterName = "@nome";
                        nome.Value = "%" + marca.Nome + "%";
                        comando.Parameters.Add(nome);

                        comando.CommandText = @"SELECT id_marca AS ID_marca, marca AS Nome_marca FROM tb_marca WHERE marca like @nome";
                    }
                    else
                    {
                        string auxSql = "";
                        if (marca.IdMarca != 0)
                        {
                            auxSql = $"where id_marca = '{marca.IdMarca}'";
                        }


                        //verifica se tem filtro
                        comando.CommandText = @"SELECT id_marca AS ID_marca, marca AS Nome_marca FROM tb_marca" + " " + auxSql;
                    }

                    //Executa o script na conexão e retorna as linhas afetadas.
                    var sdr = comando.ExecuteReader();
                    DataTable linhas = new DataTable();
                    linhas.Load(sdr);
                    return linhas;
                }
            }       //using faz o conexao.Close() automático quando fecha o seu escopo
        }
        /// <summary>
        /// Inserindo no banco
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="stringConexao"></param>
        /// <param name="marca"></param>
        public void InserirDbProvider(string provider, string stringConexao, Marca marca)
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
                    var nomeMarca = comando.CreateParameter();
                    nomeMarca.ParameterName = "@nomeMarca";
                    nomeMarca.Value = marca.Nome;
                    comando.Parameters.Add(nomeMarca);

                    //Abre conexão
                    conexao.Open();

                    if (marca.IdMarca != 0)
                    {
                        var id = comando.CreateParameter();
                        id.ParameterName = "@Id";
                        id.Value = marca.IdMarca;
                        comando.Parameters.Add(id);

                        comando.CommandText = @"UPDATE tb_marca SET marca= @nomeMarca WHERE id_marca = @Id; ";
                    }
                    else
                    {
                        //Script para inserir com os parâmetros adicionados
                        comando.CommandText = @"INSERT INTO tb_marca(marca) VALUES (@nomeMarca)";
                    }                                                 //Executa o script na conexão e retorna o número de linhas afetadas.
                    var linhas = comando.ExecuteNonQuery();
                    //fecha conexão
                    conexao.Close();
                }
            }                                                       //using faz o conexao.Close() automático quando fecha o seu escopo
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
                    comando.CommandText = $"delete from tb_marca where id_marca = {id}";
                    //Executa o script na conexão e retorna o número de linhas afetadas.
                    var linhas = comando.ExecuteNonQuery();
                    //fecha conexão
                    conexao.Close();
                }
            }
        }
    }
}
