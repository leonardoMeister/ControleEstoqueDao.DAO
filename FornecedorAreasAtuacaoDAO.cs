using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;

namespace ControleEstoqueDao.DAO
{
    public class FornecedorAreasAtuacao
    {
        public int AreaId { get; set; }
        public int FornecedorId { get; set; }

        public FornecedorAreasAtuacao(int area, int fornecedor)
        {
            AreaId = area;
            FornecedorId = fornecedor;
        }
        public FornecedorAreasAtuacao()
        {
        }
    }
    public class FornecedorAreasAtuacaoDAO
    {
        private DbProviderFactory factory;
        string strConnection = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;
        /// <summary>
        /// COnstrutor
        /// </summary>
        public FornecedorAreasAtuacaoDAO()
        {

        }

        /// <summary>
        /// Fazendo Select no banco
        /// </summary>
        /// <param name="provider">Qual o banco provedor</param>
        /// <param name="stringConexao">Conexao com o banco</param>
        /// <param name="marca">Objeto a selecionar</param>
        /// <returns></returns>
        public DataTable SelectDbProvider(string provider, string stringConexao)
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
                    //Pegando query
                    comando.CommandText = @"select fornecedor_id 'Id Fornecedor', area_id 'Id Area'
                                          from tb_fornecedor_areas_atuacao";
                    //Executa o script na conexão e retorna as linhas afetadas.
                    var sdr = comando.ExecuteReader();
                    DataTable linhas = new DataTable();
                    linhas.Load(sdr);
                    return linhas;
                }
            }       //using faz o conexao.Close() automático quando fecha o seu escopo
        }

        /// <summary>
        /// Removendo do banco
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="strConnection"></param>
        /// <param name="id"></param>
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

                    comando.CommandText = $"delete from tb_fornecedor_areas_atuacao where fornecedor_id = {id}";
                    //Executa o script na conexão e retorna o número de linhas afetadas.
                    var linhas = comando.ExecuteNonQuery();
                    //fecha conexão
                    conexao.Close();
                }
            }
        }

        /// <summary>
        /// INserindo no banco
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="stringConexao"></param>
        /// <param name="produto"></param>
        public void InserirDbProvider(string provider, string stringConexao, FornecedorAreasAtuacao fornecedorAreasAtuacao)
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
                    var areaId = comando.CreateParameter();
                    areaId.ParameterName = "@AreaId";
                    areaId.Value = fornecedorAreasAtuacao.AreaId;
                    comando.Parameters.Add(areaId);

                    var fornecedorId = comando.CreateParameter();
                    fornecedorId.ParameterName = "@FornecedorId";
                    fornecedorId.Value = fornecedorAreasAtuacao.FornecedorId;
                    comando.Parameters.Add(fornecedorId);

                    //Abre conexão
                    conexao.Open();
                    //Script para inserir com os parâmetros adicionados
                    comando.CommandText = @"INSERT INTO tb_fornecedor_areas_atuacao(fornecedor_id, area_id)
                                            VALUES (@FornecedorId, @AreaId)";
                    //Executa o script na conexão e retorna o número de linhas afetadas.
                    comando.ExecuteNonQuery();
                }
            }
        }


    }
}
