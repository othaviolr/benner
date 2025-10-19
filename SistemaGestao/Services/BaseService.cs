using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SistemaGestao.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected string CaminhoArquivo { get; set; }
        protected List<T> Dados { get; set; }

        protected BaseService(string nomeArquivo)
        {
            string pastaData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

            if (!Directory.Exists(pastaData))
                Directory.CreateDirectory(pastaData);

            CaminhoArquivo = Path.Combine(pastaData, nomeArquivo);
            Dados = new List<T>();

            CarregarDados();
        }

        protected void CarregarDados()
        {
            try
            {
                if (File.Exists(CaminhoArquivo))
                {
                    string json = File.ReadAllText(CaminhoArquivo);

                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        Dados = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar dados: {ex.Message}");
                Dados = new List<T>();
            }
        }

        protected void SalvarDados()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Dados, Formatting.Indented);
                File.WriteAllText(CaminhoArquivo, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar dados: {ex.Message}");
                throw new Exception($"Erro ao salvar dados: {ex.Message}");
            }
        }

        public virtual List<T> ObterTodos()
        {
            return Dados.ToList();
        }
    }
}