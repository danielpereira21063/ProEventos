using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Models;
using System;
using System.Threading.Tasks;

namespace ProEventos.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantePersist _palestrantePersist;
        private readonly IMapper _mapper;

        public PalestranteService(IPalestrantePersist PalestrantePersist, IMapper mapper)
        {
            _palestrantePersist = PalestrantePersist;
            _mapper = mapper;
        }

        public async Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model)
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestrantePersist.Add(palestrante);

                var success = await _palestrantePersist.SaveChangesAsync();

                if (success)
                {
                    var result = _palestrantePersist.GetPalestranteByUserIdAsync(userId);
                    return _mapper.Map<PalestranteDto>(palestrante);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteUpdateDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userId);

                if (palestrante == null) return null;

                model.Id = palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, palestrante);

                _palestrantePersist.Update<Palestrante>(palestrante);

                var success = await _palestrantePersist.SaveChangesAsync();
                if (success)
                {
                    var response = await _palestrantePersist.GetPalestranteByUserIdAsync(userId);
                    return _mapper.Map<PalestranteUpdateDto>(response);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                var Palestrantes = await _palestrantePersist.GetAllPalestrantesAsync(pageParams, includeEventos);
                var resultado = _mapper.Map<PageList<PalestranteDto>>(Palestrantes);

                resultado.CurrentPage = Palestrantes.CurrentPage;
                resultado.TotalPages = Palestrantes.TotalPages;
                resultado.PageSize = Palestrantes.PageSize;
                resultado.TotalCount = Palestrantes.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            try
            {
                var Palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, includeEventos);
                return _mapper.Map<PalestranteDto>(Palestrante);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
