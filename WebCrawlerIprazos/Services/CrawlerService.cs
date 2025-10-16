using HtmlAgilityPack;
using System.Text.RegularExpressions;
using WebCrawlerIprazos.Models;

namespace WebCrawlerIprazos.Services
{
    public class CrawlerService
    {
        private readonly HttpClient _client = new();
        private readonly string _caminhoHtml = Path.Combine("Outputs", "Html");

        public CrawlerService()
        {
            Directory.CreateDirectory(_caminhoHtml);
        }

        public async Task<List<Proxy>> ObterProxiesAsync(string url, int numeroPagina)
        {
            try
            {
                var response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[ERRO] Página {numeroPagina} retornou {response.StatusCode}");
                    return new();
                }

                var html = await response.Content.ReadAsStringAsync();

                
                var arquivo = Path.Combine(_caminhoHtml, $"pagina_{numeroPagina}.html");
                await File.WriteAllTextAsync(arquivo, html);

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var linhas = doc.DocumentNode.SelectNodes("//table/tbody/tr");
                var proxies = new List<Proxy>();

                if (linhas == null || linhas.Count == 0)
                    return proxies;

                foreach (var linha in linhas)
                {
                    var celulas = linha.SelectNodes("td");
                    if (celulas?.Count >= 7)
                    {
                        var proxy = new Proxy
                        {
                            IpAddress = celulas[1].InnerText.Trim(),
                            Port = celulas[2].InnerText.Trim(),
                            Country = celulas[3].InnerText.Trim(),
                            Protocol = celulas[6].InnerText.Trim(),
                            PaginaOrigem = numeroPagina
                        };

                        if (string.IsNullOrWhiteSpace(proxy.IpAddress))
                            continue;

                        proxies.Add(proxy);
                    }
                }

                return proxies;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Erro ao processar página {numeroPagina}: {ex.Message}");
                return new();
            }
        }

        public async Task<int> ObterNumeroTotalPaginasAsync(string urlBase)
        {
            try
            {
                var html = await _client.GetStringAsync(urlBase);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                
                var linkUltima = doc.DocumentNode.SelectSingleNode("//ul[contains(@class,'pagination')]/li[last()]/a");

                if (linkUltima != null && int.TryParse(Regex.Match(linkUltima.InnerText, @"\d+").Value, out int total))
                    return total;

                
                return 1;
            }
            catch
            {
                return 1;
            }
        }
    }
}
