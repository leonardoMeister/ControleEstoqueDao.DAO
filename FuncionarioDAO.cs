using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;

namespace ControleEstoqueDao.DAO
{
    public class Funcionario
    {
        public int IdFuncionario { get; set; }
        public string Cpf { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public int Grupo { get; set; }
        public int LojaId { get; set; }
        public string Cargo { get; set; }
        public DateTime DataNascimento { get; set; }
        public char Sexo { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int EnderecoId { get; set; }

        /// <summary>
        /// Pega a representação do objeto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Nome: {Nome}, Id: {IdFuncionario}";
        }
        /// <summary>
        /// Construtor
        /// </summary>
        public Funcionario()
        {

        }
        /// <summary>
        /// Sobrecarga Construtor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nome"></param>
        public Funcionario(int id, string nome)
        {
            this.IdFuncionario = id;
            this.Nome = nome;
        }

    }
    public class FuncionarioDAO
    {
        private DbProviderFactory factory;
        public FuncionarioDAO()
        {
        }

        /// <summary>
        /// Fazendo Select no banco
        /// </summary>
        /// <param name="provider">Qual o banco provedor</param>
        /// <param name="stringConexao">Conexao com o banco</param>
        /// <param name="marca">Objeto a selecionar</param>
        /// <returns></returns>
        public DataTable SelectDbProvider(string provider, string stringConexao, Funcionario funcionario)
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

                    if (funcionario.Nome != null && funcionario.Nome != "")
                    {
                        var nome = comando.CreateParameter();
                        nome.ParameterName = "@nome";
                        nome.Value = "%" + funcionario.Nome + "%";
                        comando.Parameters.Add(nome);

                        comando.CommandText = @"SELECT  id_funcionario 'Id', nome 'Nome',cpf 'Cpf',matricula 'Matricula',
		                                        grupo 'Grupo', cargo 'Cargo',data_nascimento 'Data Nascimento',
										        sexo 'Sexo',telefone 'Telefone', email 'Email', endereco_id 'Id Endereco',
                                                loja_id 'Loja'
                                                FROM tb_funcionario WHERE nome like @nome";
                    }
                    else
                    {
                        string auxSql = "";
                        if (funcionario.IdFuncionario != 0)
                        {
                            auxSql = $"where id_funcionario = '{funcionario.IdFuncionario}'";
                        }
                        //Criando query
                        comando.CommandText = @"SELECT  id_funcionario 'Id', nome 'Nome',cpf 'Cpf',matricula 'Matricula',
		                                        grupo 'Grupo', cargo 'Cargo',data_nascimento 'Data Nascimento',
										        sexo 'Sexo',telefone 'Telefone', email 'Email', endereco_id 'Id Endereco',
                                                loja_id 'Loja'
                                                FROM tb_funcionario aa
                                                inner join tb_endereco bb 
                                                on bb.id_endereco = aa.endereco_id" + " "+  auxSql;

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
        /// Validando login
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="stringConexao"></param>
        /// <param name="funcionario"></param>
        /// <returns></returns>
        public DataTable ValidaLogin(string provider, string stringConexao, Funcionario funcionario)
        {
            factory = DbProviderFactories.GetFactory(provider);
            using (var conexao = factory.CreateConnection()) //Cria conexão
            {
                conexao.ConnectionString = stringConexao;
                using (var comando = factory.CreateCommand()) //Cria comando
                {
                    comando.Connection = conexao;

                    var cpf = comando.CreateParameter();
                    cpf.ParameterName = "@cpf";
                    cpf.Value = funcionario.Cpf;
                    comando.Parameters.Add(cpf);

                    var senha = comando.CreateParameter();
                    senha.ParameterName = "@senha";
                    senha.Value = funcionario.Senha;
                    comando.Parameters.Add(senha);

                    conexao.Open();
                    comando.CommandText = @"" +
                    @"SELECT f.id_funcionario AS ID, f.nome AS Nome, f.cpf AS CPF, f.matricula AS Matricula,
                f.grupo AS Grupo, f.cargo AS Cargo, f.loja_id AS Loja, l.tipo AS TipoLoja " +
                "FROM tb_funcionario f " +
                "INNER JOIN tb_loja l ON f.loja_id = l.id_loja " +
                "WHERE cpf = @cpf and senha = @senha;";

                    //Executa o script na conexão e retorna as linhas afetadas.
                    var sdr = comando.ExecuteReader();
                    DataTable linhas = new DataTable();
                    linhas.Load(sdr);
                    return linhas;
                }
            } //using faz o conexao.Close() automático quando fecha o seu escopo
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
        /// Fazendo Select no banco de apenas Gerentes
        /// </summary>
        /// <param name="provider">Qual o banco provedor</param>
        /// <param name="stringConexao">Conexao com o banco</param>
        /// <param name="marca">Objeto a selecionar</param>
        /// <returns></returns>
        public DataTable SelectDbProviderFuncionarioID(string provider, string stringConexao)
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
                    //criando query
                    comando.CommandText = @"select id_funcionario 'id_funcionario', nome 'Nome Gerente'
                                            from tb_funcionario
                                            where cargo = 'Gerente'";
                    //Executa o script na conexão e retorna as linhas afetadas.
                    var sdr = comando.ExecuteReader();
                    DataTable linhas = new DataTable();
                    linhas.Load(sdr);
                    return linhas;
                }
            }
        }


