
using BigBang2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigBang2.Repository.Interface
{
    public interface IDoctor<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<bool> Exists(int id);
        Task<IEnumerable<Doctor>> GetDoctorsByActiveStatus(bool isActive);
        Task<IEnumerable<Doctor>> GetDoctorsBySpecialty(string specialty);
    }
}