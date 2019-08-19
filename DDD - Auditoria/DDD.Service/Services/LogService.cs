using FluentValidation;
using DDD.Domain.Entities;
using DDD.Domain.Interfaces;
using DDD.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.SQLite;


namespace DDD.Service.Services
{
    public class LogService
    {
        private string _connectionString = @"Data Source=C:\Users\638228\Desktop\AAW - Atividade 04-20190710T225844Z-001\AAW - Atividade 04\DDD_ContaCorrente\DDD.Application\app.db";

        public Boolean gravarMovimento(int idConta, 
                                        DateTime DataAlteracao, 
                                        double ValorMovimentado, 
                                        char tipoOperacao)
        {
            int resultado = -1;

            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = " INSERT INTO Log (IdContaCorrente, DataAlteracao. ValorMovimentado, TipoOperacao) VALUES (IdContaCorrente = @idConta, DataAlteracao = @dataAlteracao, ValorMovimentado = @ValorMovimentado, TipoOperacao = @tipoOperacao  ";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@idConta", idConta);
                    cmd.Parameters.AddWithValue("@dataAlteracao", DataAlteracao);
                    cmd.Parameters.AddWithValue("@ValorMovimentado", ValorMovimentado);
                    cmd.Parameters.AddWithValue("@tipoOperacao", tipoOperacao);
                    try
                    {
                        resultado = cmd.ExecuteNonQuery();
                        if (resultado == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }

            return true;
        }
    }
}
