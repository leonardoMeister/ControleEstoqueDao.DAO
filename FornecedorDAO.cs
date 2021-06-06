using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;

namespace ControleEstoqueDao.DAO
{
    public class Fornecedor
    {
        public int IdFornecedor { get; set; }
        public string Cnpj { get; set; }
        public string Ie { get; set; }
        public string Contato { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public int EnderecoId { get; set; }

        public Fornecedor()
        {

        }
        public Fornecedor(int id, string nome)
        {
            this.NomeFantasia = nome;
            this.IdFornecedor = id;
        }

    }

    public class FornecedorDAO
    {
        private DbProviderFactory factory;
        public FornecedorDAO()
        {
        }

        /// <summary>
        /// Fazendo Select no banco
        /// </summary>
        /// <param name="provider">Qual o banco provedor</param>
        /// <param name="stringConexao">Conexao com o banco</param>
        /// <param name="marca">Objeto a selecionar</param>
        /// <returns></returns>
        public DataTable SelectDbProvider(string provider, string stringConexao, Fornecedor fornecedor)
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
                    if (fornecedor.NomeFantasia != null && fornecedor.NomeFantasia != "")
                    {
                        var nome = comando.CreateParameter();
                        nome.ParameterName = "@nome";
                        nome.Value = "%" + fornecedor.NomeFantasia + "%";
                        comando.Parameters.Add(nome);

                        comando.CommandText = @"SELECT id_fornecedor 'Id Fornecedor', cnpj 'CNPJ', ie 'IE',
                                                razao_social 'Razao Social', nome_fantasia 'Nome Fantasia',
                                                telefone 'Telefone', email 'Email', 
                                                contato 'Contato', endereco_id 'Id Endereco'
                                                FROM tb_fornecedor WHERE nome_fantasia like @nome";
                    }
                    else
                    {
                        string auxSql = "";
                        if (fornecedor.IdFornecedor != 0)
                        {
                            auxSql = $"Where id_fornecedor = '{fornecedor.IdFornecedor}'";
                        }
                        //Criando query
                        comando.CommandText = @"select id_fornecedor 'Id Fornecedor', cnpj 'CNPJ', ie 'IE',
                                            razao_social 'Razao Social', nome_fantasia 'Nome Fantasia',
                                            telefone 'Telefone', email 'Email', cidade 'Cidade',
                                            logradouro 'Logradouro' , contato 'Contato', id_endereco 'Id Endereco'
                                            from tb_fornecedor tbf inner join tb_endereco tbe 
                                            on tbe.id_endereco = tbf.endereco_id " + " " + auxSql;

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


                    comando.CommandText = $"delete from tb_endereco where id_endereco = {id}";
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
        public int InserirDbProvider(string provider, string stringConexao, Fornecedor fornecedor)
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
                    var cnpj = comando.CreateParameter();
                    cnpj.ParameterName = "@Cnpj";
                    cnpj.Value = fornecedor.Cnpj;
                    comando.Parameters.Add(cnpj);

                    var contato = comando.CreateParameter();
                    contato.ParameterName = "@Contato";
                    contato.Value = fornecedor.Contato;
                    comando.Parameters.Add(contato);

                    var email = comando.CreateParameter();
                    email.ParameterName = "@Email";
                    email.Value = fornecedor.Email;
                    comando.Parameters.Add(email);

                    var enderecoId = comando.CreateParameter();
                    enderecoId.ParameterName = "@EnderecoId";
                    enderecoId.Value = fornecedor.EnderecoId;
                    comando.Parameters.Add(enderecoId);

                    var ie = comando.CreateParameter();
                    ie.ParameterName = "@Ie";
                    ie.Value = fornecedor.Ie;
                    comando.Parameters.Add(ie);

                    var nomeFantasia = comando.CreateParameter();
                    nomeFantasia.ParameterName = "@NomeFantasia";
                    nomeFantasia.Value = fornecedor.NomeFantasia;
                    comando.Parameters.Add(nomeFantasia);

                    var razaoSocial = comando.CreateParameter();
                    razaoSocial.ParameterName = "@RazaoSocial";
                    razaoSocial.Value = fornecedor.RazaoSocial;
                    comando.Parameters.Add(razaoSocial);

                    var telefone = comando.CreateParameter();
                    telefone.ParameterName = "@Telefone";
                    telefone.Value = fornecedor.Telefone;
                    comando.Parameters.Add(telefone);

                    //Abre conexão
                    conexao.Open();
                    if (fornecedor.IdFornecedor != 0)
                    {
                        var id = comando.CreateParameter();
                        id.ParameterName = "@id";
                        id.Value = fornecedor.IdFornecedor;
                        comando.Parameters.Add(id);

                        comando.CommandText = @"UPDATE tb_fornecedor SET cnpj= @Cnpj , ie = @Ie , contato = @Contato ,
                                                razao_social = @RazaoSocial , nome_fantasia = @NomeFantasia ,  telefone = @Telefone ,
                                                email = @Email
                                                WHERE id_fornecedor = @Id; ";
                    }
                    else
                    {
                        //ajusta o comando SQL para capturar o ID gerado tanto do SQLServer como do MySQL
                        string auxSQL_ID = (ConfigurationManager.ConnectionStrings["BD"].ProviderName.Contains("MySql")) ? "SELECT  LAST_INSERT_ID(); " : "SELECT SCOPE_IDENTITY(); ";
                        //realiza o INSERT e retorna o ID gerado, necessário para o cadastro de lojas, funcionários, fornecedores, clientes
                        {
                            //comando.CommandText = @"INSERT INTO tb_fornecedor(id_fornecedor, cnpj, ie,razao_social , nome_fantasia, telefone,email, cidade, logradouro) VALUES (@cep, @logradouro, @numero, @complemento, @bairro, @cidade, @Estado, @pais);" + " " + auxSQL_ID;
                        }
                        //criando query
                        comando.CommandText = @"" +
                        @"INSERT INTO tb_fornecedor(endereco_id, cnpj, ie, contato , razao_social , nome_fantasia, telefone, email)
                            VALUES (@EnderecoId, @Cnpj, @Ie, @Contato, @RazaoSocial, @NomeFantasia, @Telefone, @Email);" +
                        " " + auxSQL_ID;
                        //executa o comando no banco de dados e captura o ID gerado
                    }

                    var IdRecebimentoGerado = comando.ExecuteScalar();
                    return Convert.ToInt32(IdRecebimentoGerado);
                }
            }
        }

    }
}
