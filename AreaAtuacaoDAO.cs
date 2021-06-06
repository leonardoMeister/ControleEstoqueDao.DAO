using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace ControleEstoqueDao.DAO
{
    public class AreaAtuacao
    {
        public AreaAtuacao(int id, string nome)
        {
            IdArea = id;
            Area = nome;
        }

        public AreaAtuacao()
        {
        }
        public override string ToString()
        {
            return $"Area: {Area}, Id: {IdArea}";
        }

        public int IdArea { get; set; }
        public string Area { get; set; }

    }

    /*
        id_area INT IDENTITY(1,1)  NOT NULL,
        area VARCHAR(100)  NOT NULL,
        PRIMARY KEY(id_area),
     */
    public class AreaAtuacaoDAO
    {
        private DbProviderFactory factory;

        public AreaAtuacaoDAO()
        {
        }
        /// <summary>
        /// Pega os dados para usar nos listBox de fornecedor
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="stringConexao"></param>
        /// <param name="id"></param>
        /// <param name="aux">Define se vai pegar a lista do fornecedor ou não</param>
        /// <returns></returns>
        public DataTable PegarAreasParaFornecedor(string provider, string stringConexao, int id, bool aux)
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

                    //verifica se tem filtro
                    if (aux)
                    {
                        comando.CommandText = $@"select * from tb_area_atuacao 
                                                where id_area in  (select area_id from tb_fornecedor_areas_atuacao
                                                where fornecedor_id = {id}) ";
                    }
                    else
                    {
                        comando.CommandText = $@"select * from tb_area_atuacao 
                                                where id_area not in  (select area_id from tb_fornecedor_areas_atuacao
                                                where fornecedor_id = {id}) ";
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
        /// Fazendo Select no banco
        /// </summary>
        /// <param name="provider">Qual o banco provedor</param>
        /// <param name="stringConexao">Conexao com o banco</param>
        /// <param name="areaAtuacao">Objeto a selecionar</param>
        /// <returns></returns>
        public DataTable SelectDbProvider(string provider, string stringConexao, AreaAtuacao areaAtuacao)
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
                    if (areaAtuacao.Area != null && areaAtuacao.Area != "")
                    {
                        var nome = comando.CreateParameter();
                        nome.ParameterName = "@nome";
                        nome.Value = "%" + areaAtuacao.Area + "%";
                        comando.Parameters.Add(nome);

                        comando.CommandText = @"SELECT id_area AS ID, area AS Nome  FROM tb_area_atuacao WHERE area like @nome";
                    }
                    else
                    {
                        string auxSql = "";
                        if (areaAtuacao.IdArea != 0)
                        {
                            auxSql = $"where id_area = '{areaAtuacao.IdArea}'";
                        }

                        //verifica se tem filtro
                        comando.CommandText = @"SELECT id_area AS ID, area AS Nome FROM tb_area_atuacao " + " " + auxSql;
                    }
                    //Executa o script na conexão e retorna as linhas afetadas.
                    var sdr = comando.ExecuteReader();
                    DataTable linhas = new DataTable();
                    linhas.Load(sdr);
                    return linhas;
                }
            }       //using faz o conexao.Close() automático quando fecha o seu escopo
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
                    comando.CommandText = $"delete from tb_area_atuacao where id_area = {id}";
                    //Executa o script na conexão e retorna o número de linhas afetadas.
                    var linhas = comando.ExecuteNonQuery();
                    //fecha conexão
                    conexao.Close();
                }
            }
        }

        public void InserirDbProvider(string provider, string stringConexao, AreaAtuacao areaAtuacao)
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
                    nomeMarca.ParameterName = "@nomeArea";
                    nomeMarca.Value = areaAtuacao.Area;
                    comando.Parameters.Add(nomeMarca);

                    //Abre conexão
                    conexao.Open();

                    if (areaAtuacao.IdArea != 0)
                    {
                        var id = comando.CreateParameter();
                        id.ParameterName = "@Id";
                        id.Value = areaAtuacao.IdArea;
                        comando.Parameters.Add(id);

                        comando.CommandText = @"UPDATE tb_area_atuacao SET area= @nomeArea WHERE id_area = @Id; ";
                    }
                    else
                    {
                        //Script para inserir com os parâmetros adicionados
                        comando.CommandText = @"INSERT INTO tb_area_atuacao(area) VALUES (@nomeArea)";
                    }
                    //Executa o script na conexão e retorna o número de linhas afetadas.
                    var linhas = comando.ExecuteNonQuery();
                    //fecha conexão
                    conexao.Close();
                }
            }                                                       //using faz o conexao.Close() automático quando fecha o seu escopo
        }
    }
}
