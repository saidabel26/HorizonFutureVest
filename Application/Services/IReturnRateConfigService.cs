using System.Threading.Tasks;

public interface IReturnRateConfigService
{
    Task<ReturnRateConfigDto?> GetConfigAsync();
    Task UpdateConfigAsync(ReturnRateConfigDto dto);
}
