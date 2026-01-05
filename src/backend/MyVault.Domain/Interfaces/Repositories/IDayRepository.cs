using MyVault.Domain.Entities;

namespace MyVault.Domain.Interfaces.Repositories;

public interface IDayRepository
{
    public Task<Day> Create(Day day);
    public Task<DayItem> CreateItem(DayItem item);
    public Day? Get(int id);
    public List<Day> Get(int limit = 100, int skip = 0);
    public Day? Update(Day day);
    public bool Delete(int id);
}