        /// <summary>
        /// Inserindo no banco
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="stringConexao"></param>
        /// <param name="funcionario"></param>
        public void InserirDbProvider(string provider, string stringConexao, Funcionario funcionario)
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
                    var nomeFuncionario = comando.CreateParameter();
                    nomeFuncionario.ParameterName = "@nomeFuncionario";
                    nomeFuncionario.Value = funcionario.Nome;
                    comando.Parameters.Add(nomeFuncionario);

                    var cargo = comando.CreateParameter();
                    cargo.ParameterName = "@cargo";
                    cargo.Value = funcionario.Cargo;
                    comando.Parameters.Add(cargo);

                    var lojaId = comando.CreateParameter();
                    lojaId.ParameterName = "@LojaId";
                    lojaId.Value = funcionario.LojaId;
                    comando.Parameters.Add(lojaId);

                    var cpf = comando.CreateParameter();
                    cpf.ParameterName = "@cpf";
                    cpf.Value = funcionario.Cpf;
                    comando.Parameters.Add(cpf);

                    var data = comando.CreateParameter();
                    data.ParameterName = "@dataNascimento";
                    data.Value = funcionario.DataNascimento;
                    comando.Parameters.Add(data);

                    var email = comando.CreateParameter();
                    email.ParameterName = "@email";
                    email.Value = funcionario.Email;
                    comando.Parameters.Add(email);

                    var enderecoId = comando.CreateParameter();
                    enderecoId.ParameterName = "@enderecoId";
                    enderecoId.Value = funcionario.EnderecoId;
                    comando.Parameters.Add(enderecoId);

                    var grupo = comando.CreateParameter();
                    grupo.ParameterName = "@grupo";
                    grupo.Value = funcionario.Grupo;
                    comando.Parameters.Add(grupo);

                    var matricula = comando.CreateParameter();
                    matricula.ParameterName = "@matricula";
                    matricula.Value = funcionario.Matricula;
                    comando.Parameters.Add(matricula);

                    var senha = comando.CreateParameter();
                    senha.ParameterName = "@senha";
                    senha.Value = funcionario.Senha;
                    comando.Parameters.Add(senha);

                    var sexo = comando.CreateParameter();
                    sexo.ParameterName = "@sexo";
                    sexo.Value = funcionario.Sexo;
                    comando.Parameters.Add(sexo);

                    var telefone = comando.CreateParameter();
                    telefone.ParameterName = "@telefone";
                    telefone.Value = funcionario.Telefone;
                    comando.Parameters.Add(telefone);

                    //Abre conexão
                    conexao.Open();

                    if (funcionario.IdFuncionario != 0)
                    {
                        var id = comando.CreateParameter();
                        id.ParameterName = "@id";
                        id.Value = funcionario.IdFuncionario;
                        comando.Parameters.Add(id);

                        comando.CommandText = @"UPDATE tb_funcionario SET nome= @nomeFuncionario , cpf = @cpf , matricula = @matricula ,
                                                grupo = @grupo , data_nascimento = @dataNascimento ,  sexo = @sexo , telefone = @telefone,
                                                email = @email , senha = @senha , loja_id = @LojaId
                                                WHERE id_funcionario = @Id; ";
                    }
                    else
                    {
                        //Script para inserir com os parâmetros adicionados
                        comando.CommandText = @"INSERT INTO tb_funcionario (nome,cpf,matricula,
                                          grupo,cargo,data_nascimento,sexo,telefone,email,senha,endereco_id, loja_id ) 
                                          VALUES (@nomeFuncionario,@cpf,@matricula,@grupo, @cargo,@dataNascimento,
                                          @sexo,@telefone,@email,@senha,@enderecoId,@LojaId)";
                    }
                    //Executa o script na conexão e retorna o número de linhas afetadas.
                    var linhas = comando.ExecuteNonQuery();
                    //fecha conexão
                    conexao.Close();
                }
            }
        }
    }
}
