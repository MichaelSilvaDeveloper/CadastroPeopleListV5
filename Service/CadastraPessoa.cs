using DataBaseConnection;
using System;
using System.Data.SqlClient;
using Validation;

namespace Service
{
    public class CadastraPessoa : ICadastraPessoa
    {
        private static readonly Connection _connection = new Connection();
        private static readonly SqlCommand _sqlCommand = new SqlCommand();
        private readonly ValidateCpf _validateCpf = new ValidateCpf();
        SqlDataReader sqlDataReader;

        public void AddPerson()
        {
            Console.WriteLine("Informe o nome da pessoa: ");
            string name = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Informe a idade da pessoa: ");
            int age = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("Informe o telefone da pessoa:");
            int telephone = Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("Informe o cpf da pessoa: ");
            string cpf = Convert.ToString(Console.ReadLine());

            while (!_validateCpf.IsValidCpf(cpf))
            {
                Console.WriteLine("Cpf inválido, favor inserir um cpf válido: ");
                cpf = Convert.ToString(Console.ReadLine());
            } 

            try
            {
                _sqlCommand.Connection = _connection.Connect();
                string strSql = "select Id_Pessoa from Pessoa where Cpf = " + cpf;
                _sqlCommand.CommandText = strSql;
                sqlDataReader = _sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    Console.WriteLine("Já existe um registro com esse cpf");
                }
                else
                {
                    if (!sqlDataReader.IsClosed) { sqlDataReader.Close(); }
                    sqlDataReader.Close();
                    strSql = String.Format("insert into Pessoa (Nome, Idade, Telefone, Cpf) values ( '{0}','{1}','{2}','{3}')", name, age, telephone, cpf);
                    _sqlCommand.CommandText = strSql;
                    _sqlCommand.ExecuteNonQuery();
                    Console.WriteLine("Cadastrado com sucesso");
                }
                _connection.Disconnect();
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(sqlException.Message);
            }
        }

        public void ShowPeople()
        {
            Console.WriteLine(" - Pessoas Cadastradas - ");
            try
            {
                _sqlCommand.Connection = _connection.Connect();
                string strSql = "select * from Pessoa";
                _sqlCommand.CommandText = strSql;
                sqlDataReader = _sqlCommand.ExecuteReader();
                if (!sqlDataReader.HasRows)
                {
                    Console.WriteLine("Não existem pessoas cadastradas");
                }
                else
                {
                    while (sqlDataReader.Read())
                    {
                        Console.WriteLine("Id: {0}, Nome: {1}, Idade: {2}, Telefone: {3}, Cpf: {4}", sqlDataReader["Id_Pessoa"], sqlDataReader["Nome"], sqlDataReader["Idade"], sqlDataReader["Telefone"], sqlDataReader["Cpf"]);
                    }
                }
                _connection.Disconnect();
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(sqlException.Message);
            }        
        }

        public void SearchPerson()
        {
            Console.WriteLine(" - Buscar Pessoa Por Cpf - ");
            Console.WriteLine("Informe o cpf da pessoa: ");
            string searchPersonByCpf = Convert.ToString(Console.ReadLine());
            try
            {
                _sqlCommand.Connection = _connection.Connect();
                string strSql = "select * from Pessoa where Cpf = " + searchPersonByCpf;
                _sqlCommand.CommandText = strSql;
                sqlDataReader = _sqlCommand.ExecuteReader();
                if (!sqlDataReader.HasRows)
                {
                    Console.WriteLine("Item não encontrado");
                }
                else
                {
                    while (sqlDataReader.Read())
                    {
                        Console.WriteLine("Id: {0}, Nome: {1}, Idade: {2}, Telefone: {3}, Cpf: {4}", sqlDataReader["Id_Pessoa"], sqlDataReader["Nome"], sqlDataReader["Idade"], sqlDataReader["Telefone"], sqlDataReader["Cpf"]);
                    }
                }
                _connection.Disconnect();
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(sqlException.Message);
            }
        }

        public void RemovePerson()
        {
            Console.WriteLine(" - Remover Pessoa - ");
            Console.WriteLine("Informe o cpf da pessoa que deseja remover: ");
            string removePersonByCpf = Convert.ToString(Console.ReadLine());
            try
            {
                _sqlCommand.Connection = _connection.Connect();
                string strSql = "select * from Pessoa where Cpf = " + removePersonByCpf;
                _sqlCommand.CommandText = strSql;
                sqlDataReader = _sqlCommand.ExecuteReader();
                if (!sqlDataReader.HasRows)
                {
                    Console.WriteLine("Pessoa não encontrada com esse CPF");
                }
                else
                {
                    if (!sqlDataReader.IsClosed) { sqlDataReader.Close(); }
                    sqlDataReader.Close();
                    strSql = "delete from Pessoa where Cpf = " + removePersonByCpf;
                    _sqlCommand.CommandText = strSql;
                    _sqlCommand.ExecuteNonQuery();
                    Console.WriteLine("Pessoa excluída");
                }
                _connection.Disconnect();
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(sqlException.Message);
            }
        }

        public void EditPerson()
        {
            Console.WriteLine(" - Editar Pessoa - ");
            Console.WriteLine("Informe o cpf da pessoa que deseja editar: ");
            string cpfPerson = Convert.ToString(Console.ReadLine());
            try
            {
                _sqlCommand.Connection = _connection.Connect();
                string strSql = "select * from Pessoa where Cpf = " + cpfPerson;
                _sqlCommand.CommandText = strSql;
                sqlDataReader = _sqlCommand.ExecuteReader();
                if (!sqlDataReader.HasRows)
                {
                    Console.WriteLine("Pessoa não encontrada");
                }
                else
                {
                    if (!sqlDataReader.IsClosed) { sqlDataReader.Close(); }
                    sqlDataReader.Close();
                    Console.WriteLine("Informe o novo nome da pessoa: ");
                    string newNamePerson = Convert.ToString(Console.ReadLine());
                    strSql = "update Pessoa set Nome = @nome where Cpf = " + cpfPerson;
                    _sqlCommand.Parameters.AddWithValue("@Nome", newNamePerson);
                    _sqlCommand.CommandText = strSql;
                    _sqlCommand.ExecuteNonQuery();
                }
                _connection.Disconnect();
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(sqlException.Message);
            }
        }
    }
}