using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProfileWriteRepository
    {
        void Add(Profile profile);
        void Remove(Profile profile);
    }
}
