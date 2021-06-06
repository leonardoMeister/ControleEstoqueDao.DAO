using System;
using System.Data;
using System.Data.Common;
using System.Configuration;

namespace ControleEstoqueDao.DAO
{
    public class Endereco
    {
        public int IdEndereco { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }

        public Endereco(int id)
        {
            this.IdEndereco = id;
        }
        public Endereco()
        {

        }
    }

    public class EnderecoDAO
    {
        private DbProviderFactory factory;
        /// <summary>
        /// Construtor
        /// </summary>
        public EnderecoDAO()
        {
        }

        /// <summary>
        /// Fazendo Select no banco
        /// </summary>
        /// <param name="provider">Qual o banco provedor</param>
        /// <param name="stringConexao">Conexao com o banco</param>
        /// <param name="marca">Objeto a selecionar</param>
        /// <returns></returns>
        public DataTable SelectDbProvider(string provider, string stringConexao, int id)
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
                    if (id != 0)
                    {
                        auxSql = $"where id_endereco = {id}";
                    }

                    //Criando query
                    comando.CommandText = @"SELECT id_endereco 'Id' , cep AS CEP, logradouro AS Logradouro,numero as  Numero,
                                            complemento as Complemento,bairro as Bairro,cidade as Cidade,
                                            uf as Estado,pais as Pais FROM tb_endereco" + " " + auxSql;
                    //Executa o script na conexão e retorna as linhas afetadas.
                    var sdr = comando.ExecuteReader();
                    DataTable linhas = new DataTable();
                    linhas.Load(sdr);
                    return linhas;
                }
            }
        }

        /// <summary>
        /// INserindo no banco
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="stringConexao"></param>
        /// <param name="produto"></param>
        public int InserirDbProvider(string provider, string stringConexao, Endereco endereco)
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
                    var cep = comando.CreateParameter();
                    cep.ParameterName = "@cep";
                    cep.Value = endereco.Cep;
                    comando.Parameters.Add(cep);

                    var bairro = comando.CreateParameter();
                    bairro.ParameterName = "@bairro";
                    bairro.Value = endereco.Bairro;
                    comando.Parameters.Add(bairro);

                    var cidade = comando.CreateParameter();
                    cidade.ParameterName = "@cidade";
                    cidade.Value = endereco.Cidade;
                    comando.Parameters.Add(cidade);

                    var complemento = comando.CreateParameter();
                    complemento.ParameterName = "@complemento";
                    complemento.Value = endereco.Complemento;
                    comando.Parameters.Add(complemento);

                    var estado = comando.CreateParameter();
                    estado.ParameterName = "@estado";
                    estado.Value = endereco.Estado;
                    comando.Parameters.Add(estado);

                    var logradouro = comando.CreateParameter();
                    logradouro.ParameterName = "@logradouro";
                    logradouro.Value = endereco.Logradouro;
                    comando.Parameters.Add(logradouro);

                    var numero = comando.CreateParameter();
                    numero.ParameterName = "@numero";
                    numero.Value = endereco.Numero;
                    comando.Parameters.Add(numero);

                    var pais = comando.CreateParameter();
                    pais.ParameterName = "@pais";
                    pais.Value = endereco.Pais;
                    comando.Parameters.Add(pais);

                    //Abre conexão
                    conexao.Open();
                    if (endereco.IdEndereco != 0)
                    {
                        var id = comando.CreateParameter();
                        id.ParameterName = "@id";
                        id.Value = endereco.IdEndereco;
                        comando.Parameters.Add(id);

                        comando.CommandText = @"UPDATE tb_endereco SET cep= @cep , logradouro = @logradouro , numero = @numero ,
                                                complemento = @complemento , bairro = @bairro ,  cidade = @cidade , uf = @Estado , pais = @pais
                                                WHERE id_endereco = @Id; ";

                    }
                    else
                    {
                        //ajusta o comando SQL para capturar o ID gerado tanto do SQLServer como do MySQL
                        string auxSQL_ID = (ConfigurationManager.ConnectionStrings["BD"].ProviderName.Contains("MySql")) ? "SELECT  LAST_INSERT_ID(); " : "SELECT SCOPE_IDENTITY(); ";
                        //realiza o INSERT e retorna o ID gerado, necessário para o cadastro de lojas, funcionários, fornecedores, clientes
                        comando.CommandText = @"" +
                        "INSERT INTO tb_endereco(cep, logradouro, numero, complemento, bairro, cidade, uf, pais) " +
                        "VALUES (@cep, @logradouro, @numero, @complemento, @bairro, @cidade, @Estado, @pais);" +
                        " " + auxSQL_ID;
                    }

                    //executa o comando no banco de dados e captura o ID gerado
                    var IdRecebimentoGerado = comando.ExecuteScalar();
                    return Convert.ToInt32(IdRecebimentoGerado);
                }
            }
        }

    }
}
