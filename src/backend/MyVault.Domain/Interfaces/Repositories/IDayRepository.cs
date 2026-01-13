using MyVault.Domain.Entities;

namespace MyVault.Domain.Interfaces.Repositories;

public interface IDayRepository
{
    public Task<Day> CreateAsync(Day day);
    public Task<Day?> GetAsync(int id);
    public Task<List<Day>> GetAsync(int limit = 100, int offset = 0);
    public Task<Day?> UpdateAsync(Day day);
    public Task<bool> DeleteAsync(int id);
    public Task<DayItem> CreateItemAsync(DayItem item);
    public Task<DayItem?> UpdateItemAsync(DayItem item);
    public Task<bool> DeleteItemAsync(int id);
}
