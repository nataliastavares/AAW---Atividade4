using System;
using System.Collections.Generic;
using System.Text;
using DDD.Domain;
using System.Data;
using System.Data.SQLite;

namespace DDD_ContaCorrente.Service.Services
{
    class ContaCorrenteService
    {
        private DDD.Service.Services.LogService objAuditoria;
        private string _connectionString = @"Data Source=C:\Users\638228\Desktop\AAW - Atividade 04-20190710T225844Z-001\AAW - Atividade 04\DDD_ContaCorrente\DDD.Application\app.db";

        public DataTable LerDados<T>(string query) where T : IDbConnection, new()
        {
            using (var conn = new T())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Connection.ConnectionString = _connectionString;
                    cmd.Connection.Open();
                    var table = new DataTable();
                    table.Load(cmd.ExecuteReader());
                    return table;
                }
            }
        }

        public Boolean criarNovaConta(DDD.Domain.Entities.ContaCorrente conta)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "INSERT INTO ContaCorrente(IdContaCorrente,Saldo,LimiteCredito) VALUES (@Saldo,@LimiteCredito)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@Saldo", conta.saldo);
                    cmd.Parameters.AddWithValue("@LimiteCredito", conta.limiteCredito);
                    try
                    {
                        resultado = cmd.ExecuteNonQuery();
                        if (resultado == 1) {
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
            
        }

        public Boolean definirLimiteCredito(double LimiteCredito)
        {
            int resultado = -1;
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = " UPDATE ContaCorrente SET LimiteCredito = @LimiteCredito ";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@LimiteCredito", LimiteCredito);
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
        }

        public double consultarSaldoConta(int IdConta)
        {
            DataTable conta;
            try
            {
                conta = LerDados<SQLiteConnection>("select saldo from ContaCorrente where idContaCorrente = " + Convert.ToString(IdConta));
                return Convert.ToDouble(conta.Rows[0]["Saldo"]);
            }
            catch (Exception ex)
            {
                throw new Exception("Problemas para exibir saldo: " + ex);
            }
        }

        public Boolean debitarConta(int idConta, double valor)
        {
            int resultado = -1;

            double saldoConta = consultarSaldoConta(idConta);

            if (saldoConta >= valor) {
                using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        cmd.CommandText = " UPDATE ContaCorrente SET LimiteCredito = @LimiteCredito ";
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@LimiteCredito", saldoConta - valor);
                        try
                        {
                            resultado = cmd.ExecuteNonQuery();
                            if (resultado == 1)
                            {
                                objAuditoria.gravarMovimento(idConta, DateTime.Now, valor, Convert.ToChar("D"));
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
                
            }
            else
            {
                return false;
            }
        }

        public Boolean creditarConta(int idConta, double valor)
        {
            int resultado = -1;
            double saldoConta = consultarSaldoConta(idConta);

            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = " UPDATE ContaCorrente SET LimiteCredito = @LimiteCredito ";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@LimiteCredito", saldoConta + valor);
                    try
                    {
                        resultado = cmd.ExecuteNonQuery();
                        if (resultado == 1)
                        {
                            objAuditoria.gravarMovimento(idConta, DateTime.Now, valor, Convert.ToChar("C"));
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
        }
    }
}
