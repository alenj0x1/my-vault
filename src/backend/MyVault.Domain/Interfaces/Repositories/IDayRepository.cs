using MyVault.Domain.Entities;

namespace MyVault.Domain.Interfaces.Repositories;

public interface IDayRepository
{
    public Task<Day> Create(Day day);
    public Task<DayItem> CreateItem(DayItem item);
    public Task<Day?> Get(int id);
    public Task<List<Day>> Get(int limit = 100, int offset = 0);
    public Task<Day?> Update(Day day);
    public Task<bool> Delete(int id);
}
