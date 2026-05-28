using Application.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IImportationOrderService
    {

        // 1. LISTAR TODAS LAS ÓRDENES
        Task<IEnumerable<ImportationOrderResponseDTO>> GetAllAsync();

        // 2. CREAR UNA NUEVA ORDEN
        Task<ImportationOrderResponseDTO> CreateAsync(ImportationOrderCreateDTO orderCreateDTO);

        //3, ACTUALIZAR UNA ORDEN EXISTENTE
        Task<ImportationOrderResponseDTO> EditAsync(string id, ImportationOrderUpdateDTO orderUpdateDTO);

        //4. OBTENER POR ID
        Task<ImportationOrderResponseDTO> GetEntityById(string id);

        //5. ELIMINAR UNA ORDEN
        Task<bool> DeleteAsync(string id);


    }
}
